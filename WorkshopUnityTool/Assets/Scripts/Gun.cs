using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject bulletPrefab;
    public GameObject bulletContainer;
    [HideInInspector]public List<BulletBehaviour> bullets;
    public int bulletPoolSize;
    public ParticleSystem shellParticles;
    public ParticleSystem destroyParticles;

    [Header("Balancing")]
    public float spread;
    public float rate;
    private float rateTimer;

    [Header("Camera")]
    public Camera mainCamera;
    public Screenshake camKnockback;
    public Screenshake destroyShake;

    [Header("Slow Motion")]
    [Range(0,1)]
    public float targetSpeed;
    public float sloMoDuration;
    public float easeInSpeed;
    public float easeOutSpeed;


    private float slowMoTimer = 0;
    private Coroutine slowMotion;

    // Start is called before the first frame update
    void Start()
    {
        //Get cam
        mainCamera = Camera.main;

        //Init Bullet
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject go =  Instantiate(bulletPrefab, bulletContainer.transform.position, Quaternion.identity, bulletContainer.transform);
            go.SetActive(false);
            bullets.Add(go.GetComponent<BulletBehaviour>());
            bullets[bullets.Count - 1].destroyParticles = destroyParticles;
            bullets[bullets.Count - 1].destroyShake = destroyShake;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            rateTimer += Time.deltaTime;
            if(rateTimer > rate)
            {
                Shoot();
                rateTimer = 0;
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine(SlowMotion(targetSpeed, sloMoDuration, easeInSpeed, easeOutSpeed));
        }
    }

    void Shoot()
    {
        var direction = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);

        var mousPos = Input.mousePosition;
        var position = mainCamera.ScreenToWorldPoint(new Vector3(mousPos.x, mousPos.y, 10));

        

        camKnockback._Direction = -direction;

        if (camKnockback.trauma + 0.2 > camKnockback.maxTrauma)
            camKnockback.trauma = camKnockback.maxTrauma;
        else
            camKnockback.trauma += 0.2f;

        direction = Quaternion.Euler(0,0, Random.Range(0-spread, 0+spread)) * direction;
        direction.Normalize();

        for (int i = 0; i < bulletPoolSize; i++)
        {
            if(!bullets[i].gameObject.activeSelf)
            {
                bullets[i].gameObject.SetActive(true);
                bullets[i].gameObject.transform.localPosition = transform.position;
                bullets[i].direction = direction;
                shellParticles.Play();
                break;
            }
        }
    }



    private IEnumerator SlowMotion(float targetSpeed, float duration, float easeInSpeed, float easeOutSpeed)
    {
        slowMoTimer = 0;
        while (Time.timeScale > targetSpeed)
        {
            slowMoTimer += Time.fixedDeltaTime * easeInSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetSpeed, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = targetSpeed;

        yield return new WaitForSecondsRealtime(duration);

        slowMoTimer = 0;
        while (slowMoTimer <= 1)
        {
            slowMoTimer += Time.fixedDeltaTime * easeOutSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        slowMoTimer = 0;
        Time.timeScale = 1;
    }
}
