using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityUIConstructor {

    // Note that this class requires a GUIElement or Collider attached to the same object to work
    public class MouseOverDetector : MonoBehaviour {

        public UnityEvent MouseEnter;
        public UnityEvent MouseExit;

        public void Initialise() {
            MouseEnter = new UnityEvent();
            MouseExit = new UnityEvent();
        }

        void OnMouseEnter() {
            Debug.Log("enter"); // TODO
            MouseEnter.Invoke();
        }

        void OnMouseExit() {
            Debug.Log("exit");
            MouseExit.Invoke();
        }

    }

}