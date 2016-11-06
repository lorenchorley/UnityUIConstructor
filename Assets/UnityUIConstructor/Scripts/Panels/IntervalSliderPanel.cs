using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UnityUIConstructor {

    public class IntervalSliderPanel : InterfacePanel {

        IntervalSlider IntervalSlider;

        protected override void Setup() {

            IntervalSlider = GetComponent<IntervalSlider>();
            RectTransform RectTransform = IntervalSlider.transform as RectTransform;

            // Set to cover parent
            RectTransform.anchorMin = v00;
            RectTransform.anchorMax = v11;
            RectTransform.anchoredPosition = v00;
            RectTransform.sizeDelta = v00;

        }

        public IntervalSliderPanel SetValues(float min, float max, float lower, float upper) {
            IntervalSlider.minValue = min;
            IntervalSlider.maxValue = max;
            IntervalSlider.lowerValue = lower;
            IntervalSlider.upperValue = upper;
            return this;
        }

        public IntervalSliderPanel OnChange(UnityAction<float, float> action) {
            IntervalSlider.onValueChanged.AddListener(action);
            return this;
        }

    }

}