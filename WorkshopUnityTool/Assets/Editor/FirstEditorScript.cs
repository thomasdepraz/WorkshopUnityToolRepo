using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class FirstEditorScript : Editor
{
    [MenuItem("Tools/HelloWorld %w")]
    public static void HelloWorld()
    {
        GameObject go = GameObject.FindObjectOfType<Light>().gameObject;
        EditorGUIUtility.PingObject(go);
        Light light = go.GetComponent<Light>();

        Undo.RecordObject(light, "Lumière rouge");

        light.color = Color.red;
        light.intensity *= 2;

        EditorSceneManager.MarkAllScenesDirty();
    }
}
