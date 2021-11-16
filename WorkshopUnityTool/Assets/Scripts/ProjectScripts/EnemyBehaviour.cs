using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Data")]
    public float fireRate;
    public float fireSpread;

    [Header("References")]
    public Transform self;
    public BulletPoolManager enemyBulletPool;
    public Transform playerTransform;


    //Private variables 
    private float fireRateTimer;
    private Vector2 fireDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {

    }

    void Shoot()
    {
        fireRateTimer += Time.deltaTime;
        if(fireRateTimer >= fireRate)
        {
            //Shoot from enemy pool
            fireDir = playerTransform.position - self.position;
            fireDir = Quaternion.Euler(new Vector3(0, 0, Random.Range(-fireSpread, fireSpread))) * fireDir;
            fireDir.Normalize();

            enemyBulletPool.GetFreeElement().Shoot(fireDir, self);

            fireRateTimer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
