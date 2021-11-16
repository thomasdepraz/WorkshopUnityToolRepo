using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    private Camera cam;
    public GameObject particleContainer;
    public Screenshake scrShake;
    public Screenshake camKnbk;
    public ParticleSystem particle;
    private ParticleSystem.MainModule particleSettings;
    private ParticleSystem.ShapeModule shape;

    private TextMeshProUGUI text;

    private float timer;
    private float slowMoTimer = 0;
    private Coroutine slowMotion;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        particleSettings = particle.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireParticle();
            CamKnockback();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            timer = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(slowMotion != null )
                StopCoroutine(slowMotion);

            slowMotion = StartCoroutine(SlowMotion(0.5f, 1f, 1f, 1f));
        }

        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;

            if (timer > 0.1f)
            {
                FireParticle();
                timer = 0;
            }
        }

        print(Time.timeScale);
    }

    void FireParticle()
    {
        scrShake.trauma += 0.3f;

        particleSettings.startColor = Random.ColorHSV(0, 1, 0.2f, 0.3f, 1f, 1f, 1, 1);

        particleContainer.transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        particle.Play();
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
        while (Time.timeScale <= 1)
        {
            slowMoTimer += Time.fixedDeltaTime * easeOutSpeed;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, slowMoTimer);
            yield return new WaitForEndOfFrame();
        }

        slowMoTimer = 0;
        Time.timeScale = 1;
    }

    private void CamKnockback()
    {
        var dir = cam.WorldToScreenPoint(Vector3.zero) - Input.mousePosition;
        camKnbk._Direction = -dir;
        camKnbk.trauma += 0.5f;
    }
    
}
