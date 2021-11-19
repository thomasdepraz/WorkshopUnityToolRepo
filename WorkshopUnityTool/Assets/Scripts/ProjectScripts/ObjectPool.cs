using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform self;
    public List<GameObject> pool = new List<GameObject>();
    public GameObject reference;
    public int usedObjectsCount;

    public void Start()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            Instantiate(pool[i],self);
        }
    }

    public GameObject GetFreeElement()
    {
        print($"Getting in {name}");
        for (int i = 0; i < pool.Count; i++)
        {
            UnityEditor.EditorGUIUtility.PingObject(pool[i]);
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

#if UNITY_EDITOR
    int cache;
    public void CountActive()
    {
        cache = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if(pool[i].activeSelf)
            {
                cache++;
            }
        }
        usedObjectsCount = cache;
    }

    private void Update()
    {
        CountActive();
    }
#endif
}
