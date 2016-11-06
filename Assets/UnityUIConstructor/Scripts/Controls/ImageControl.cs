using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UnityUIConstructor {

    public class ImageControl : InterfaceControl {

        protected override void Setup() {
            RequestImage();
        }

        public ImageControl SetImage(Sprite sprite) {
            Image.sprite = sprite;

            return this;
        }

    }
}