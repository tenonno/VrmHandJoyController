using System;
using UnityEditor;
using UnityEngine;

namespace VrmHandJoyController
{
    [CustomEditor(typeof(VrmHandJoyController))]
    public sealed class VrmHandJoyControllerEditor : Editor
    {
        private bool _foldoutDefaultHandPose;

        private HandPoseDataEditorUtility _defaultHandPoseUtility;

        private Joycon.Button[] _joyconButtons;

        private SerializedProperty _buttonsProperty;
        private SerializedProperty _lerpCoefficientProperty;
        private SerializedProperty _keepRootBonePositionProperty;

        private HandPoseDataEditorUtility[] _defaultHandPoseUtility2;

        private bool[] _openedButton;

        private SerializedProperty[] _buttonEnabledProperties;

        private bool _showBaseInspector;

        private bool _foldoutHandPose;
        private bool _foldoutDebug;

        private void OnEnable()
        {
            serializedObject.Update();

            _joyconButtons = Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

            _defaultHandPoseUtility = new HandPoseDataEditorUtility(serializedObject, "_defaultHandPose.", false);

            _buttonsProperty = serializedObject.FindProperty("_buttons");
            _lerpCoefficientProperty = serializedObject.FindProperty("_lerpCoefficient");
            _keepRootBonePositionProperty = serializedObject.FindProperty("_keepRootBonePosition");

            _defaultHandPoseUtility2 = new HandPoseDataEditorUtility[_joyconButtons.Length];
            _openedButton = new bool[_joyconButtons.Length];

            _buttonsProperty.arraySize = _joyconButtons.Length;
            for (var i = 0; i < _joyconButtons.Length; i++)
            {
                _buttonsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_button").enumValueIndex = i;
                _defaultHandPoseUtility2[i] =
                    new HandPoseDataEditorUtility(serializedObject, $"_buttons.Array.data[{i}].", true);
            }

            _buttonEnabledProperties = new SerializedProperty[_buttonsProperty.arraySize];
            for (int i = 0; i < _buttonsProperty.arraySize; i++)
            {
                _buttonEnabledProperties[i] =
                    _buttonsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("_enabled");
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_model"), new GUIContent("Model"));

            _foldoutDefaultHandPose = EditorGUILayout.Foldout(_foldoutDefaultHandPose, "Default Hand Pose");

            if (_foldoutDefaultHandPose)
            {
                EditorGUI.indentLevel++;
                _defaultHandPoseUtility.DrawGui();
                EditorGUI.indentLevel--;
            }

            _foldoutHandPose = EditorGUILayout.Foldout(_foldoutHandPose, "Hand Pose Setting");
            if (_foldoutHandPose)
            {
                EditorGUI.indentLevel++;

                var i = 0;
                foreach (Joycon.Button button in Enum.GetValues(typeof(Joycon.Button)))
                {
                    var enabled = _buttonEnabledProperties[i].boolValue;
                    var suffix = enabled ? " ✔︎" : "";

                    _openedButton[i] = EditorGUILayout.Foldout(_openedButton[i], button + suffix);

                    if (_openedButton[i])
                    {
                        EditorGUI.indentLevel++;
                        _defaultHandPoseUtility2[i].DrawGui();
                        EditorGUI.indentLevel--;
                    }

                    i++;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(_lerpCoefficientProperty, new GUIContent("Lerp Coefficient"));
            EditorGUILayout.PropertyField(_keepRootBonePositionProperty, new GUIContent("Keep Root Bone Position"));


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
