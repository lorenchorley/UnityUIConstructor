using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityUIConstructor {

    // Note that this class requires a GUIElement or Collider attached to the same object to work
    public class MouseOverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public UnityEvent MouseEnter;
        public UnityEvent MouseExit;

        public void Initialise() {
            MouseEnter = new UnityEvent();
            MouseExit = new UnityEvent();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            MouseEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData) {
            MouseExit.Invoke();
        }

    }

}