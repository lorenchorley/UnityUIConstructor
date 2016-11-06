using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityUIConstructor {

    public class MouseDragHandler : MonoBehaviour, IPointerUpHandler {

        float startingMousePositionOnAxis;
        float startingOffsetValueOnAxis;
        float startingValueOffAxis;

        Vector3 previousPosition;

        int axis;
        bool updateContinuously;
        Action<float> update;

        public void Setup(int axis, bool updateContinuously, Action<float> update) {
            this.axis = axis;
            this.updateContinuously = updateContinuously;
            this.update = update;
        }

        void Start() {
            startingMousePositionOnAxis = Input.mousePosition[axis];
            if (axis == 0) {
                startingOffsetValueOnAxis = transform.position.x - Input.mousePosition.x;
                startingValueOffAxis = transform.position.y;
            } else if (axis == 1) {
                startingOffsetValueOnAxis = transform.position.y - Input.mousePosition.y;
                startingValueOffAxis = transform.position.x;
            }

            previousPosition = transform.position;
        }

        void Update() {
            if (axis == 0) {
                transform.position = new Vector2(Input.mousePosition.x + startingOffsetValueOnAxis, startingValueOffAxis);
            } else if (axis == 1) {
                transform.position = new Vector2(startingValueOffAxis, Input.mousePosition.y + startingOffsetValueOnAxis);
            }

            if (updateContinuously && previousPosition != transform.position)
                update.Invoke(startingMousePositionOnAxis - Input.mousePosition[axis]);

            previousPosition = transform.position;
        }

        public void OnPointerUp(PointerEventData eventData) {
            update.Invoke(startingMousePositionOnAxis - Input.mousePosition[axis]);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Destroy(this);
        }

    }

}