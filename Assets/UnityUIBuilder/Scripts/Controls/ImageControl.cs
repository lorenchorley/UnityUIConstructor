using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UnityUIConstructor {

    public class ImageControl : InterfaceControl {

        protected override void Setup() {
            RequestImage();
            LayoutElement.preferredHeight = 50;
            LayoutElement.preferredWidth = 50;
        }

        public ImageControl SetImage(Sprite sprite) {
            Image.sprite = sprite;

            return this;
        }

    }
}