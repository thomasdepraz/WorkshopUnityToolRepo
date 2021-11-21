using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerInspector : Editor
{
    LevelManager script;
    ObjectPool poolScript;
    SerializedProperty chunkList;
    SerializedProperty enemyPool;
    SerializedProperty obstaclePool;
    SerializedProperty playerStartRef;
    SerializedProperty levelFinishRef;

    int enemyQuantity = 0;
    int obstacleQuantity = 0;

    GUIStyle titleStyle;
    private void OnEnable()
    {
        chunkList = serializedObject.FindProperty(nameof(script.chunkPool));
        enemyPool = serializedObject.FindProperty(nameof(script.enemyPool));
        obstaclePool = serializedObject.FindProperty(nameof(script.obstaclePool));

        playerStartRef = serializedObject.FindProperty(nameof(script.playerStart));
        levelFinishRef = serializedObject.FindProperty(nameof(script.levelGoal));
    }

    public override void OnInspectorGUI()
    {
        //Style Creation
        titleStyle = new GUIStyle(EditorStyles.boldLabel);
        if (titleStyle.fontSize != 20)
        {
            titleStyle.fontSize = 20;
        }


        serializedObject.Update();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Levels", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(chunkList);
        if(GUILayout.Button("Add new Level"))
        {
            CreateLevelProfile();        
        }

        GUILayout.Space(30);
        EditorGUILayout.LabelField("Object Pools", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(enemyPool);

        #region EnemyPool
        if (enemyPool.objectReferenceValue != null)
        {
            SerializedObject so = new SerializedObject(enemyPool.objectReferenceValue);
            so.Update();

            //Afficher objet reference
            EditorGUILayout.PropertyField(so.FindProperty(nameof(poolScript.reference)));

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            //Afficher longueur de la pool 
            SerializedProperty pool = so.FindProperty(nameof(poolScript.pool));
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField(pool.arraySize, GUILayout.MaxWidth(50));
                EditorGUI.EndDisabledGroup();
            
                //Afficher petit bouton +/ moins
                if(GUILayout.Button("+"))
                {
                    RebuilPool(
                        pool, 
                        pool.arraySize + enemyQuantity, 
                        so.FindProperty(nameof(poolScript.reference)).objectReferenceValue as GameObject, 
                        so.FindProperty(nameof(poolScript.self)).objectReferenceValue as Transform);
                }
                if(GUILayout.Button("-"))
                {
                    RebuilPool(
                        pool, 
                        pool.arraySize - enemyQuantity, 
                        so.FindProperty(nameof(poolScript.reference)).objectReferenceValue as GameObject, 
                        so.FindProperty(nameof(poolScript.self)).objectReferenceValue as Transform);
                }

            enemyQuantity = EditorGUILayout.IntField("Quantity", enemyQuantity);
            if (enemyQuantity < 0)
                enemyQuantity = 0;
            EditorGUILayout.EndHorizontal();


            //Afficher barre d'utilisation de la pool
            int arraySize = so.FindProperty(nameof(poolScript.pool)).arraySize;
            int used = so.FindProperty(nameof(poolScript.usedObjectsCount)).intValue;
            float result = (float)used / (float)arraySize;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), result, "Pool usage : " + (result*100).ToString() + " %");
            so.ApplyModifiedProperties(); 
        }
        #endregion

        GUILayout.Space(15);

        EditorGUILayout.PropertyField(obstaclePool);

        #region obstaclePool
        if (obstaclePool.objectReferenceValue != null)
        {
            SerializedObject so = new SerializedObject(obstaclePool.objectReferenceValue);
            so.Update();

            //Afficher objet reference
            EditorGUILayout.PropertyField(so.FindProperty(nameof(poolScript.reference)));

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            //Afficher longueur de la pool 
            SerializedProperty pool = so.FindProperty(nameof(poolScript.pool));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField(pool.arraySize, GUILayout.MaxWidth(50));
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("+"))
            {
                RebuilPool(
                    pool,
                    pool.arraySize + obstacleQuantity,
                    so.FindProperty(nameof(poolScript.reference)).objectReferenceValue as GameObject,
                    so.FindProperty(nameof(poolScript.self)).objectReferenceValue as Transform);
            }
            if (GUILayout.Button("-"))
            {
                RebuilPool(
                    pool,
                    pool.arraySize - obstacleQuantity,
                    so.FindProperty(nameof(poolScript.reference)).objectReferenceValue as GameObject,
                    so.FindProperty(nameof(poolScript.self)).objectReferenceValue as Transform);
            }

            obstacleQuantity = EditorGUILayout.IntField("Quantity", obstacleQuantity);
            if (obstacleQuantity < 0)
                obstacleQuantity = 0;
            EditorGUILayout.EndHorizontal();


            //Afficher barre d'utilisation de la pool
            int arraySize = so.FindProperty(nameof(poolScript.pool)).arraySize;
            int used = so.FindProperty(nameof(poolScript.usedObjectsCount)).intValue;
            float result = (float)used / (float)arraySize;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), result, "Pool usage : " + (result * 100).ToString() + " %");
            so.ApplyModifiedProperties();
        }
        #endregion

        GUILayout.Space(30);
        EditorGUILayout.LabelField("Other References", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(playerStartRef);
        EditorGUILayout.PropertyField(levelFinishRef);

        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    public void CreateLevelProfile()
    {
        ChunkProfile newLevel = ScriptableObject.CreateInstance<ChunkProfile>();
        
        AssetDatabase.CreateAsset(newLevel, "Assets/NewLevelProfile.asset");

        //OpenEditor Window
        OpenWindow(newLevel);
    }

    void OpenWindow(ChunkProfile profile)
    {
        ChunkEditorWindow editWindow;
        if (!EditorWindow.HasOpenInstances<ChunkEditorWindow>())
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            editWindow = EditorWindow.CreateWindow<ChunkEditorWindow>("Chunk Editor Window", new Type[] { inspectorType });
        }
        editWindow = EditorWindow.GetWindow(typeof(ChunkEditorWindow)) as ChunkEditorWindow;

        editWindow.InitWindow(profile, target as LevelManager);
        editWindow.Show();
    }

    private void RebuilPool(SerializedProperty pool, int poolSize, GameObject reference, Transform parent)
    {
        int size = poolSize;
        if (size < 0) size = 0;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        serializedObject.Update();


        //détruire les objets du pool
        while (pool.arraySize > 0)
        {
            SerializedProperty cur = pool.GetArrayElementAtIndex(0);
            System.Object cb = cur.objectReferenceValue;
            if (cb == null) pool.DeleteArrayElementAtIndex(0);
            else
            {
                DestroyImmediate((cb as GameObject).gameObject);
                pool.DeleteArrayElementAtIndex(0);
            }

        }

        //recréér le bon nombre d'objets
        //les remettre enfants de cet objet, les positionner correctement
        //les référencer dans l'array
        
        for (int i = 0; i < size; i++)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(reference, parent) as GameObject;

            pool.InsertArrayElementAtIndex(pool.arraySize);
            pool.GetArrayElementAtIndex(pool.arraySize - 1).objectReferenceValue = go;
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}
