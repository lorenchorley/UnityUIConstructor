﻿#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUIConstructor {
    [CustomEditor(typeof(Slider), true)]
    [CanEditMultipleObjects]
    public class IntervalSliderEditor : SelectableEditor {
        SerializedProperty m_Direction;
        SerializedProperty m_FillRect;
        SerializedProperty m_LowerHandleRect;
        SerializedProperty m_UpperHandleRect;
        SerializedProperty m_MinValue;
        SerializedProperty m_MaxValue;
        SerializedProperty m_WholeNumbers;
        SerializedProperty m_LowerValue;
        SerializedProperty m_UpperValue;
        SerializedProperty m_OnValueChanged;

        protected override void OnEnable() {
            base.OnEnable();
            m_FillRect = serializedObject.FindProperty("m_FillRect");
            m_LowerHandleRect = serializedObject.FindProperty("m_LowerHandleRect");
            m_UpperHandleRect = serializedObject.FindProperty("m_UpperHandleRect");
            m_Direction = serializedObject.FindProperty("m_Direction");
            m_MinValue = serializedObject.FindProperty("m_MinValue");
            m_MaxValue = serializedObject.FindProperty("m_MaxValue");
            m_WholeNumbers = serializedObject.FindProperty("m_WholeNumbers");
            m_LowerValue = serializedObject.FindProperty("m_LowerValue");
            m_UpperValue = serializedObject.FindProperty("m_UpperValue");
            m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_FillRect);
            EditorGUILayout.PropertyField(m_LowerHandleRect);
            EditorGUILayout.PropertyField(m_UpperHandleRect);

            if (m_FillRect.objectReferenceValue != null || m_LowerHandleRect.objectReferenceValue != null || m_UpperHandleRect.objectReferenceValue != null) {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_Direction);
                if (EditorGUI.EndChangeCheck()) {
                    Slider.Direction direction = (Slider.Direction) m_Direction.enumValueIndex;
                    foreach (var obj in serializedObject.targetObjects) {
                        Slider slider = obj as Slider;
                        slider.SetDirection(direction, true);
                    }
                }

                EditorGUILayout.PropertyField(m_MinValue);
                EditorGUILayout.PropertyField(m_MaxValue);
                EditorGUILayout.PropertyField(m_WholeNumbers);
                EditorGUILayout.Slider(m_LowerValue, m_MinValue.floatValue, m_MaxValue.floatValue);
                EditorGUILayout.Slider(m_UpperValue, m_MinValue.floatValue, m_MaxValue.floatValue);

                bool warning = false;
                foreach (var obj in serializedObject.targetObjects) {
                    Slider slider = obj as Slider;
                    Slider.Direction dir = slider.direction;
                    if (dir == Slider.Direction.LeftToRight || dir == Slider.Direction.RightToLeft)
                        warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnLeft() != null || slider.FindSelectableOnRight() != null));
                    else
                        warning = (slider.navigation.mode != Navigation.Mode.Automatic && (slider.FindSelectableOnDown() != null || slider.FindSelectableOnUp() != null));
                }

                if (warning)
                    EditorGUILayout.HelpBox("The selected slider direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);

                // Draw the event notification options
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_OnValueChanged);
            } else {
                EditorGUILayout.HelpBox("Specify a RectTransform for the slider fill or the slider handle or both. Each must have a parent RectTransform that it can slide within.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif