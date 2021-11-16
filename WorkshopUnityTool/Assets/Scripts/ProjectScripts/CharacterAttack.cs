using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public float rate;
    [SerializeField] public float spread;

    [Header("References")]
    [SerializeField] public Transform self;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public BulletPoolManager bulletPool;

    //Varibales for customInspector
    [SerializeField] public int count;
    [SerializeField] public GameObject bulletParent;

    //Private variables
    private float rateTimer;
    private Vector2 direction;
    public  Camera cam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            direction =  (Vector2)((cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10))) - self.position);
            rateTimer += Time.deltaTime;
            if(rateTimer >= rate)
            {
                Shoot(direction.normalized);
                rateTimer = 0;
            }
        }
    }

    public void Shoot(Vector2 dir)
    {
        bulletPool.GetFreeElement().Shoot(dir, self);
    }
}
