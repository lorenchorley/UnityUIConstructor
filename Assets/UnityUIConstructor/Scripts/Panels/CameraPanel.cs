using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace UnityUIConstructor {

    public class CameraPanel : InterfacePanel {

        public Camera Camera;
        bool resizeCameraToFit;

        protected override void Setup() {
            Coordinator.RunAfterFinishedGeneratingInterface(() => {
                if (Camera != null)
                    Camera.enabled = false;
            });
        }

        public CameraPanel SetCamera(Camera Camera, bool resizeCameraToFit = false) {
            this.Camera = Camera;
            this.resizeCameraToFit = resizeCameraToFit;
            Coordinator.RunAfterInterfaceConfigured(() => {
                ResizeDetector.OnChange.AddListener(() => FitCameraToPanel());
                FitCameraToPanel();
            });
            ParentSlide.OnShow.AddListener(() => {
                if (Camera != null)
                    Camera.enabled = true;
                FitCameraToPanel();
            });
            ParentSlide.OnHide.AddListener(() => {
                if (Camera != null)
                    Camera.enabled = false;
            });
            return this;
        }

        public CameraPanel FitCameraToPanel() {
            if (Camera == null || !Camera.enabled) {
                return this;
            } else if (RectTransform.rect.height <= 0 || RectTransform.rect.width <= 0) { 
                Assert.IsTrue(Camera.enabled);
                Camera.enabled = false;
                return this;
            } else {
                Assert.IsTrue(Camera.enabled);
                Camera.enabled = true;
            }

            float width, height, x, y;

            // Set x and y to panel coordinates
            Vector2 canvasOriginOffset = -Vector2.Scale((Coordinator.ParentCanvas.transform as RectTransform).pivot, Coordinator.ParentCanvas.pixelRect.size);
            Vector2 panelOriginOffet = -Vector2.Scale(RectTransform.pivot, RectTransform.rect.size);
            Vector2 newPanelPos = (Vector2) Coordinator.ParentCanvas.transform.position + (Vector2) RectTransform.position + canvasOriginOffset + panelOriginOffet;
            x = newPanelPos.x;
            y = newPanelPos.y;

            if (resizeCameraToFit) {

                // Fit camera to exactly the camera rect
                width = RectTransform.rect.width;
                height = RectTransform.rect.height;

            } else {

                float panelRatio = RectTransform.rect.width / RectTransform.rect.height;
                float cameraRatio;
                if (Camera.pixelHeight <= 0)
                    cameraRatio = (float) Camera.pixelWidth / (float) Camera.pixelHeight;
                else
                    cameraRatio = (float) Camera.pixelWidth / 0.001f;

                // Fit camera within the panel rect
                if (cameraRatio >= panelRatio) {
                    width = RectTransform.rect.width;
                    height = RectTransform.rect.width / cameraRatio;
                    y -= 0.5f * (height - RectTransform.rect.size.y); // Offset of camear within panel
                } else {
                    height = RectTransform.rect.height;
                    width = RectTransform.rect.height * cameraRatio;
                    x -= 0.5f * (width - RectTransform.rect.size.x); // Offset of camear within panel
                }

            }

            Camera.pixelRect = new Rect(x, y, width, height);

            return this;
        }

    }

}