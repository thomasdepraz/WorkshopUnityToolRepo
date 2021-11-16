using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
//[CustomEditor(typeof(CharacterAttack))]
public class CharacterAttackInspector : Editor
{
    CharacterAttack script;
    SerializedProperty selfTransform;
    SerializedProperty bulletPrefab;
    SerializedProperty bullets;
    SerializedProperty count;


    public GUIStyle titleStyle;

    //Private Variables
    bool doOnce;
    GameObject bulletPoolParent;

    void OnEnable()
    {
        script = target as CharacterAttack;
        selfTransform = serializedObject.FindProperty(nameof(script.self));
        bulletPrefab = serializedObject.FindProperty(nameof(script.bulletPrefab));
        //bullets = serializedObject.FindProperty(nameof(script.bullets));
        count = serializedObject.FindProperty(nameof(script.count)); 
    }

    public override void OnInspectorGUI()
    {
        #region Create GUIStyle
        //Create Style
        if(!doOnce)
        {
            titleStyle = new GUIStyle(EditorStyles.label);
            titleStyle.fontSize = 32;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontStyle = FontStyle.Bold;
            doOnce = true;
        }
        #endregion

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", script, typeof(CharacterAttack),true);
        EditorGUI.EndDisabledGroup();

        serializedObject.Update(); 

        EditorGUILayout.PropertyField(selfTransform);
        EditorGUILayout.PropertyField(bulletPrefab);

        #region BulletPool
        EditorGUILayout.LabelField("Bullet Pool", titleStyle, GUILayout.MinHeight(40));
        EditorGUILayout.BeginHorizontal();//Buttons for bullet list control

        bool add = GUILayout.Button("+");
        bool remove = GUILayout.Button("-");
        EditorGUILayout.PropertyField(count);
        if (count.intValue < 0)
            count.intValue = 0;

        if (add && count.intValue > 0)
        {
            //Instantiate objects
            if (bulletPoolParent == null)
                bulletPoolParent = new GameObject("BulletPoolParent");

            for (int i = 0; i < count.intValue; i++)
            {
                GameObject go = Instantiate(bulletPrefab.objectReferenceValue as GameObject, bulletPoolParent.transform);
                Bullet curBullet = go.GetComponent<Bullet>();
                //curBullet.originTransform = selfTransform.objectReferenceValue as Transform;
                //script.bullets.Add(curBullet);
            }
        }
        if(remove && count.intValue > 0)
        {
            //Remove/Destroy objects
            for (int i = 0; i < count.intValue; i++)
            {
                //DestroyImmediate(script.bullets[script.bullets.Count - 1].gameObject);
                //script.bullets.RemoveAt(script.bullets.Count - 1);
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear"))
        {
            bullets.ClearArray();

            if(bulletPoolParent != null)
                DestroyImmediate(bulletPoolParent.gameObject);
        }

        //if(script.bullets.Count == 0)
        //{
            //Draw Error
        //}
       
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(bullets);
        #endregion

        serializedObject.ApplyModifiedProperties();
    }
}
