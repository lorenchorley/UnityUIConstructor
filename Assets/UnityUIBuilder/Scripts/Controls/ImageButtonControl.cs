﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class ImageButtonControl : InterfaceControl {

        protected override void Setup() {
            RequestImage();

            HorizontalLayoutGroup.childForceExpandHeight = false;
            HorizontalLayoutGroup.childForceExpandWidth = false;

            ColorBlock c = Button.colors;
            c.fadeDuration = 0.05f;
            Button.colors = c;

        }

        public ImageButtonControl SetSize(Vector2 size) {
            LayoutElement.preferredWidth = size.x;
            LayoutElement.preferredHeight = size.y;
            return this;
        }

        public ImageButtonControl SetImage(Sprite sprite) {
            Image.sprite = sprite;
            return this;
        }

        public ImageButtonControl OnClick(UnityAction action) {
            Button.onClick.AddListener(action);
            return this;
        }

    }
}