using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfacePanel : InterfaceElement {

        public void SetupPanel() {
            Setup();
        }


        public InterfaceElement ParentElement;
        public InterfaceSlide ParentSlide;
        public string PanelName;

        protected ElementLayoutType LayoutType = ElementLayoutType.NotSet;
        protected Dictionary<string, InterfacePanel> subpanels = new Dictionary<string, InterfacePanel>();
        protected Dictionary<string, InterfaceControl> controls = new Dictionary<string, InterfaceControl>();

        public C AddControl<C>(string name) where C : InterfaceControl {
            if (controls.ContainsKey(name))
                throw new Exception("Panel " + PanelName + " already has a control called " + name);

            C control = UnityHelper.NewComponent<C>((Coordinator.IndicateTypesInHierarchy ? "Control: " : "") + name, transform);
            control.ControlName = name;
            control.Coordinator = Coordinator;
            control.ParentElement = this;
            control.ParentSlide = ParentSlide;
            controls.Add(name, control);
            control.SetupControl();
            return control;
        }

        public C AddControl<C>(string name, out C control) where C : InterfaceControl {
            control = AddControl<C>(name);
            return control;
        }

        public C GetControl<C>(string name) where C : InterfaceControl {
            InterfaceControl control;
            if (controls.TryGetValue(name, out control)) {
                if (control is C)
                    return control as C;
                else
                    throw new Exception("Control was not of type " + typeof(C));
            } else
                throw new Exception("No control of name '" + name + "' was found on panel '" + PanelName + "'");
        }

        public P AddPanel<P>(string name) where P : InterfacePanel {
            if (subpanels.ContainsKey(name))
                throw new Exception("Panel " + PanelName + " already has a subpanel called " + name);

            P panel = UnityHelper.NewComponent<P>((Coordinator.IndicateTypesInHierarchy ? "Panel: " : "") + name, transform);
            panel.PanelName = name;
            panel.Coordinator = Coordinator;
            panel.ParentElement = this;
            panel.ParentSlide = ParentSlide;
            subpanels.Add(name, panel);
            panel.Setup();
            return panel;
        }

        public P AddPanel<P>(string name, out P panel) where P : InterfacePanel {
            panel = AddPanel<P>(name);
            return panel;
        }

        public P GetPanel<P>(string name) where P : InterfacePanel {
            InterfacePanel panel;
            if (subpanels.TryGetValue(name, out panel)) {
                if (panel is P)
                    return panel as P;
                else
                    throw new Exception("Panel was not of type " + typeof(P));
            } else
                throw new Exception("No panel of name '" + name + "' was found on panel '" + PanelName + "'");
        }

        public InterfaceSlide Return() {
            return (InterfaceSlide) ParentElement;
        }

        public InterfacePanel ReturnPanel() {
            return (InterfacePanel) ParentElement;
        }

        public E Return<E>() where E : InterfaceElement {
            return (E) ParentElement;
        }

        public InterfacePanel Process(Action<InterfacePanel> action) {
            action.Invoke(this);
            return this;
        }

        public InterfacePanel Process(Action<InterfacePanel, object[]> action, object[] args) {
            action.Invoke(this, args);
            return this;
        }

        public InterfacePanel ProcessIf(bool condition, Action<InterfacePanel> action) {
            if (condition)
                action.Invoke(this);
            return this;
        }

        public InterfacePanel ForEach<O>(IEnumerable<O> enumerable, Action<InterfacePanel, O> action) {
            foreach (O o in enumerable) {
                action.Invoke(this, o);
            }
            return this;
        }

        public override ElementLayoutType GetLayoutType() {
            return LayoutType;
        }

        public InterfacePanel SetVerticalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            VerticalLayoutGroup.padding = padding;
            VerticalLayoutGroup.spacing = spacing;
            VerticalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            VerticalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;

            LayoutType = ElementLayoutType.Vertical;

            return this;
        }

        public InterfacePanel SetHorizonalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            HorizontalLayoutGroup.padding = padding;
            HorizontalLayoutGroup.spacing = spacing;
            HorizontalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            HorizontalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;

            LayoutType = ElementLayoutType.Horizonal;

            return this;
        }

        public InterfacePanel SetGridLayout(TextAnchor alignment = TextAnchor.UpperLeft, RectOffset padding = null, float hspacing = 0, float vspacing = 0, Vector2? cellSize = null) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);
            if (!cellSize.HasValue)
                cellSize = new Vector2(50, 50);

            GridLayoutGroup.padding = padding;
            GridLayoutGroup.spacing = new Vector2(hspacing, vspacing);
            GridLayoutGroup.childAlignment = alignment;
            GridLayoutGroup.cellSize = cellSize.Value;

            LayoutType = ElementLayoutType.Grid;

            return this;
        }

        public InterfacePanel SetToFillParent() {
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.localPosition = Vector2.zero;
            RectTransform.sizeDelta = Vector2.zero;
            return this;
        }

        public InterfacePanel SetToFillCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.zero;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
            return this;
        }

        public InterfacePanel FitContentHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfacePanel FitContentVertically() {
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfacePanel UnconstrainHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            return this;
        }

        public InterfacePanel UnconstrainVertically() {
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            return this;
        }

        public InterfacePanel FitContent() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfacePanel SetMinHeight(float value) {
            LayoutElement.minHeight = value;
            return this;
        }

        public InterfacePanel SetPreferredHeight(float value) {
            LayoutElement.preferredHeight = value;
            return this;
        }

        public InterfacePanel SetFlexibleHeight(float value) {
            LayoutElement.flexibleHeight = value;
            return this;
        }

        public InterfacePanel SetMinWidth(float value) {
            LayoutElement.minWidth = value;
            return this;
        }

        public InterfacePanel SetPreferredWidth(float value) {
            LayoutElement.preferredWidth = value;
            return this;
        }

        public InterfacePanel SetFlexibleWidth(float value) {
            LayoutElement.flexibleWidth = value;
            return this;
        }

        public InterfacePanel SetColour(float grey) {
            Image.color = new Color(grey, grey, grey);
            return this;
        }

        public InterfacePanel SetColour(Color color) {
            Image.color = color;
            return this;
        }

        public InterfacePanel SetToCoverParent() {
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.localPosition = Vector2.zero;
            RectTransform.sizeDelta = Vector2.zero;
            return this;
        }

        public InterfacePanel SetToCoverCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.zero;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
            return this;
        }

        public InterfacePanel SetMouseHoverActions(UnityAction OnEnter, UnityAction OnExit, GameObject TargetObject = null) {
            if (TargetObject == null)
                TargetObject = gameObject;

            MouseOverDetector RespondOnMouseOver = TargetObject.GetComponent<MouseOverDetector>();
            if (RespondOnMouseOver == null) {
                RespondOnMouseOver = TargetObject.AddComponent<MouseOverDetector>();
                RespondOnMouseOver.Initialise();
            }

            RespondOnMouseOver.MouseEnter.RemoveAllListeners();
            RespondOnMouseOver.MouseEnter.AddListener(OnEnter);

            RespondOnMouseOver.MouseExit.RemoveAllListeners();
            RespondOnMouseOver.MouseExit.AddListener(OnExit);

            return this;
        }

    }

}