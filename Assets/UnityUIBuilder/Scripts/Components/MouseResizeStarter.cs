using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityUIConstructor {

    [RequireComponent(typeof(RectTransform))]
    public class MouseResizeStarter : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

        int axis;
        bool updateContinuously;
        SplitResizablePanel SplitResizablePanel;
        Texture2D cursor;
        Vector2 hotspot;

        Vector3[] corners = new Vector3[4];

        public void Setup(int axis, bool updateContinuously, SplitResizablePanel SplitResizablePanel, Texture2D cursor, Vector2 hotspot) {
            this.axis = axis;
            this.updateContinuously = updateContinuously;
            this.SplitResizablePanel = SplitResizablePanel;
            this.cursor = cursor;
            this.hotspot = hotspot;
        }

        public void OnPointerDown(PointerEventData eventData) {
            gameObject.AddComponent<MouseDragHandler>().Setup(axis, updateContinuously, UpdateResize);
        }

        public void UpdateResize(float difference) {
            float regionlength = SplitResizablePanel.RectTransform.rect.size[axis];
            SplitResizablePanel.RectTransform.GetWorldCorners(corners);
            Vector3 mousePositionWithinResiablePanel = Input.mousePosition - corners[0];
            SplitResizablePanel.SetDivision(mousePositionWithinResiablePanel[axis], regionlength);
        }

        public void OnPointerEnter(PointerEventData data) {
            Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
        }
        public void OnPointerExit(PointerEventData data) {
            if (GetComponent<MouseDragHandler>() == null)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}