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
    public Screenshake destroyShake;
    public ParticleSystem destroyParticle;

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

        //if (Collision() && gameObject.activeSelf)
        //{
        //    ResetBullet();
        //}
    }

    public void ResetBullet()
    {
        DestroyFeedback();
        direction = Vector2.zero;
        gameObject.SetActive(false);
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
        //Particles
        destroyParticle.transform.position = self.position;
        destroyParticle.Play();

        //Screenshake if available
        if(destroyShake != null)
        {
            destroyShake.trauma += 0.3f;
        }
    }

    public void OnBecameInvisible()//The bullet exits the screen
    {
        ResetBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ResetBullet();
    }
}
