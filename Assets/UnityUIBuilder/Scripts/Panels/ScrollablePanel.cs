using UnityEngine;
using System;
using UnityEngine.UI;

namespace UnityUIConstructor {

    public class ScrollablePanel : InterfacePanel {

        private InterfacePanel ScrollingPanel;
        private ScrollRect ScrollRect;

        protected override void Setup() {

            gameObject.name += " (Scrolling Frame)";

            ScrollRect = gameObject.AddComponent<ScrollRect>();
            ScrollRect.horizontal = false;
            ScrollRect.vertical = false;
            ScrollRect.scrollSensitivity = 10;

            Coordinator.OnInterfaceConfigured.AddListener(delegate {
                ScrollRect.horizontalNormalizedPosition = 1;
                ScrollRect.verticalNormalizedPosition = 1;
                if (ScrollRect.horizontalScrollbar != null)
                    ScrollRect.horizontalScrollbar.transform.SetAsLastSibling();
                if (ScrollRect.verticalScrollbar != null)
                    ScrollRect.verticalScrollbar.transform.SetAsLastSibling();
            });

        }

        public P GetScrollingPanel<P>() where P : InterfacePanel {
            if (ScrollingPanel == null) {
                P panel = null;
                AddPanel<P>("Scrolling Panel", out panel);
                ScrollingPanel = panel;
                panel.SetToCoverParent();
                ScrollRect.content = panel.RectTransform;
                return panel;

            } else
                return (P) ScrollingPanel;
        }

        public ScrollablePanel SetHorizontalScrolling(bool enable, bool addScrollbar = false, bool addMask = false) {
            ScrollRect.horizontal = enable;
            if (addScrollbar) {
                if (Coordinator.HorizontalScrollbar == null)
                    throw new Exception("No horizontal scrollbar set");
                GameObject scrollbar = GameObject.Instantiate<GameObject>(Coordinator.HorizontalScrollbar.gameObject);
                RectTransform scrollbarRect = scrollbar.transform as RectTransform;
                scrollbarRect.SetParent(transform);
                scrollbarRect.anchorMin = new Vector2(1, 0);
                scrollbarRect.anchorMax = new Vector2(1, 1);
                scrollbarRect.pivot = new Vector2(1, 0.5f);
                scrollbarRect.anchoredPosition = Vector2.zero;
                scrollbarRect.sizeDelta = new Vector2(0, scrollbarRect.rect.height);
                ScrollRect.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();
                ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
                ScrollRect.viewport = RectTransform;
            }
            if (addMask) {
                gameObject.AddComponent<Mask>();
            }
            return this;
        }

        public ScrollablePanel SetVerticalScrolling(bool enable, bool addScrollbar = false, bool addMask = false) {
            ScrollRect.vertical = enable;
            if (addScrollbar) {
                if (Coordinator.VertcialScrollbar == null)
                    throw new Exception("No vertical scrollbar set");
                GameObject scrollbar = GameObject.Instantiate<GameObject>(Coordinator.VertcialScrollbar.gameObject);
                RectTransform scrollbarRect = scrollbar.transform as RectTransform;
                scrollbarRect.SetParent(transform);
                scrollbarRect.anchorMin = new Vector2(1, 0);
                scrollbarRect.anchorMax = new Vector2(1, 1);
                scrollbarRect.pivot = new Vector2(1, 0.5f);
                scrollbarRect.anchoredPosition = Vector2.zero;
                scrollbarRect.sizeDelta = new Vector2(scrollbarRect.rect.width, 0);
                ScrollRect.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();
                ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
                ScrollRect.viewport = RectTransform;
            }
            if (addMask) {
                gameObject.AddComponent<Mask>();
            }
            return this;
        }

        public ScrollablePanel SetScrollingType(ScrollRect.MovementType type) {
            ScrollRect.movementType = type;
            return this;
        }

    }
}