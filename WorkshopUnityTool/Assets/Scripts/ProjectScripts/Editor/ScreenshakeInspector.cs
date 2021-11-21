using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Screenshake))]
public class ScreenshakeInspector : Editor
{
    Screenshake script;
    SerializedProperty transformTarget;
    SerializedProperty seedVector;
    SerializedProperty speedValue;
    SerializedProperty maxMagnitudeValue;
    SerializedProperty noiseMagnitudeValue;

    SerializedProperty traumaValue;
    SerializedProperty maxTraumaValue;
    SerializedProperty traumaDecreaseValue;
    SerializedProperty isTimeBased;
    SerializedProperty isRotational;
    SerializedProperty rotationalSpreadValue;

    GUIStyle titleStyle;
    float testTrauma = 0;

    private void OnEnable()
    {
        transformTarget = serializedObject.FindProperty(nameof(script._Target));
        seedVector = serializedObject.FindProperty(nameof(script._Seed));
        speedValue = serializedObject.FindProperty(nameof(script._Speed));
        maxMagnitudeValue = serializedObject.FindProperty(nameof(script._MaxMagnitude));
        noiseMagnitudeValue = serializedObject.FindProperty(nameof(script._NoiseMagnitude));

        traumaValue = serializedObject.FindProperty(nameof(script.trauma));
        maxTraumaValue = serializedObject.FindProperty(nameof(script.maxTrauma));
        traumaDecreaseValue = serializedObject.FindProperty(nameof(script.traumaDecreaseSpeed));
        isTimeBased = serializedObject.FindProperty(nameof(script.timeBased));
        isRotational = serializedObject.FindProperty(nameof(script.rotational));
        rotationalSpreadValue = serializedObject.FindProperty(nameof(script.rotationalSpread));
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        # region Style Creation
        titleStyle = new GUIStyle(EditorStyles.boldLabel);
        if (titleStyle.fontSize != 20)
        {
            titleStyle.fontSize = 20;
        }
        #endregion

        serializedObject.Update();

        GUILayout.Space(5);
        EditorGUILayout.LabelField("Data", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(transformTarget);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(seedVector);
        if(GUILayout.Button("Random", GUILayout.MaxWidth(60)))
        {
            seedVector.vector2Value = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));    
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.PropertyField(speedValue);
        EditorGUILayout.PropertyField(maxMagnitudeValue);
        EditorGUILayout.PropertyField(noiseMagnitudeValue);

        EditorGUILayout.PropertyField(isRotational);
        if(isRotational.boolValue)
        {
            EditorGUILayout.PropertyField(rotationalSpreadValue);
        }

        EditorGUILayout.PropertyField(isTimeBased);

        GUILayout.Space(30);
        EditorGUILayout.LabelField("Trauma", titleStyle, GUILayout.MinHeight(20));
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(maxTraumaValue);
        EditorGUILayout.PropertyField(traumaDecreaseValue);
        GUILayout.Space(10);
        float value = traumaValue.floatValue / maxTraumaValue.floatValue;
        EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), value, "Trauma : " + (value * 100).ToString() + "%");

        if(Application.isPlaying)
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            testTrauma = EditorGUILayout.FloatField("Trauma to add ",testTrauma);
            if (testTrauma > maxTraumaValue.floatValue) testTrauma = maxTraumaValue.floatValue;
            if (testTrauma < 0) testTrauma = 0;

            if (GUILayout.Button("Add Trauma", GUILayout.MaxWidth(80)))
            {
                traumaValue.floatValue += testTrauma;
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
