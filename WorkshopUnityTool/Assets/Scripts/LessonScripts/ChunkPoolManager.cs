using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPoolManager : MonoBehaviour
{
    public Transform self;
    public List<ChunkBehaviour> chunks = new List<ChunkBehaviour>();
    public GameObject chunkPrefab;


    //Tool Variables
#if UNITY_EDITOR
    public bool foldoutBool;
    public int poolSize;
#endif

    public void Start()
    {
        SendNewChunk();
    }
    public void SendNewChunk()
    {
        GetFreeElement().StartMoving();
    }

    public ChunkBehaviour GetFreeElement()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            if (!chunks[i].enabled)
            {
                return chunks[i];
            }
        }
        Debug.LogWarning("No chunk found");

        //on l'appele mais on as pas vraiment envie de le faire;
        return ExtendPool();
    }

    private ChunkBehaviour ExtendPool()
    {
        GameObject go = Instantiate(chunkPrefab, self);
        ChunkBehaviour cb = go.GetComponent<ChunkBehaviour>();
        if (!cb) cb = go.AddComponent<ChunkBehaviour>();
        return cb;
    }
}
