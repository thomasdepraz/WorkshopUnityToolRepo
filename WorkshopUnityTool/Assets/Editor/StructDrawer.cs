using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(myStruct))]
public class StructDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float numberOfLines = 2;
        float lineHeight = EditorGUIUtility.singleLineHeight;

        return lineHeight * numberOfLines;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight + 1;

        Rect topZone = new Rect(position.x, position.y, position.width,lineHeight);
        Rect leftZone = new Rect(topZone.x, topZone.y, topZone.width * 0.5f, lineHeight);
        Rect rightZone = new Rect(topZone.x + topZone.width * 0.5f, topZone.y, topZone.width * 0.5f, lineHeight);
        Rect bottomZone = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);

        SerializedProperty speed = property.FindPropertyRelative("structFloat");
        SerializedProperty color = property.FindPropertyRelative("structColor");

        EditorGUI.PropertyField(leftZone, speed);
        EditorGUI.PropertyField(rightZone, color);
        

    }

}
