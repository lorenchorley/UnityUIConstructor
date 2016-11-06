using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfaceSlide : InterfaceElement {

        public static readonly Vector2 v00 = Vector2.zero;
        public static readonly Vector2 v0h = new Vector2(0, 0.5f);
        public static readonly Vector2 v01 = Vector2.up;
        public static readonly Vector2 vh0 = new Vector2(0.5f, 0);
        public static readonly Vector2 vhh = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 vh1 = new Vector2(0.5f, 1);
        public static readonly Vector2 v10 = Vector2.right;
        public static readonly Vector2 v1h = new Vector2(1, 0.5f);
        public static readonly Vector2 v11 = Vector2.one;

        public void SetupSlide() {
            OnShow = new UnityEvent();
            OnHide = new UnityEvent();
            Setup();
        }

        public string SlideName;
        public UnityEvent OnShow;
        public UnityEvent OnHide;

        protected ElementLayoutType LayoutType = ElementLayoutType.NotSet;
        protected Dictionary<string, InterfacePanel> panels = new Dictionary<string, InterfacePanel>();

        public P AddPanel<P>(string name) where P : InterfacePanel {
            if (panels.ContainsKey(name))
                throw new Exception("Slide " + SlideName + " already has a panel called " + name);

            P panel = Coordinator.InstantiatePanel<P>(name, transform);
            panel.PanelName = name;
            panel.Coordinator = Coordinator;
            panel.ParentElement = this;
            panel.ParentSlide = this;
            panels.Add(name, panel);
            panel.SetupPanel();
            return panel;
        }

        public P AddPanel<P>(string name, out P panel) where P : InterfacePanel {
            panel = AddPanel<P>(name);
            return panel;
        }

        public P GetPanel<P>(string name) where P : InterfacePanel {
            InterfacePanel panel;
            if (panels.TryGetValue(name, out panel)) {
                if (panel is P)
                    return panel as P;
                else
                    throw new Exception("Panel was not of type " + typeof(P));
            } else
                throw new Exception("No panel of name '" + name + "' was found on slide '" + SlideName + "'");
        }

        public List<P> GetPanelsInChildren<P>() where P : InterfacePanel {
            List<P> matchingPanels = new List<P>();
            foreach (InterfacePanel panel in panels.Values) {
                if (panel is P)
                    matchingPanels.Add(panel as P);
                matchingPanels.AddRange(panel.GetPanelsInChildren<P>());
            }
            return matchingPanels;
        }

        public List<C> GetControlsInChildren<C>() where C : InterfaceControl {
            List<C> matchingControls = new List<C>();
            foreach (InterfacePanel panel in panels.Values) {
                matchingControls.AddRange(panel.GetControlsInChildren<C>());
            }
            return matchingControls;
        }

        public InterfaceSlide ClearContents() {
            foreach (InterfacePanel panel in panels.Values) {
                panel.transform.SetParent(null);
                Destroy(panel.gameObject);
            }
            panels.Clear();
            return this;
        }

        public InterfaceSlide Process(Action<InterfaceSlide> action) {
            action.Invoke(this);
            return this;
        }

        public InterfaceSlide ProcessIf(bool condition, Action<InterfaceSlide> action) {
            if (condition)
                action.Invoke(this);
            return this;
        }

        public void SetVisibility(bool visible) {
            gameObject.SetActive(visible);
            if (visible) {
                if (OnShow != null)
                    OnShow.Invoke();
            } else {
                if (OnHide != null)
                    OnHide.Invoke();
            }
        }

        public override ElementLayoutType GetLayoutType() {
            return LayoutType;
        }

        public InterfaceSlide SetVerticalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            VerticalLayoutGroup.padding = padding;
            VerticalLayoutGroup.spacing = spacing;
            VerticalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            VerticalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;

            LayoutType = ElementLayoutType.Vertical;

            return this;
        }

        public InterfaceSlide SetHorizonalLayout(Expand expand = Expand.None, RectOffset padding = null, float spacing = 0) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            HorizontalLayoutGroup.padding = padding;
            HorizontalLayoutGroup.spacing = spacing;
            HorizontalLayoutGroup.childForceExpandWidth = expand == Expand.Horizontal || expand == Expand.Both;
            HorizontalLayoutGroup.childForceExpandHeight = expand == Expand.Vertical || expand == Expand.Both;

            LayoutType = ElementLayoutType.Horizonal;

            return this;
        }

        public InterfaceSlide SetGridLayout(TextAnchor alignment = TextAnchor.UpperLeft, RectOffset padding = null, float hspacing = 0, float vspacing = 0, float cellWidth = 50, float cellHeight = 50) {
            if (padding == null)
                padding = new RectOffset(0, 0, 0, 0);

            GridLayoutGroup.padding = padding;
            GridLayoutGroup.spacing = new Vector2(hspacing, vspacing);
            GridLayoutGroup.childAlignment = alignment;
            GridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

            LayoutType = ElementLayoutType.Grid;

            return this;
        }

        public InterfaceSlide SetToCoverParent() {
            Position(Anchors.StretchBoth);
            return this;
        }

        public InterfaceSlide SetToCoverCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.zero;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
            return this;
        }

        public InterfaceSlide Position(Anchors anchors, bool pivot = true, bool position = true) {
            switch (anchors) {
            case Anchors.TopLeft:
                RectTransform.anchorMin = v01;
                RectTransform.anchorMax = v01;
                if (pivot)
                    RectTransform.pivot = v01;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopCenter:
                RectTransform.anchorMin = vh1;
                RectTransform.anchorMax = vh1;
                if (pivot)
                    RectTransform.pivot = vh1;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopRight:
                RectTransform.anchorMin = v11;
                RectTransform.anchorMax = v11;
                if (pivot)
                    RectTransform.pivot = v11;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.TopStretch:
                RectTransform.anchorMin = v01;
                RectTransform.anchorMax = v11;
                if (pivot)
                    RectTransform.pivot = vh1;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.CenterLeft:
                RectTransform.anchorMin = v0h;
                RectTransform.anchorMax = v0h;
                if (pivot)
                    RectTransform.pivot = v0h;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterBoth:
                RectTransform.anchorMin = vhh;
                RectTransform.anchorMax = vhh;
                if (pivot)
                    RectTransform.pivot = vhh;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterRight:
                RectTransform.anchorMin = v1h;
                RectTransform.anchorMax = v1h;
                if (pivot)
                    RectTransform.pivot = v1h;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.CenterStretch:
                RectTransform.anchorMin = v0h;
                RectTransform.anchorMax = v1h;
                if (pivot)
                    RectTransform.pivot = vhh;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.BottomLeft: // Done
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v00;
                if (pivot)
                    RectTransform.pivot = v00;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomCenter: // Done
                RectTransform.anchorMin = vh0;
                RectTransform.anchorMax = vh0;
                if (pivot)
                    RectTransform.pivot = vh0;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomRight: // Done
                RectTransform.anchorMin = v10;
                RectTransform.anchorMax = v10;
                if (pivot)
                    RectTransform.pivot = v10;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomStretch: // Done
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v10;
                if (pivot)
                    RectTransform.pivot = vh0;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(0, RectTransform.rect.height);
                }
                break;
            case Anchors.StretchLeft:
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v01;
                if (pivot)
                    RectTransform.pivot = v0h;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchCenter:
                RectTransform.anchorMin = vh0;
                RectTransform.anchorMax = vh1;
                if (pivot)
                    RectTransform.pivot = vhh;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchRight:
                RectTransform.anchorMin = v10;
                RectTransform.anchorMax = v11;
                if (pivot)
                    RectTransform.pivot = v1h;
                if (position) {
                    RectTransform.anchoredPosition = v00;
                    RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, 0);
                }
                break;
            case Anchors.StretchBoth:
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v11;
                if (pivot)
                    RectTransform.pivot = vhh;
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

        public void SetOnHideAction(UnityAction action) {
            OnHide.AddListener(action);
        }

        public void SetOnShowAction(UnityAction action) {
            OnShow.AddListener(action);
        }

        public InterfaceSlide SetColour(Color color) {
            Image.color = color;
            return this;
        }

        public InterfaceSlide SetColour(float grey) {
            Image.color = new Color(grey, grey, grey);
            return this;
        }

    }

}