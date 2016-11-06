using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UnityUIConstructor {

    public class TextControl : InterfaceControl {

        protected override void Setup() {

            Text.font = Coordinator.InterfaceFont;
            Text.text = ControlName;
            Text.color = Color.black;
            Text.alignment = TextAnchor.MiddleCenter;

        }

        public TextControl SetText(string text) {
            Text.text = text;
            return this;
        }

        public TextControl FitToText(float verticalPadding = 0, float horizontalPadding = 0) {

            switch (ParentElement.GetLayoutType()) {
            case ElementLayoutType.Horizonal:
                FitToContentVertically();
                break;
            case ElementLayoutType.Vertical:
                FitToContentHorizonally();
                break;
            case ElementLayoutType.NotSet:
                throw new Exception("No layout type set");
            }

            if (horizontalPadding > 0) {
                LayoutElement.minWidth = 2 * horizontalPadding + Text.preferredWidth;
            }

            if (verticalPadding > 0) {
                LayoutElement.minHeight = 2 * verticalPadding + Text.preferredHeight;
            }

            return this;
        }

        public TextControl SetTextColour(Color colour) {
            Text.color = colour;
            return this;
        }

        public TextControl SetTextColour(float grey) {
            Text.color = new Color(grey, grey, grey);
            return this;
        }

    }
}