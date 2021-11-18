using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(PathingRuntime))]
public class PathingInspector : Editor
{
    PathingRuntime script;
    SerializedProperty wayPoints;
    Transform self;

    ReorderableList rList;

    private void OnEnable()
    {
        wayPoints = serializedObject.FindProperty(nameof(script.waypoints));
        self = (target as PathingRuntime).transform;

        //Init List 
        rList = new ReorderableList(serializedObject, wayPoints, true, true, true, true);

        //Prepare callbacks
        rList.onAddCallback += Add;
        rList.onRemoveCallback += Remove;
        //rList.onReorderCallback += Move;
        rList.drawElementCallback += ElementDrawer;
        rList.drawHeaderCallback += HeaderDrawer;
        rList.elementHeightCallback += ElementHeight;
    }




    #region Callbacks
    private void Add(ReorderableList list)
    {
        wayPoints.arraySize++;
    }
    private void Remove(ReorderableList list)
    {
        wayPoints.DeleteArrayElementAtIndex(rList.index);
    }
    /*
    private void Move(ReorderableList list)
    {
        throw new NotImplementedException();
    }*/
    private void HeaderDrawer(Rect rect)
    {
        EditorGUI.LabelField(rect,"List Title");
    }
    private void ElementDrawer(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, wayPoints.GetArrayElementAtIndex(index));
    }
    private float ElementHeight(int index)
    {

        float lines = EditorGUIUtility.currentViewWidth > 331 ? 1 : 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 1;
        return lines * lineHeight;
    }
    #endregion
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        rList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < wayPoints.arraySize; i++)
        {
            SerializedProperty prop = wayPoints.GetArrayElementAtIndex(i);

            EditorGUI.BeginChangeCheck();
               Vector3 newPos = Handles.PositionHandle(self.position + prop.vector3Value, self.rotation);
            if(EditorGUI.EndChangeCheck()) prop.vector3Value = newPos - self.position;

            int nextIndex = (i + 1) % wayPoints.arraySize;
            SerializedProperty nextProp = wayPoints.GetArrayElementAtIndex(nextIndex);

            Handles.DrawLine(prop.vector3Value + self.position, nextProp.vector3Value + self.position,3f);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
