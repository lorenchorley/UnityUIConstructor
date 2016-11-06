using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class AnglerPanel : InterfacePanel {

        Angler Angler;

        protected override void Setup() {

            Angler = GetComponent<Angler>();

        }

        public AnglerPanel SetEnabled(bool enabled) {
            Angler.enabled = enabled;
            if (enabled) { 
                Color c = Angler.BackgroundGraphic.color;
                c.a = 1;
                Angler.BackgroundGraphic.color = c;
            } else {
                Color c = Angler.BackgroundGraphic.color;
                c.a = 0.25f;
                Angler.BackgroundGraphic.color = c;
            }
            return this;
        }

        public AnglerPanel SetBackground(Sprite sprite) {
            Angler.BackgroundGraphic.sprite = sprite;
            return this;
        }

        public AnglerPanel SetHandle(Sprite sprite) {
            Angler.HandleGraphic.sprite = sprite;
            return this;
        }

        public AnglerPanel SetValue(float angle) {
            Angler.value = angle;
            return this;
        }

        public AnglerPanel AllowDisableButton() {

            return this;
        }

        public AnglerPanel OnChange(UnityAction<float> action) {
            Angler.onValueChanged.AddListener(action);
            return this;
        }

    }

}