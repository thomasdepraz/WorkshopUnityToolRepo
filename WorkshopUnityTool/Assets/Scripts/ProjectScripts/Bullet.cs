using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class Bullet : MonoBehaviour
{
    [Header("Data")]
    public float speed;
    public LayerMask enemyLayerMasks;
    public LayerMask playerLayerMasks;
    public bool isEnemyBullet;

    [Header("References")]
    public Transform self;

    //Hidden Variables
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool isActive;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hits = new RaycastHit2D[1];

    public void Start()
    {
        if (isEnemyBullet)
            contactFilter.layerMask = enemyLayerMasks;
        else
            contactFilter.layerMask = playerLayerMasks;

        contactFilter.useLayerMask = true;
    }

    public void Shoot(Vector2 dir, Transform origin)
    {
        direction = dir;
        self.position = origin.position;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        //MoveBullet
        self.Translate(direction * speed * Time.deltaTime);

        if (Collision() && gameObject.activeSelf)
        {
            ResetBullet();
        }
    }

    public void ResetBullet()
    {
        gameObject.SetActive(false);
        direction = Vector2.zero;
        DestroyFeedback();
    }

    public bool Collision()
    {
        if (Physics2D.Raycast(self.localPosition, direction, contactFilter, hits, speed * Time.deltaTime)> 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyFeedback()
    {

    }

    public void OnBecameInvisible()//The bullet exits the screen
    {
        ResetBullet();
    }
}
