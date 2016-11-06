using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfacePanel : InterfaceElement {

        public static readonly Vector2 v00 = Vector2.zero;
        public static readonly Vector2 v0h = new Vector2(0, 0.5f);
        public static readonly Vector2 v01 = Vector2.up;
        public static readonly Vector2 vh0 = new Vector2(0.5f, 0);
        public static readonly Vector2 vhh = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 vh1 = new Vector2(0.5f, 1);
        public static readonly Vector2 v10 = Vector2.right;
        public static readonly Vector2 v1h = new Vector2(1, 0.5f);
        public static readonly Vector2 v11 = Vector2.one;

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

            C control = Coordinator.InstantiateControl<C>(name, transform);
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

            P panel = Coordinator.InstantiatePanel<P>(name, transform);
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

        public List<P> GetPanelsInChildren<P>() where P : InterfacePanel {
            List<P> matchingPanels = new List<P>();
            foreach (InterfacePanel panel in subpanels.Values) {
                if (panel is P)
                    matchingPanels.Add(panel as P);
                matchingPanels.AddRange(panel.GetPanelsInChildren<P>());
            }
            return matchingPanels;
        }

        public List<C> GetControlsInChildren<C>() where C : InterfaceControl {
            List<C> matchingControls = new List<C>();
            foreach (InterfaceControl control in controls.Values) {
                if (control is C)
                    matchingControls.Add(control as C);
            }
            foreach (InterfacePanel panel in subpanels.Values) {
                matchingControls.AddRange(panel.GetControlsInChildren<C>());
            }
            return matchingControls;
        }

        public virtual InterfacePanel ClearContents() {
            foreach (InterfaceControl control in controls.Values) {
                control.transform.SetParent(null);
                Destroy(control.gameObject);
            }
            controls.Clear();
            foreach (InterfacePanel panel in subpanels.Values) {
                panel.transform.SetParent(null);
                Destroy(panel.gameObject);
            }
            subpanels.Clear();
            return this;
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

        public InterfacePanel SetVerticalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0, TextAnchor alignment = TextAnchor.UpperLeft) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            VerticalLayoutGroup.padding = padding;
            VerticalLayoutGroup.spacing = spacing;
            VerticalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            VerticalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;
            VerticalLayoutGroup.childAlignment = alignment;

            LayoutType = ElementLayoutType.Vertical;

            return this;
        }

        public InterfacePanel SetHorizonalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0, TextAnchor alignment = TextAnchor.UpperLeft) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            HorizontalLayoutGroup.padding = padding;
            HorizontalLayoutGroup.spacing = spacing;
            HorizontalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            HorizontalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;
            HorizontalLayoutGroup.childAlignment = alignment;

            LayoutType = ElementLayoutType.Horizonal;


            return this;
        }

        public InterfacePanel SetGridLayout(TextAnchor alignment = TextAnchor.UpperLeft, GridLayoutGroup.Corner corner = GridLayoutGroup.Corner.LowerLeft, GridLayoutGroup.Axis startAxis = GridLayoutGroup.Axis.Horizontal, RectOffset padding = null, float hspacing = 0, float vspacing = 0, Vector2? cellSize = null, GridLayoutGroup.Constraint constraint = GridLayoutGroup.Constraint.Flexible, int contraintCount = 1) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);
            if (!cellSize.HasValue)
                cellSize = new Vector2(50, 50);

            GridLayoutGroup.padding = padding;
            GridLayoutGroup.spacing = new Vector2(hspacing, vspacing);
            GridLayoutGroup.startCorner = corner;
            GridLayoutGroup.startAxis = startAxis;
            GridLayoutGroup.childAlignment = alignment;
            GridLayoutGroup.cellSize = cellSize.Value;
            GridLayoutGroup.constraint = constraint;
            GridLayoutGroup.constraintCount = contraintCount;

            LayoutType = ElementLayoutType.Grid;

            return this;
        }
        
        public InterfacePanel SetToFillCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = v00;
            RectTransform.anchorMax = v00;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
            return this;
        }

        public InterfacePanel FitToContentHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfacePanel FitToContentVertically() {
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

        public InterfacePanel FitToContent() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfacePanel SetMinWidth(float value) {
            LayoutElement.minWidth = value;
            return this;
        }

        public InterfacePanel SetMinHeight(float value) {
            LayoutElement.minHeight = value;
            return this;
        }

        public InterfacePanel SetMinSize(float width, float height) {
            LayoutElement.minWidth = width;
            LayoutElement.minHeight = height;
            return this;
        }

        public InterfacePanel SetPreferredWidth(float value) {
            LayoutElement.preferredWidth = value;
            return this;
        }

        public InterfacePanel SetPreferredHeight(float value) {
            LayoutElement.preferredHeight = value;
            return this;
        }

        public InterfacePanel SetPreferredSize(float width, float height) {
            LayoutElement.preferredWidth = width;
            LayoutElement.preferredHeight = height;
            return this;
        }

        public InterfacePanel SetFlexibleHeight(float value) {
            LayoutElement.flexibleHeight = value;
            return this;
        }

        public InterfacePanel SetFlexibleWidth(float value) {
            LayoutElement.flexibleWidth = value;
            return this;
        }

        public InterfacePanel SetFlexibleSize(float width, float height) {
            LayoutElement.flexibleWidth = width;
            LayoutElement.flexibleHeight = height;
            return this;
        }

        public InterfacePanel IgnoreLayout() {
            LayoutElement.ignoreLayout = true;
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
            Position(Anchors.StretchBoth);
            return this;
        }

        public InterfacePanel SetToCoverCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = v00;
            RectTransform.anchorMax = v00;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
            return this;
        }

        public InterfacePanel Position(Anchors anchors, bool pivot = true, bool position = true) {
            switch (anchors) {
            case Anchors.TopLeft:
                RectTransform.anchorMin = v01;
                RectTransform.anchorMax = v01;
                if (pivot) RectTransform.pivot = v01;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopCenter:
                RectTransform.anchorMin = vh1;
                RectTransform.anchorMax = vh1;
                if (pivot) RectTransform.pivot = vh1;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopRight:
                RectTransform.anchorMin = v11;
                RectTransform.anchorMax = v11;
                if (pivot) RectTransform.pivot = v11;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopStretch:
                RectTransform.anchorMin = v01;
                RectTransform.anchorMax = v11;
                if (pivot) RectTransform.pivot = vh1;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.CenterLeft:
                RectTransform.anchorMin = v0h;
                RectTransform.anchorMax = v0h;
                if (pivot) RectTransform.pivot = v0h;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterBoth:
                RectTransform.anchorMin = vhh;
                RectTransform.anchorMax = vhh;
                if (pivot) RectTransform.pivot = vhh;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterRight:
                RectTransform.anchorMin = v1h;
                RectTransform.anchorMax = v1h;
                if (pivot) RectTransform.pivot = v1h;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterStretch:
                RectTransform.anchorMin = v0h;
                RectTransform.anchorMax = v1h;
                if (pivot) RectTransform.pivot = vhh;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.BottomLeft: 
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v00;
                if (pivot) RectTransform.pivot = v00;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomCenter: 
                RectTransform.anchorMin = vh0;
                RectTransform.anchorMax = vh0;
                if (pivot) RectTransform.pivot = vh0;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomRight: 
                RectTransform.anchorMin = v10;
                RectTransform.anchorMax = v10;
                if (pivot) RectTransform.pivot = v10;
                if (position) RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomStretch: 
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v10;
                if (pivot) RectTransform.pivot = vh0;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.StretchLeft:
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v01;
                if (pivot) RectTransform.pivot = v0h;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchCenter:
                RectTransform.anchorMin = vh0;
                RectTransform.anchorMax = vh1;
                if (pivot) RectTransform.pivot = vhh;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchRight:
                RectTransform.anchorMin = v10;
                RectTransform.anchorMax = v11;
                if (pivot) RectTransform.pivot = v1h;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchBoth: 
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v11;
                if (pivot) RectTransform.pivot = vhh;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = v00;
                }
                break;
            default:
                throw new Exception("Position not supported: " + anchors);
            }
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