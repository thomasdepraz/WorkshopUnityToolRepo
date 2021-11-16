using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public float rate;
    [SerializeField] public float spread;
    [SerializeField] public float shootTraumaAmount;

    [Header("References")]
    [SerializeField] public Transform self;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public BulletPoolManager bulletPool;
    [SerializeField] public ParticleSystem shellParticles;
    [SerializeField] public Screenshake knockbackShake;
    [SerializeField] public Screenshake shake;

    //Varibales for customInspector
#if UNITY_EDITOR
    public int count;
    public GameObject bulletParent;
#endif

    //Private variables
    private float rateTimer;
    private Vector2 direction;
    private  Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            direction = Input.mousePosition - cam.WorldToScreenPoint(self.position);
            direction = Quaternion.Euler(new Vector3(0,0,Random.Range(-spread, spread))) * direction;
            rateTimer += Time.deltaTime;
            if(rateTimer >= rate)
            {
                #region ShootFeedback
                knockbackShake._Direction = -direction;

                if (knockbackShake.trauma + 0.2 > knockbackShake.maxTrauma)
                    knockbackShake.trauma = knockbackShake.maxTrauma;
                else
                    knockbackShake.trauma += 0.2f;

                shellParticles.Play();
                #endregion

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
