using System;
using UnityEditor;
using UnityEngine;

namespace VrmHandJoyController
{
    [CustomEditor(typeof(VrmHandJoyController))]
    public sealed class VrmHandJoyControllerEditor : Editor
    {
        private Joycon.Button[] _joyconButtons;
        private SerializedProperty _modelProperty;
        private SerializedProperty _defaultHandPoseProperty;
        private SerializedProperty _leftHandPosesProperty;
        private SerializedProperty _rightHandPosesProperty;
        private SerializedProperty _lerpCoefficientProperty;
        private SerializedProperty _leftHandSourceProperty;
        private SerializedProperty _rightHandSourceProperty;
        private bool _showBaseInspector;
        private bool _foldoutHandPose;
        private bool _foldoutDebug;

        private void OnEnable()
        {
            serializedObject.Update();

            _joyconButtons = Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

            _modelProperty = serializedObject.FindProperty("_model");
            _defaultHandPoseProperty = serializedObject.FindProperty("_defaultHandPose");

            _leftHandPosesProperty = serializedObject.FindProperty("_leftHandPoses");
            _rightHandPosesProperty = serializedObject.FindProperty("_rightHandPoses");

            _lerpCoefficientProperty = serializedObject.FindProperty("_lerpCoefficient");

            _leftHandSourceProperty = serializedObject.FindProperty("_leftHandSource");
            _rightHandSourceProperty = serializedObject.FindProperty("_rightHandSource");

            _leftHandPosesProperty.arraySize = _joyconButtons.Length;
            _rightHandPosesProperty.arraySize = _joyconButtons.Length;

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_modelProperty, new GUIContent("Model"));
            EditorGUILayout.PropertyField(_defaultHandPoseProperty, new GUIContent("Default Hand Pose"));

            _foldoutHandPose = EditorGUILayout.Foldout(_foldoutHandPose, "Hand Pose Setting");
            if (_foldoutHandPose)
            {
                EditorGUI.indentLevel++;

                var i = 0;
                foreach (Joycon.Button button in Enum.GetValues(typeof(Joycon.Button)))
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.Label(button.ToString());

                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.indentLevel++;
                    GUILayout.Label("Left");
                    EditorGUILayout.PropertyField(_leftHandPosesProperty.GetArrayElementAtIndex(i), GUIContent.none);

                    GUILayout.Label("Right");
                    EditorGUILayout.PropertyField(_rightHandPosesProperty.GetArrayElementAtIndex(i), GUIContent.none);

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.EndVertical();
                    i++;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Label("Hand Source");

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Left");
            EditorGUILayout.PropertyField(_leftHandSourceProperty, GUIContent.none);

            GUILayout.Label("Right");
            EditorGUILayout.PropertyField(_rightHandSourceProperty, GUIContent.none);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(_lerpCoefficientProperty, new GUIContent("Lerp Coefficient"));

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("JoyCon Status");
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label($"Left: {((VrmHandJoyController)target).IsConnectedLeftJoyCon}");
            GUILayout.Label($"Right: {((VrmHandJoyController)target).IsConnectedRightJoyCon}");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            _foldoutDebug = EditorGUILayout.Foldout(_foldoutDebug, "Debug");
            if (_foldoutDebug)
            {
                EditorGUI.indentLevel++;
                base.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
