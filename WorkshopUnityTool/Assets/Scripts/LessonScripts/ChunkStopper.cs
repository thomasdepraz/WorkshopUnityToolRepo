using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkStopper : MonoBehaviour
{
    [Header("References")]
    public ChunkPoolManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.parent.GetComponent<ChunkBehaviour>().StopMoving();

        manager.SendNewChunk();
    }
}
