using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
//[CustomEditor(typeof(MyRuntimeScript))]
public class ProperInspector : Editor
{
    MyRuntimeScript script;

    SerializedProperty someColor;
    SerializedProperty someFloat;
    SerializedProperty someCurve;
    SerializedProperty someTransform;
    SerializedProperty someColors;
    SerializedProperty someStruct;
    SerializedProperty subSp;




    private void OnEnable()
    {
        someColor = serializedObject.FindProperty(nameof(script.someColor));
        someFloat = serializedObject.FindProperty(nameof(script.someFloat));
        someCurve = serializedObject.FindProperty(nameof(script.someCurve));
        someTransform = serializedObject.FindProperty(nameof(script._transform));
        someColors = serializedObject.FindProperty(nameof(script.colorArray));
        someStruct = serializedObject.FindProperty(nameof(script.someStruct));

        subSp = someStruct.FindPropertyRelative("structFloat"); 
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(someColor);
        EditorGUILayout.PropertyField(someFloat);

        if (someFloat.floatValue < 0)
            someFloat.floatValue = 0;

        EditorGUILayout.PropertyField(someCurve);
        EditorGUILayout.PropertyField(someTransform);

        EditorGUILayout.PropertyField(someColors);
        EditorGUILayout.PropertyField(someStruct);
        EditorGUILayout.PropertyField(subSp);
   

        serializedObject.ApplyModifiedProperties();

        if(GUILayout.Button("Transfrom forward"))
        {
            SerializedObject so = new SerializedObject(someTransform.objectReferenceValue);
            SerializedProperty sp = so.FindProperty("m_LocalPosition");

            so.Update();
            sp.vector3Value += Vector3.forward * 10;
            so.ApplyModifiedProperties();
        }
    }

}
