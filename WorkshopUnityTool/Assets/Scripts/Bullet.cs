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
    public Transform originTransform;

    //Hidden Variables
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool isActive;


    private void Update()
    {
        //MoveBullet
        self.Translate(direction * speed * Time.deltaTime);
        print(direction);

        if (Collision() && gameObject.activeSelf)
            ResetBullet();
    }

    public void ResetBullet()
    {
        gameObject.SetActive(false);
        self.position = originTransform.position;
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
