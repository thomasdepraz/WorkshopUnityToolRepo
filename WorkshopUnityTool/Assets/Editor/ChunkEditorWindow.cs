using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ChunkEditorWindow : EditorWindow
{
    LevelManager currentLevelManager;
    ChunkProfile currentChunk;
    SerializedProperty widthProp, heightProp, tilesProp, colorsProp, currentTileSelected;
    SerializedObject serializedObject;

    public string tempPath;

    bool isMouseDown;
    float marginRatio;
    int enumLength;
    bool disableGoalButton;
    bool disablePlayerButton;

    //Start
    public void InitWindow(ChunkProfile chunk, LevelManager levelManager = null)
    {
        currentLevelManager = levelManager;
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

        colorsProp.arraySize = enumLength;
    }

    //Update
    private void OnGUI()
    {
        if (serializedObject == null)
        {
            EditorWindow w = EditorWindow.GetWindow(typeof(ChunkEditorWindow));
            w.Close();
        }


        ProcessEvent();
        serializedObject.Update();
        heightProp.intValue = 9;
        widthProp.intValue = 17;
        if (tilesProp.arraySize != 161)
            tilesProp.arraySize = 161;

        //Color hard set
        if(colorsProp.arraySize != enumLength)
        {
            colorsProp.arraySize = enumLength;
            for (int i = 0; i < colorsProp.arraySize; i++)
            {
                switch(i)
                {
                    case 0: colorsProp.GetArrayElementAtIndex(0).colorValue = Color.black; break;

                    case 1: colorsProp.GetArrayElementAtIndex(1).colorValue = Color.yellow; break;

                    case 2: colorsProp.GetArrayElementAtIndex(2).colorValue = Color.red; break;

                    case 3: colorsProp.GetArrayElementAtIndex(3).colorValue = Color.white ; break;

                    case 4: colorsProp.GetArrayElementAtIndex(4).colorValue = Color.cyan; break;

                    default:
                        break;
                }

            }
        }

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
            if((i == (int)TileType.LevelFinish && disableGoalButton) || (i == (int)TileType.PlayerSpawner && disablePlayerButton))
            {
                EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button(Enum.GetName(typeof(TileType), i)))
                {
                    currentTileSelected.enumValueIndex = i;
                }
                colorsProp.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(colorsProp.GetArrayElementAtIndex(i).colorValue);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                if (GUILayout.Button(Enum.GetName(typeof(TileType), i)))
                {
                    currentTileSelected.enumValueIndex = i;
                }
               colorsProp.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(colorsProp.GetArrayElementAtIndex(i).colorValue);
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        #endregion

        float totalWidth = EditorGUIUtility.currentViewWidth;
        float gridWidth = totalWidth * (1f - 2f * marginRatio);
        GUILayout.Space(20);
        Rect nextRect = EditorGUILayout.GetControlRect();

        Rect backGroundArea = new Rect(nextRect.x + totalWidth * marginRatio - 10 , nextRect.y -10, gridWidth + 20, gridWidth / 1.89f + 20);
        Rect area = new Rect(nextRect.x + totalWidth * marginRatio, nextRect.y, gridWidth, gridWidth / 1.89f);
        EditorGUI.DrawRect(backGroundArea, colorsProp.GetArrayElementAtIndex(currentTileSelected.enumValueIndex).colorValue);
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

        for (int y = 0; y < heightProp.intValue; y++)
        {
            float curX =  area.x;
            for (int x = 0; x < widthProp.intValue; x++)
            {
                curX += spaceWidth;
                
                Rect rect = new Rect(curX, curY, cellWidth, cellWidth);
                curX += cellWidth;

                int tileIndex = y * widthProp.intValue + x;


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

        if(nextRect.y !=0)
        {
            disableGoalButton = GoalAlreadyDefined();
            disablePlayerButton = PlayerAlreadyDefined();
        }

        #region Errors
        if (!disableGoalButton)
        {
            EditorGUILayout.HelpBox("Level needs one LevelFinish to be saved", MessageType.Warning);
        }
        if (!disablePlayerButton)
        {
            EditorGUILayout.HelpBox("Level needs one PlayerSpawner to be saved", MessageType.Warning);
        }
        #endregion

        #region Buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear", GUILayout.MaxWidth(60)))
        {
            for (int i = 0; i < tilesProp.arraySize; i++)
            {
                tilesProp.GetArrayElementAtIndex(i).enumValueIndex = (int)TileType.None;
            }

        }
        if (disableGoalButton && disablePlayerButton && !currentChunk.saved)
        {
            if (GUILayout.Button("Save", GUILayout.MaxWidth(60)))
            {
                if (!currentChunk.saved)
                    Save();
            }
        }
        EditorGUILayout.EndHorizontal();
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
                {
                    isMouseDown = false;
                    currentTileSelected.enumValueIndex = (int)TileType.None;
                }

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
                {
                    isMouseDown = false;
                    currentTileSelected.enumValueIndex = (int)TileType.None;
                }

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

    private void OnDestroy()
    {
        if(!IsLevelValid() && !currentChunk.saved)
        {
            //Destroy the asset at temp path
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(currentChunk));
            AssetDatabase.SaveAssets();
        }
        else
        {
            //Save asset to fianl path and add it to levelManagerList
            if(!currentChunk.saved)
                Save();
        }
    }

    bool IsLevelValid()
    {
        if (disableGoalButton && disablePlayerButton)
            return true;
        else
            return false;
    }

    void Save()
    {
        //Save asset to final Path
        string path = EditorUtility.SaveFilePanelInProject("Save file", "LevelDesign_", "asset", "");
        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(currentChunk), path);
        AssetDatabase.SaveAssets();
        currentChunk.saved = true;
        EditorUtility.SetDirty(currentChunk);

        //add To level ManagerPool if needed
        if(currentLevelManager!= null && !currentLevelManager.chunkPool.Contains(currentChunk))
        {
            currentLevelManager.chunkPool.Add(currentChunk);

            EditorGUIUtility.PingObject(currentLevelManager);

            //close window
            Close();
        }
    }
}
