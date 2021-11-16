using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChunkProfile))]
public class ChunkProfileInspector : Editor
{
    public SerializedProperty width, height;
    private void OnEnable()
    {
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Open window"))
            OpenWindow();


        serializedObject.ApplyModifiedProperties();
    }

    void OpenWindow()
    {
        ChunkEditorWindow editWindow;
        if(!EditorWindow.HasOpenInstances<ChunkEditorWindow>())
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            editWindow = EditorWindow.CreateWindow<ChunkEditorWindow>("Chunk Editor Window", new Type[] { inspectorType });
        }
        editWindow = EditorWindow.GetWindow(typeof(ChunkEditorWindow))as ChunkEditorWindow;

        editWindow.InitWindow(target as ChunkProfile);
        editWindow.Show();
    }

}
