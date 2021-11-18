using System;
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
    int enumLength;
    bool disableGoalButton;
    bool disablePlayerButton;

    //Start
    public void InitWindow(ChunkProfile chunk)
    {
        currentChunk = chunk;
        serializedObject = new SerializedObject(currentChunk);
        isMouseDown = false;
        marginRatio = 0.05f;
        enumLength = Enum.GetNames(typeof(TileType)).Length;

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
        heightProp.intValue = 9;
        widthProp.intValue = 17;
        if (tilesProp.arraySize != 161)
            tilesProp.arraySize = 161;

        if(colorsProp.arraySize != enumLength)
            colorsProp.arraySize = enumLength;

        #region Top Disabled GUI
        EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Current Chunk", currentChunk, typeof(ChunkProfile));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(widthProp);
                EditorGUILayout.PropertyField(heightProp);
            EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Buttons + Colors GUI

        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < enumLength; i++)
        {
            EditorGUILayout.BeginVertical();
            //GUI.backgroundColor = colorsProp.GetArrayElementAtIndex(i).colorValue;
            //if(currentTileSelected.enumValueIndex.)
                if (GUILayout.Button(Enum.GetName(typeof(TileType), i)))
                {
                    currentTileSelected.enumValueIndex = i;
                }
            colorsProp.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(colorsProp.GetArrayElementAtIndex(i).colorValue); 
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        #endregion

        float totalWidth = EditorGUIUtility.currentViewWidth;
        float gridWidth = totalWidth * (1f - 2f * marginRatio);
        Rect nextRect = EditorGUILayout.GetControlRect();

        Rect area = new Rect(nextRect.x + totalWidth * marginRatio, nextRect.y, gridWidth, gridWidth / 1.89f);
        EditorGUI.DrawRect(area, Color.gray);

        EditorUtility.SetDirty(currentChunk);

        serializedObject.ApplyModifiedProperties();
        //if (nextRect.y == 0) return;
        if (heightProp.intValue < 0) return;
        if (widthProp.intValue < 0) return;
        serializedObject.Update();

        #region GridArea
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
                if(isPaintingOverThis && nextRect.y!=0)
                {
                    tilesProp.GetArrayElementAtIndex(tileIndex).enumValueIndex = currentTileSelected.enumValueIndex;
                }


                int enumIndexInPalette = tilesProp.GetArrayElementAtIndex(tileIndex).enumValueIndex;
                
                Color col = colorsProp.GetArrayElementAtIndex(enumIndexInPalette).colorValue;
                EditorGUI.DrawRect(rect, col);
            }
            curY += cellWidth;
            curY += spaceWidth;
        }
        #endregion
        GUILayout.Space(area.height + 10);
        #region Buttons
        if (GUILayout.Button("Clear"))
        {
            for (int i = 0; i < tilesProp.arraySize; i++)
            {
                tilesProp.GetArrayElementAtIndex(i).enumValueIndex = (int)TileType.None;
            }
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    bool GoalAlreadyDefined()
    {
        for (int i = 0; i < tilesProp.arraySize; i++)
        {

            if (tilesProp.GetArrayElementAtIndex(i).enumValueIndex == (int)TileType.LevelFinish)
            {
                if(currentTileSelected.enumValueIndex == (int)TileType.LevelFinish)
                    currentTileSelected.enumValueIndex = (int)TileType.None;

                return true;
            }
        }
        return false;
    }

    bool PlayerAlreadyDefined()
    {
        for (int i = 0; i < tilesProp.arraySize; i++)
        {
            if (tilesProp.GetArrayElementAtIndex(i).enumValueIndex == (int)TileType.PlayerSpawner)
            {
                if (currentTileSelected.enumValueIndex == (int)TileType.PlayerSpawner)
                    currentTileSelected.enumValueIndex = (int)TileType.None;

                return true;
            }
        }
        return false;
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
