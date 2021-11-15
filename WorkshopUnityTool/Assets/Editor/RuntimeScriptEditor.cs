using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(MyRuntimeScript))]
public class RuntimeScriptEditor : Editor
{
    MyRuntimeScript _target;
    GUIStyle style;
    private void OnEnable()
    {
        _target = (MyRuntimeScript)target;
        style = new GUIStyle();
        style.fontSize = 64;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
    }
    
    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField("TITLE", style, GUILayout.MinHeight(50));

        _target._transform = (Transform)EditorGUILayout.ObjectField("Self", _target._transform, typeof(Transform), true);
        _target.someColor = EditorGUILayout.ColorField("Some Color", _target.someColor);

        EditorGUI.BeginChangeCheck();
        float newValue = EditorGUILayout.FloatField("Some Float", _target.someFloat);
        
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_target, "tweakedfloat");
            _target.someFloat = newValue;
            if (_target.someFloat < 0) _target.someFloat = 0;
        }


    }

    private void OnValidate()
    {
        Repaint();
    }
}
