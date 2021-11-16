using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public Transform self;
    public List<Bullet> bullets = new List<Bullet>();
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public Bullet GetFreeElement()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if(!bullets[i].gameObject.activeSelf)
            {
                return bullets[i];
            }
        }

        Debug.LogWarning("No bullet found, creating one");

        return ExtendPool();
    }

    private Bullet ExtendPool()
    {
        GameObject bullet = Instantiate(bulletPrefab, self);
        Bullet b = bullet.GetComponent<Bullet>();
        if (!b) b = bullet.AddComponent<Bullet>();
        return b;
    }
}