using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityUIConstructor {

    [AddComponentMenu("UI/Angler")]
    [RequireComponent(typeof(RectTransform))]
    public class Angler : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement {

        [Serializable]
        public class AnglerEvent : UnityEvent<float> { }

        public Image BackgroundGraphic;
        public Image HandleGraphic;

        [SerializeField]
        private RectTransform m_HandleRect;
        public RectTransform handleRect { get { return m_HandleRect; } set { if (SetPropertyUtility.SetClass(ref m_HandleRect, value)) { UpdateCachedReferences(); UpdateVisuals(); } } }

        [Serializable]
        public class ToggleButtonSettings {
            public Button Button;
            public Sprite Enabled;
            public Sprite Disabled;

            [NonSerialized]
            public bool ButtonFunctionsSet = false;

        }
        public ToggleButtonSettings ToggleButton;

        [Space]
        
        [SerializeField]
        private float m_MinValue = 0;
        public float minValue { get { return m_MinValue; } set { if (SetPropertyUtility.SetStruct(ref m_MinValue, value)) { Set(m_Value); UpdateVisuals(); } } }

        [SerializeField]
        private float m_MaxValue = 360;
        public float maxValue { get { return m_MaxValue; } set { if (SetPropertyUtility.SetStruct(ref m_MaxValue, value)) { Set(m_Value); UpdateVisuals(); } } }
        
        [SerializeField]
        protected float m_Value;
        public virtual float value
        {
            get
            {
                return m_Value;
            }
            set
            {
                Set(value);
            }
        }

        public float normalizedValue
        {
            get
            {
                if (Mathf.Approximately(minValue, maxValue))
                    return 0;
                return Mathf.InverseLerp(minValue, maxValue, value);
            }
            set
            {
                this.value = Mathf.Lerp(minValue, maxValue, value);
            }
        }

        [Space]

        // Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
        [SerializeField]
        private AnglerEvent m_OnValueChanged = new AnglerEvent();
        public AnglerEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        // Private fields

        private Transform m_HandleTransform;
        private RectTransform m_HandleContainerRect;

        private DrivenRectTransformTracker m_Tracker;

        public float angle;

        protected Angler() { }

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();

            //Onvalidate is called before OnEnabled. We need to make sure not to touch any other objects before OnEnable is run.
            if (IsActive()) {
                UpdateCachedReferences();
                Set(m_Value, false);
                // Update rects since other things might affect them even if value didn't change.
                UpdateVisuals();
            }

            var prefabType = UnityEditor.PrefabUtility.GetPrefabType(this);
            if (prefabType != UnityEditor.PrefabType.Prefab && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing) {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(value);
#endif
        }

        public virtual void LayoutComplete() { }

        public virtual void GraphicUpdateComplete() { }

        protected override void OnEnable() {
            base.OnEnable();
            UpdateCachedReferences();
            Set(m_Value, false);
            // Update rects since they need to be initialized correctly.
            UpdateVisuals();

            if (BackgroundGraphic != null) {
                Color c = BackgroundGraphic.color;
                c.a = 1;
                BackgroundGraphic.color = c;
            }
        }

        protected override void OnDisable() {
            m_Tracker.Clear();
            base.OnDisable();

            if (BackgroundGraphic != null) {
                Color c = BackgroundGraphic.color;
                c.a = 0.25f;
                BackgroundGraphic.color = c;
            }

            EnableOnClick eoc = GetComponent<EnableOnClick>();
            if (eoc == null)
                eoc = gameObject.AddComponent<EnableOnClick>();
            eoc.ToEnable = this;

        }

        protected override void OnDidApplyAnimationProperties() {
            // Has value changed? Various elements of the slider have the old normalisedValue assigned, we can use this to perform a comparison.
            // We also need to ensure the value stays within min/max.
            m_Value = ClampValue(m_Value);
            float oldNormalizedValue = normalizedValue;
            if (m_HandleContainerRect != null)
                oldNormalizedValue = ClampValue(m_HandleRect.eulerAngles.z) / (m_MaxValue - m_MinValue);

            UpdateVisuals();

            if (oldNormalizedValue != normalizedValue)
                onValueChanged.Invoke(m_Value);
        }

        void UpdateCachedReferences() {
            if (m_HandleRect) {
                m_HandleTransform = m_HandleRect.transform;
                if (m_HandleTransform.parent != null)
                    m_HandleContainerRect = m_HandleTransform.parent.GetComponent<RectTransform>();
            } else {
                m_HandleContainerRect = null;
            }
        }

        float ClampValue(float input) {
            return Mathf.Clamp(input, minValue, maxValue);
        }

        // Set the valueUpdate the visible Image.
        void Set(float input) {
            Set(input, true);
        }

        protected virtual void Set(float input, bool sendCallback) {
            // Clamp the input
            float newValue = ClampValue(input);

            // If the stepped value doesn't match the last one, it's time to update
            if (m_Value == newValue)
                return;

            m_Value = newValue;
            UpdateVisuals();
            if (sendCallback)
                m_OnValueChanged.Invoke(newValue);
        }

        protected override void OnRectTransformDimensionsChange() {
            base.OnRectTransformDimensionsChange();

            //This can be invoked before OnEnabled is called. So we shouldn't be accessing other objects, before OnEnable is called.
            if (!IsActive())
                return;

            UpdateVisuals();
        }
        
        // Force-update the slider. Useful if you've changed the properties and want it to update visually.
        private void UpdateVisuals() {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UpdateCachedReferences();
#endif

            m_Tracker.Clear();
            
            if (m_HandleContainerRect != null) {
                m_Tracker.Add(this, m_HandleRect, DrivenTransformProperties.Rotation);

                m_HandleRect.anchoredPosition = Vector2.zero;
                m_HandleRect.anchorMin = new Vector2(0.5f, 0.5f);
                m_HandleRect.anchorMax = new Vector2(0.5f, 0.5f);
                m_HandleRect.eulerAngles = new Vector3(m_HandleRect.eulerAngles.x, m_HandleRect.eulerAngles.y, m_Value + 0);
            }

            if (ToggleButton.Button != null) {
                if (enabled)
                    ToggleButton.Button.image.sprite = ToggleButton.Disabled;
                else
                    ToggleButton.Button.image.sprite = ToggleButton.Enabled;

                if (!ToggleButton.ButtonFunctionsSet) {
                    ToggleButton.ButtonFunctionsSet = true;
                    ToggleButton.Button.onClick.AddListener(() => {
                        if (enabled) {
                            enabled = false;
                            ToggleButton.Button.image.sprite = ToggleButton.Enabled;
                        } else {
                            enabled = true;
                            ToggleButton.Button.image.sprite = ToggleButton.Disabled;
                        }
                    });
                }
            }

        }

        // Update the slider's position based on the mouse.
        void UpdateDrag(PointerEventData eventData, Camera cam) {
            if (m_HandleContainerRect != null) {
                Vector3 mouseDelta = Input.mousePosition - m_HandleRect.position;
                angle = Mathf.Atan2(mouseDelta.x, -mouseDelta.y) * Mathf.Rad2Deg + 180;

                normalizedValue = Mathf.Clamp01(angle / (m_MaxValue - m_MinValue));
            }
        }

        private bool MayDrag(PointerEventData eventData) {
            return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
        }

        public override void OnPointerDown(PointerEventData eventData) {
            if (!MayDrag(eventData))
                return;

            base.OnPointerDown(eventData);

            if (m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(m_HandleRect, eventData.position, eventData.enterEventCamera)) {
                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(m_HandleRect, eventData.position, eventData.pressEventCamera, out localMousePos);
            } else {
                // Outside the slider handle - jump to this point instead
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        public virtual void OnDrag(PointerEventData eventData) {
            if (!MayDrag(eventData))
                return;
            UpdateDrag(eventData, eventData.pressEventCamera);
        }
        
        public virtual void OnInitializePotentialDrag(PointerEventData eventData) {
            eventData.useDragThreshold = false;
        }

    }
}