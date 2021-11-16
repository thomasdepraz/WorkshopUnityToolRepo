using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ChunkEditorWindow : EditorWindow
{
    ChunkProfile currentChunk;
    SerializedProperty widthProp, heightProp, tilesProp, colorsProp, currentTileSelected;
    SerializedObject serializedObject;


    bool isMouseDown;
    float marginRatio;

    //Start
    public void InitWindow(ChunkProfile chunk)
    {
        currentChunk = chunk;
        serializedObject = new SerializedObject(currentChunk);
        isMouseDown = false;
        marginRatio = 0.05f;

        heightProp = serializedObject.FindProperty(nameof(currentChunk.height));
        widthProp = serializedObject.FindProperty(nameof(currentChunk.width));
        tilesProp = serializedObject.FindProperty(nameof(currentChunk.tiles));
        colorsProp = serializedObject.FindProperty(nameof(currentChunk.tileColors));
        currentTileSelected = serializedObject.FindProperty(nameof(currentChunk.currentTile));
    }

    //Update
    private void OnGUI()
    {
        ProcessEvent();
        serializedObject.Update();
        #region TOP GUI
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Current Chunk", currentChunk, typeof(ChunkProfile));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(widthProp);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(heightProp);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(currentTileSelected);
        EditorGUILayout.PropertyField(colorsProp);

        heightProp.intValue = widthProp.intValue;


        float totalWidth = EditorGUIUtility.currentViewWidth;
        float gridWidth = totalWidth * (1f - 2f * marginRatio);
        Rect nextRect = EditorGUILayout.GetControlRect();


        Rect area = new Rect(nextRect.x + totalWidth * marginRatio, nextRect.y, gridWidth, gridWidth);
        EditorGUI.DrawRect(area, Color.gray);


        EditorUtility.SetDirty(currentChunk);

        if (heightProp.intValue < 0) return;
        if (widthProp.intValue < 0) return;

        #endregion

        float cellToSpaceRatio = 4f;
        float totalCellWidth = gridWidth * (cellToSpaceRatio)/(cellToSpaceRatio + 1f);
        float cellWidth = totalCellWidth / (float)widthProp.intValue;
        float totalSpaceWitdh = gridWidth - totalCellWidth;
        float spaceWidth = totalSpaceWitdh / ((float)widthProp.intValue + 1);
        float curY = area.y + spaceWidth;

        for (int i = 0; i < heightProp.intValue; i++)
        {
            float curX =  area.x;

            for (int j = 0; j < widthProp.intValue; j++)
            {
                curX += spaceWidth;
                
                Rect rect = new Rect(curX, curY, cellWidth, cellWidth);
                curX += cellWidth;

                int tileIndex = j * heightProp.intValue + i;


                bool isPaintingOverThis = isMouseDown && rect.Contains(Event.current.mousePosition);
                if(isPaintingOverThis)
                {
                    //tilesProp.GetArrayElementAtIndex(tileIndex).enumValueIndex = currentTileSelected.enumValueIndex;
                }


                int enumIndexInPalette = tilesProp.GetArrayElementAtIndex(tileIndex).enumValueIndex;
                Color col = colorsProp.GetArrayElementAtIndex(enumIndexInPalette).colorValue;
                EditorGUI.DrawRect(rect, col);
            }
            curY += cellWidth;
            curY += spaceWidth;
            GUILayout.Space(5);
        }

        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    void ProcessEvent()
    {
        if(Event.current.type == EventType.MouseDown)
        {
            isMouseDown = true;
        }
        if (Event.current.type == EventType.MouseUp)
        {
            isMouseDown = false;
        }
    }
}
