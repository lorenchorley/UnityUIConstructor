using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public abstract class InterfaceControl : InterfaceElement {

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

        public InterfaceControl FitContentHorizonally() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfaceControl FitContentVertically() {
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

        public InterfaceControl FitContent() {
            ContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            ContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return this;
        }

        public InterfaceControl SetMinHeight(float value) {
            LayoutElement.minHeight = value;
            return this;
        }

        public InterfaceControl SetPreferredHeight(float value) {
            LayoutElement.preferredHeight = value;
            return this;
        }

        public InterfaceControl SetFlexibleHeight(float value) {
            LayoutElement.flexibleHeight = value;
            return this;
        }

        public InterfaceControl SetMinWidth(float value) {
            LayoutElement.minWidth = value;
            return this;
        }

        public InterfaceControl SetPreferredWidth(float value) {
            LayoutElement.preferredWidth = value;
            return this;
        }

        public InterfaceControl SetFlexibleWidth(float value) {
            LayoutElement.flexibleWidth = value;
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