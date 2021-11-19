using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    None, 
    Obstacle, 
    Enemy, 
    PlayerSpawner,
    LevelFinish,
}


[CreateAssetMenu(fileName = "New Chunk Profile", menuName = "Chunk/Profile", order = 0)]
public class ChunkProfile : ScriptableObject
{
    public int width, height;

#if UNITY_EDITOR
    public Color[] tileColors;
    public TileType currentTile;
    public bool saved;
#endif
    public TileType[] tiles;
}


