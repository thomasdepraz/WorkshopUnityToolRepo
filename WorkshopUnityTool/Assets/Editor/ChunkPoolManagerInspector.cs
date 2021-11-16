using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ChunkPoolManager))]
public class ChunkPoolManagerInspector : Editor
{
    ChunkPoolManager script;
    SerializedProperty self;
    SerializedProperty chunkPrefab;
    SerializedProperty foldoutBool, poolSize;
    SerializedProperty chunks;


    void OnEnable()
    {
        self = serializedObject.FindProperty(nameof(script.self));
        chunkPrefab = serializedObject.FindProperty(nameof(script.chunkPrefab));
        foldoutBool = serializedObject.FindProperty(nameof(script.foldoutBool));
        poolSize = serializedObject.FindProperty(nameof(script.poolSize));
        chunks = serializedObject.FindProperty(nameof(script.chunks));

        Undo.undoRedoPerformed += RebuilPool;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= RebuilPool;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Pool", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(poolSize);
        bool changed = EditorGUI.EndChangeCheck();

        if (GUILayout.Button("Rebuild"))
        {
            RebuilPool();
        }
        EditorGUILayout.EndHorizontal();
        if(changed)
        {
            if (poolSize.intValue < 0) poolSize.intValue = 0;
            //RebuilPool();
        }

        foldoutBool.boolValue = EditorGUILayout.Foldout(foldoutBool.boolValue, "References", true);
        if(foldoutBool.boolValue)
        {
            EditorGUI.indentLevel ++;

            EditorGUILayout.PropertyField(self);
            EditorGUILayout.PropertyField(chunkPrefab);

            EditorGUI.indentLevel --;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RebuilPool()
    {
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        serializedObject.Update();


        //détruire les objets du pool
        while (chunks.arraySize > 0)
        {
            SerializedProperty cur = chunks.GetArrayElementAtIndex(0);
            Object cb = cur.objectReferenceValue;
            if (cb == null) chunks.DeleteArrayElementAtIndex(0);
            else
            {
                DestroyImmediate((cb as ChunkBehaviour).gameObject);
                chunks.DeleteArrayElementAtIndex(0);
            }

        }

        //recréér le bon nombre d'objets
        //les remettre enfants de cet objet, les positionner correctement
        //les référencer dans l'array
        for (int i = 0; i < poolSize.intValue; i++)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(chunkPrefab.objectReferenceValue as GameObject, self.objectReferenceValue as Transform) as GameObject;
            Transform tr;
            tr = go.transform;
            tr.SetParent(self.objectReferenceValue as Transform);

            EditorUtility.SetDirty(tr);
            chunks.InsertArrayElementAtIndex(chunks.arraySize);
            chunks.GetArrayElementAtIndex(chunks.arraySize - 1).objectReferenceValue = go.GetComponent<ChunkBehaviour>();
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}
