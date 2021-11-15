using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(CharacterAttack))]
public class CharacterAttackInspector : Editor
{
    CharacterAttack script;

    public GUIStyle titleStyle;

    //Private Variables
    GameObject bulletPoolParent;

    void OnEnable()
    {
        script = target as CharacterAttack;
    }

    public override void OnInspectorGUI()
    {
        script.self = EditorGUILayout.ObjectField(nameof(script.self), script.self, typeof(Transform),true) as Transform;

        EditorGUILayout.BeginHorizontal();//Buttons for bullet list control

        bool add = GUILayout.Button("+");
        bool remove = GUILayout.Button("-");
        int count = 0; 
        count = EditorGUILayout.IntField(count, GUILayout.MaxWidth(30));
        if(add)
        {
            //Instantiate objects
            if (bulletPoolParent == null)
                bulletPoolParent = new GameObject("BulletPoolParent");
        }
        if(remove)
        {
            //Remove/Destroy objects
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear"))
        {
            if(bulletPoolParent != null)
                DestroyImmediate(bulletPoolParent.gameObject);
        }

        if(script.bullets.Count == 0)
        {
            //Draw Error
        }

        EditorGUILayout.EndHorizontal();

    }

}
