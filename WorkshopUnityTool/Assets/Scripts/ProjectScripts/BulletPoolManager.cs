using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public Transform self;
    public List<Bullet> bullets = new List<Bullet>();
    public GameObject bulletPrefab;
    public ParticleSystem destroyParticle;

    // Start is called before the first frame update
    [HideInInspector]public int usedObjectsCount;
    void Start()
    {
        //init particles on bullets
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].destroyParticle = destroyParticle;
        }
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
        b.destroyParticle = destroyParticle;
        return b;
    }

#if UNITY_EDITOR
    int cache;

    public void CountActive()
    {
        cache = 0;
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].gameObject.activeSelf)
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
