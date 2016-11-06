using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class MenuItemPanel : ButtonPanel {

        protected override void Setup() {
            base.Setup();

            SetMinWidth(150);
            FitToText(5, 30);
            UnconstrainHorizonally();
            SetColour(0.9f);
        }

    }

}