using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfaceControl : InterfaceElement {

        public static readonly Vector2 v00 = Vector2.zero;
        public static readonly Vector2 v0h = new Vector2(0, 0.5f);
        public static readonly Vector2 v01 = Vector2.up;
        public static readonly Vector2 vh0 = new Vector2(0.5f, 0);
        public static readonly Vector2 vhh = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 vh1 = new Vector2(0.5f, 1);
        public static readonly Vector2 v10 = Vector2.right;
        public static readonly Vector2 v1h = new Vector2(1, 0.5f);
        public static readonly Vector2 v11 = Vector2.one;

        public void SetupControl() {
            Setup();
        }

        public InterfacePanel ParentElement;
        public InterfaceSlide ParentSlide;
        public string ControlName;

        public InterfacePanel Return() {
            return ParentElement;
        }

        public override ElementLayoutType GetLayoutType() {
            return ElementLayoutType.NotApplicable;
        }

        public E Return<E>() where E : InterfacePanel {
            return (E) ParentElement;
        }

        public InterfaceControl Process(Action<InterfaceControl> action) {
            action.Invoke(this);
            return this;
        }

        public InterfaceControl ProcessIf(bool condition, Action<InterfaceControl> action) {
            if (condition)
                action.Invoke(this);
            return this;
        }

        public InterfaceControl FitToContentHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfaceControl FitToContentVertically() {
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfaceControl UnconstrainHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            return this;
        }

        public InterfaceControl UnconstrainVertically() {
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            return this;
        }

        public InterfaceControl FitToContent() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfaceControl SetMinWidth(float value) {
            LayoutElement.minWidth = value;
            return this;
        }

        public InterfaceControl SetMinHeight(float value) {
            LayoutElement.minHeight = value;
            return this;
        }

        public InterfaceControl SetMinSize(float width, float height) {
            LayoutElement.minWidth = width;
            LayoutElement.minHeight = height;
            return this;
        }

        public InterfaceControl SetPreferredWidth(float value) {
            LayoutElement.preferredWidth = value;
            return this;
        }

        public InterfaceControl SetPreferredHeight(float value) {
            LayoutElement.preferredHeight = value;
            return this;
        }

        public InterfaceControl SetPreferredSize(float width, float height) {
            LayoutElement.preferredWidth = width;
            LayoutElement.preferredHeight = height;
            return this;
        }

        public InterfaceControl SetFlexibleHeight(float value) {
            LayoutElement.flexibleHeight = value;
            return this;
        }

        public InterfaceControl SetFlexibleWidth(float value) {
            LayoutElement.flexibleWidth = value;
            return this;
        }

        public InterfaceControl SetFlexibleSize(float width, float height) {
            LayoutElement.flexibleWidth = width;
            LayoutElement.flexibleHeight = height;
            return this;
        }

        public InterfaceControl IgnoreLayout() {
            LayoutElement.ignoreLayout = true;
            return this;
        }

        public InterfaceControl SetColour(Color color) {
            Image.color = color;
            return this;
        }

        public InterfaceControl SetColour(float grey) {
            Image.color = new Color(grey, grey, grey);
            return this;
        }

        public InterfaceControl Position(Anchors anchors, bool pivot = true, bool position = true) {
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
            case Anchors.BottomLeft:
                RectTransform.anchorMin = v00;
                RectTransform.anchorMax = v00;
                if (pivot)
                    RectTransform.pivot = v00;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomCenter:
                RectTransform.anchorMin = vh0;
                RectTransform.anchorMax = vh0;
                if (pivot)
                    RectTransform.pivot = vh0;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomRight:
                RectTransform.anchorMin = v10;
                RectTransform.anchorMax = v10;
                if (pivot)
                    RectTransform.pivot = v10;
                if (position)
                    RectTransform.anchoredPosition = v00;
                break;
            case Anchors.BottomStretch:
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

        public InterfaceControl SetMouseHoverActions(UnityAction OnEnter, UnityAction OnExit) {
            MouseOverDetector RespondOnMouseOver = GetComponent<MouseOverDetector>();
            if (RespondOnMouseOver == null) {
                RespondOnMouseOver = gameObject.AddComponent<MouseOverDetector>();
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