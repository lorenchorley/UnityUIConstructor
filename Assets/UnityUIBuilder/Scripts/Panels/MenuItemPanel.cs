using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace UnityUIConstructor {

    public class MenuItemPanel : ButtonPanel {

        protected override void Setup() {
            base.Setup();

            FitToText(10, 10);
            UnconstrainHorizonally();
            SetColour(0.9f);
        }

    }

}