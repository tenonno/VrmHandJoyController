using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vox.Hands;

namespace VrmHandJoyController
{
    [Serializable]
    public sealed class HandPoseDataEditorUtility
    {
        private FingerProperties[] _fingers;
        private bool[] _foldouts;

        private SerializedProperty _handPoseEnabledArrayProperty;
        private SerializedProperty _enabledProperty;

        private bool _joyConMode;

        private float AllSpread
        {
            get => _fingers.Sum(f => f.Spread) / _fingers.Length;
            set
            {
                foreach (var finger in _fingers)
                {
                    finger.Spread = value;
                }
            }
        }

        private float AllFingersMuscle
        {
            get => _fingers.Sum(f => f.MuscleAll) / _fingers.Length;
            set
            {
                foreach (var finger in _fingers)
                {
                    finger.MuscleAll = value;
                }
            }
        }

        public HandPoseDataEditorUtility(SerializedObject serializedObject, string rootPropertyPath, bool joyConMode)
        {
            if (joyConMode)
            {
                _enabledProperty = serializedObject.FindProperty(rootPropertyPath + "_enabled");
                _handPoseEnabledArrayProperty =
                    serializedObject.FindProperty(rootPropertyPath + "_handPoseEnabledArray");
                rootPropertyPath += "_handPose.";
                _handPoseEnabledArrayProperty.arraySize = 5;
            }

            _fingers = new[]
            {
                new FingerProperties(serializedObject, rootPropertyPath, "Thumb"),
                new FingerProperties(serializedObject, rootPropertyPath, "Index"),
                new FingerProperties(serializedObject, rootPropertyPath, "Middle"),
                new FingerProperties(serializedObject, rootPropertyPath, "Ring"),
                new FingerProperties(serializedObject, rootPropertyPath, "Little")
            };

            _foldouts = new bool[_fingers.Length];
            _joyConMode = joyConMode;
        }

        public void DrawGui()
        {
            if (_joyConMode)
            {
                EditorGUILayout.PropertyField(_enabledProperty, new GUIContent("Enabled"));
            }

            var allSpreadValue = AllSpread;
            var newAllSpreadValue = EditorGUILayout.Slider("Spread", allSpreadValue, -1f, 1f);
            if (allSpreadValue != newAllSpreadValue)
            {
                AllSpread = newAllSpreadValue;
            }

            var allFingersMuscleValue = AllFingersMuscle;
            var newAllFingersMuscleValue = EditorGUILayout.Slider("Muscles", allFingersMuscleValue, -1f, 1f);
            if (allFingersMuscleValue != newAllFingersMuscleValue)
            {
                AllFingersMuscle = newAllFingersMuscleValue;
            }

            for (var i = 0; i < _fingers.Length; ++i)
            {
                var finger = _fingers[i];

                var suffix = "";

                if (_joyConMode && _handPoseEnabledArrayProperty.GetArrayElementAtIndex(i).boolValue)
                {
                    suffix = " ✔︎";
                }

                _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], $"{finger.FingerName} Finger" + suffix);

                if (!_foldouts[i]) continue;

                EditorGUI.indentLevel++;

                if (_joyConMode)
                {
                    EditorGUILayout.PropertyField(_handPoseEnabledArrayProperty.GetArrayElementAtIndex(i),
                        new GUIContent("Enabled"));
                }

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    var allMuscleValue = finger.MuscleAll;
                    var newAllMuscleValue = EditorGUILayout.Slider("All", allMuscleValue, -1f, 1f);
                    if (allMuscleValue != newAllMuscleValue)
                    {
                        finger.MuscleAll = newAllMuscleValue;
                    }

                    EditorGUILayout.Slider(finger.PropSpread, -1f, 1f, "Spread");
                    EditorGUILayout.Slider(finger.PropMuscle1, -1f, 1f, "1st");
                    EditorGUILayout.Slider(finger.PropMuscle2, -1f, 1f, "2nd");
                    EditorGUILayout.Slider(finger.PropMuscle3, -1f, 1f, "3rd");
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
