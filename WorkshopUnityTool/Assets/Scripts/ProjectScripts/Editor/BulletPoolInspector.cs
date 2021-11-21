using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletPoolManager))]
public class BulletPoolInspector : Editor
{

    BulletPoolManager script;
    SerializedProperty self;
    SerializedProperty bulletList;
    SerializedProperty bulletReference;
    SerializedProperty particleReference;

    GUIStyle titleStyle;

    int quantity;


    private void OnEnable()
    {
        self = serializedObject.FindProperty(nameof(script.self));
        bulletList = serializedObject.FindProperty(nameof(script.bullets));
        bulletReference = serializedObject.FindProperty(nameof(script.bulletPrefab));
        particleReference = serializedObject.FindProperty(nameof(script.destroyParticle));
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

        #region Pool
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Pool", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(bulletReference);
        if(bulletReference.objectReferenceValue != null)
        {
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField(bulletList.arraySize, GUILayout.MaxWidth(50));
            EditorGUI.EndDisabledGroup();

            //Afficher petit bouton +/ moins
            if (GUILayout.Button("+"))
            {
                RebuilPool(
                    bulletList,
                    bulletList.arraySize + quantity,
                    bulletReference.objectReferenceValue as GameObject,
                    self.objectReferenceValue as Transform);
            }
            if (GUILayout.Button("-"))
            {
                RebuilPool(
                    bulletList,
                    bulletList.arraySize - quantity,
                    bulletReference.objectReferenceValue as GameObject,
                    self.objectReferenceValue as Transform);
            }

            quantity = EditorGUILayout.IntField("Quantity", quantity);
            if (quantity < 0) quantity = 0; //clamp to 0


            EditorGUILayout.EndHorizontal();

            //Afficher barre d'utilisation de la pool
            int arraySize = bulletList.arraySize;
            int used = serializedObject.FindProperty(nameof(script.usedObjectsCount)).intValue;
            float result = (float)used / (float)arraySize;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), result, "Pool usage : " + (result * 100).ToString() + " %");
        }
        #endregion 

        #region Other References
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Other References", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(self);
        EditorGUILayout.PropertyField(particleReference);
        #endregion

        serializedObject.ApplyModifiedProperties();

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
                DestroyImmediate((cb as Bullet).gameObject);
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
