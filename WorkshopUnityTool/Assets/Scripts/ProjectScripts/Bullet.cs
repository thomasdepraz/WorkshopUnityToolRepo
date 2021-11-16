using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class Bullet : MonoBehaviour
{
    [Header("Data")]
    public float speed;

    [Header("References")]
    public Transform self;

    //Hidden Variables
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool isActive;

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
            ResetBullet();
    }

    public void ResetBullet()
    {
        gameObject.SetActive(false);
        direction = Vector2.zero;
        DestroyFeedback();
    }

    public bool Collision()
    {
        return false;
    }

    public void DestroyFeedback()
    {

    }

    public void OnBecameInvisible()//The bullet exits the screen
    {
        ResetBullet();
    }
}
