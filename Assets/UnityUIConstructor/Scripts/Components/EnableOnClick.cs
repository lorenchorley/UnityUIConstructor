using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace UnityUIConstructor {

    public class EnableOnClick : UIBehaviour, IPointerDownHandler {

        public UIBehaviour ToEnable;

        public void OnPointerDown(PointerEventData eventData) {
            if (ToEnable != null) {
                ToEnable.enabled = true;
                Destroy(this);
            }
        }

    }
}