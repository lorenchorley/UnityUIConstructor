using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UnityUIConstructor {

    public class ResizeControl : InterfaceControl {

        bool? fitVertically;
        bool updateContinuously = true;
        SplitResizablePanel ParentSplitResizablePanel;

        protected override void Setup() {

            ParentSplitResizablePanel = (SplitResizablePanel) ParentElement;

            RequestImage();

        }

        public ResizeControl SetUpdateContinuous() {
            updateContinuously = true;
            return this;
        }

        public ResizeControl SetUpdateOnDragFinish() {
            updateContinuously = false;
            return this;
        }

        public ResizeControl SetHorizontal(float width) {
            fitVertically = false;
            LayoutElement.minWidth = width;
            gameObject.AddComponent<MouseResizeStarter>().Setup((int) Axis.Horizontal, updateContinuously, ParentSplitResizablePanel, Coordinator.ResizeCursor, Coordinator.hotSpot);
            ResizeDetector.OnChange.AddListener(() => UpdatePosition());
            return this;
        }

        public ResizeControl SetVertical(float height) {
            fitVertically = true;
            LayoutElement.minHeight = height;
            gameObject.AddComponent<MouseResizeStarter>().Setup((int) Axis.Vertical, updateContinuously, ParentSplitResizablePanel, Coordinator.ResizeCursor, Coordinator.hotSpot);
            ResizeDetector.OnChange.AddListener(() => UpdatePosition());
            return this;
        }

        public void UpdatePosition() {
            if (fitVertically.HasValue) {
                if (fitVertically.Value) {
                    RectTransform.localPosition = new Vector2(0, 0.5f * (ParentElement.RectTransform.rect.height));
                } else {
                    RectTransform.localPosition = new Vector2(0.5f * (ParentElement.RectTransform.rect.width), 0);
                }
            }
        }

    }
}