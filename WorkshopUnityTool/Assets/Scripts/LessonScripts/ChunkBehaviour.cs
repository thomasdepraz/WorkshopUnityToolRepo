using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBehaviour : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public Transform self;

#if UNITY_EDITOR
    public ChunkProfile profile;
#endif


    public void Awake()
    {
       enabled = false;
    }

    public void StartMoving()
    {
        self.localPosition = Vector3.zero;
        enabled = true;
    }

    public void StopMoving()
    {
        enabled = false;
    }

    void Update()
    {
        self.Translate(Vector3.down * speed * Time.deltaTime);
    }

}
