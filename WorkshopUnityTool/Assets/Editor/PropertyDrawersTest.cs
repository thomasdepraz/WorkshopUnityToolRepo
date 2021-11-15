using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MyRuntimeScript))]
public class PropertyDrawersTest : Editor
{
    SerializedProperty structProperty;
    private void OnEnable()
    {
        structProperty = serializedObject.FindProperty("someStruct");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(structProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
