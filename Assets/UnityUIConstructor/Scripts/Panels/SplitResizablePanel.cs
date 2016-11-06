using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class SplitResizablePanel : InterfacePanel {

        bool isVertical = false;
        float size;
        bool updateContinuously;

        InterfacePanel LeftOrTopPanel;
        ResizeControl ResizeControl;
        InterfacePanel RightOrBottomPanel;

        protected override void Setup() {

            Coordinator.RunAfterFinishedGeneratingInterface(delegate {
                // Finalise settings
                ResizeControl = SetResizableControl(size, updateContinuously);
                LeftOrTopPanel.transform.SetSiblingIndex(0);
                ResizeControl.transform.SetSiblingIndex(1);
                RightOrBottomPanel.transform.SetSiblingIndex(2);
                SetDivision(1, 2);
            });

        }

        public SplitResizablePanel SetupSplitResizablePanel(bool isVertical = false, float size = 5, bool updateContinuously = false) {
            this.isVertical = isVertical;
            this.size = size;
            this.updateContinuously = updateContinuously;
            return this;
        }

        public void SetDivision(float positionWithinInterval, float interval) {
            float percentage = Mathf.Clamp01(positionWithinInterval / interval);
            if (isVertical) {
                LeftOrTopPanel.LayoutElement.flexibleHeight = percentage * 100000;
                RightOrBottomPanel.LayoutElement.flexibleHeight = (1 - percentage) * 100000;
            } else {
                LeftOrTopPanel.LayoutElement.flexibleWidth = percentage * 100000;
                RightOrBottomPanel.LayoutElement.flexibleWidth = (1 - percentage) * 100000;
            }
        }

        public P GetLeftOrTopPanel<P>(string name, out P panel) where P : InterfacePanel {
            panel = GetLeftOrTopPanel<P>(name);
            return panel;
        }

        public P GetLeftOrTopPanel<P>(string name) where P : InterfacePanel {
            P panel;
            if (LeftOrTopPanel == null) {
                AddPanel<P>((name != null) ? name : (isVertical ? "Top" : "Left"), out panel)
                    .SetToCoverParent();
                LeftOrTopPanel = panel;
            } else {
                panel = (P) LeftOrTopPanel;
            }
            return panel;
        }

        public P GetRightOrBottomPanel<P>(string name, out P panel) where P : InterfacePanel {
            panel = GetRightOrBottomPanel<P>(name);
            return panel;
        }

        public P GetRightOrBottomPanel<P>(string name) where P : InterfacePanel {
            P panel;
            if (RightOrBottomPanel == null) {
                AddPanel<P>((name != null) ? name : (isVertical ? "Right" : "Bottom"), out panel)
                    .SetToCoverParent();
                RightOrBottomPanel = panel;
            } else {
                panel = (P) RightOrBottomPanel;
            }
            return panel;
        }

        private ResizeControl SetResizableControl(float size, bool updateContinuously) {
            ResizeControl control;
            AddControl<ResizeControl>("Resizer", out control);

            if (isVertical)
                control.SetVertical(size);
            else
                control.SetHorizontal(size);

            if (updateContinuously)
                control.SetUpdateContinuous();
            else
                control.SetUpdateOnDragFinish();

            Coordinator.RunAfterInterfaceConfigured(() => control.UpdatePosition());
            return control;
        }

    }
}