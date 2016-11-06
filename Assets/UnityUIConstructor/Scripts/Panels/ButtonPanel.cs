using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class ButtonPanel : InterfacePanel {

        public TextControl TextControl;

        protected override void Setup() {

            LayoutType = ParentElement.GetLayoutType();

            Image.sprite = Coordinator.ButtonSprite;
            Image.type = Coordinator.ImageType;
            Button.image = Image;

            ColorBlock c = Button.colors;
            c.fadeDuration = 0.05f;
            Button.colors = c;

            HorizontalLayoutGroup.childForceExpandHeight = false;
            HorizontalLayoutGroup.childForceExpandWidth = false;

            AddControl<TextControl>("PanelName", out TextControl);
            TextControl.name = "Text";
            TextControl.Text.text = PanelName;

        }

        public ButtonPanel SetEnabled(bool enable) {
            Button.enabled = enable;
            SetTextColour(enable ? new Color(0.1f, 0.1f, 0.1f) : Color.grey); // TODO Set these values using methods
            return this;
        }

        public ButtonPanel SetText(string text) {
            TextControl.SetText(text);
            return this;
        }

        public ButtonPanel OnClick(UnityAction action) {
            Button.onClick.AddListener(action);
            return this;
        }

        public ButtonPanel FitToText(float verticalPadding = 0, float horizontalPadding = 0) {
            TextControl.FitToText(verticalPadding, horizontalPadding);
            FitToContentHorizonally();
            FitToContentVertically();
            return this;
        }

        public TextControl GetText() {
            return TextControl;
        }

        public ButtonPanel SetTextColour(Color colour) {
            TextControl.SetTextColour(colour);
            return this;
        }

        public ButtonPanel SetTextColour(float grey) {
            TextControl.SetTextColour(grey);
            return this;
        }

        public ButtonPanel SetButtonColours(Color Normal, Color Pressed, Color Disabled, float colorMultiplier = 1, float fadeDuration = 0.1f) {
            Button.colors = new ColorBlock() {
                normalColor = Normal,
                highlightedColor = Normal,
                pressedColor = Pressed,
                disabledColor = Disabled,
                colorMultiplier = colorMultiplier,
                fadeDuration = fadeDuration
            };
            return this;
        }

    }

}