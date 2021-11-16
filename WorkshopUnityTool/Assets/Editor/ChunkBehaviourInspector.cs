using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChunkBehaviour))]
public class ChunkBehaviourInspector : Editor
{
    ChunkBehaviour script;
    public SerializedProperty scrollSpeed, self, profile;

    private void OnEnable()
    {
        scrollSpeed = serializedObject.FindProperty(nameof(script.speed));
        self = serializedObject.FindProperty(nameof(script.self));
        profile = serializedObject.FindProperty(nameof(script.profile));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(scrollSpeed);
        EditorGUILayout.PropertyField(self);
        EditorGUILayout.PropertyField(profile);


        if (GUILayout.Button("Load Profile"))
        {

        }

        serializedObject.ApplyModifiedProperties();
    }
}
