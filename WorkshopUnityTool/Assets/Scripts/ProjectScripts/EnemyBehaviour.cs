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
    private BulletPoolManager enemyBulletPool;
    private Transform playerTransform;


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

        //Get fire dir + look at
        fireDir = GameManager.Instance.playerController.self.position - self.position;
        fireDir = Quaternion.Euler(new Vector3(0, 0, Random.Range(-fireSpread, fireSpread))) * fireDir;
        fireDir.Normalize();
        self.up = Vector3.Lerp(self.up, (Vector3)fireDir, Time.deltaTime);
    }

    void Move()
    {

    }

    void Shoot()
    {
        fireRateTimer += Time.deltaTime;
        if(fireRateTimer >= fireRate)
        {
            GameManager.Instance.enemyBullets.GetFreeElement().Shoot(fireDir, self);

            fireRateTimer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        GameManager.Instance.currentkillCount++;
    }
}
