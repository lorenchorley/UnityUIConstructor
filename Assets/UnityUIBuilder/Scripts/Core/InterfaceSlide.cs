using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfaceSlide : InterfaceElement {

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

            P panel = UnityHelper.NewComponent<P>((Coordinator.IndicateTypesInHierarchy ? "Panel: " : "") + name, transform);
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

        public InterfaceSlide SetToFillParent() {
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.localPosition = Vector2.zero;
            RectTransform.sizeDelta = Vector2.zero;
            return this;
        }

        public InterfaceSlide SetToFillCanvas() {
            Canvas Canvas = GetComponentInParent<Canvas>();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.zero;
            RectTransform.sizeDelta = Canvas.pixelRect.size;
            RectTransform.position = Canvas.transform.position;
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