using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform self;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject reference;
    public GameObject GetFreeElement()
    {
        for (int i = 0; i < pool.Count; i++)
        { 
            if (!pool[i].activeSelf)
            {
                return pool[i];
            }
        }

        Debug.LogWarning("No object found, creating one");

        return ExtendPool();
    }

    private GameObject ExtendPool()
    {
        GameObject go = Instantiate(reference, self);
        return go;
    }

    public void ResetPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].SetActive(false);
        }
    }
}
