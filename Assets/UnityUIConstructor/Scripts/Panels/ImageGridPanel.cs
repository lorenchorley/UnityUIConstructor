using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UnityUIConstructor {

    public class ImageGridPanel : InterfacePanel {

        protected override void Setup() {

            SetGridLayout(TextAnchor.UpperLeft, GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal);

        }

        public ImageGridPanel SetImageSize(Vector2 size) {
            GridLayoutGroup.cellSize = size;
            return this;
        }

        public ImageGridPanel AddImage(string text, string tooltip, Sprite spite, UnityAction action) {
            AddControl<ImageButtonControl>(text)
                //.SetTooltip(tooltip)
                .SetImage(spite)
                .OnClick(action)
                ;
            return this;
        }

        public ImageGridPanel AddColour(string text, string tooltip, Color colour, UnityAction action) {
            AddControl<ImageButtonControl>(text)
                //.SetTooltip(tooltip)
                .OnClick(action)
                .SetColour(colour)
                ;
            return this;
        }

    }

}