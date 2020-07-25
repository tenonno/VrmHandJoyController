using UnityEditor;
using UnityEngine;

namespace VrmHandJoyController
{
    [CustomEditor(typeof(HandPosePreset))]
    public sealed class HandPosePresetEditor : Editor
    {
        private SerializedProperty _nameProperty;
        private HandPoseDataEditorUtility _defaultHandPoseUtility;

        private void OnEnable()
        {
            serializedObject.Update();

            _nameProperty = serializedObject.FindProperty("_name");
            _defaultHandPoseUtility = new HandPoseDataEditorUtility(serializedObject, "", true);

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_nameProperty, new GUIContent("Name"));

            _defaultHandPoseUtility.DrawGui();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
