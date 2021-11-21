using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] float speed;
    [SerializeField] AnimationCurve accelerationCurve;
    [Range(0f, 1f)] [SerializeField] float accelerationSpeed;
    [SerializeField] float slowMotionMaxTime;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode fire;


    [Header("References")]
    [SerializeField] public Transform self;
    [SerializeField] Transform graphTransfrom;
    [SerializeField] CharacterAttack attack;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Camera mainCam;

    private Vector2 movement;
    private Vector2 lastMovement;
    float currentAcceleration;
    float accelerationTimeStamp;
    [HideInInspector] public bool canMove = true;

    //prvate variables
    private float timer;
    private float slowMoTimer = 0;
    private Coroutine slowMotion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            Move();
        }
        SlowMotion();
    }


    void Move()
    {
        movement = Vector2.zero;
        if(Input.GetKey(left)) // CheckRaycast too
        {
            movement.x--;
        }
        if (Input.GetKey(right)) // CheckRaycast too
        {
            movement.x++;
        }
        if(Input.GetKey(up)) // CheckRaycast too
        {
            movement.y++;
        }
        if (Input.GetKey(down)) // CheckRaycast too
        {
            movement.y--;
        }

        //Process acceleration
        accelerationTimeStamp += Time.deltaTime * accelerationSpeed;

        if(Vector2.Dot(movement, lastMovement) < 0.8f)
            accelerationTimeStamp = 0;

        currentAcceleration = accelerationCurve.Evaluate(accelerationTimeStamp);

        //Movement
        self?.Translate(movement.normalized * speed * currentAcceleration * Time.deltaTime);

        //Get last movement value
        lastMovement = new Vector2(movement.x, movement.y);


        //Tilt object based on movement;
        Vector3 mousPos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10));

        graphTransfrom.up = mousPos - self.position;
    }

    public void SlowMotion()
    {
        if (Input.GetMouseButton(1))
        {
            timer += Time.deltaTime;
            if (timer >= slowMotionMaxTime) timer = slowMotionMaxTime;

            if (timer < slowMotionMaxTime && slowMotion == null)
            {
                slowMotion = StartCoroutine(SlowMotion(0.5f, 3, 1, 1));
            }
            if (timer >= slowMotionMaxTime && slowMotion != null)
            {
                StopCoroutine(slowMotion);
                StartCoroutine(SlowMotion(1, 3, 1, 1));
                slowMotion = null;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;

        }
        if (Input.GetMouseButtonUp(1))
        {
            if (timer < slowMotionMaxTime)
            {
                if (slowMotion != null)
                {
                    StopCoroutine(slowMotion);
                    StartCoroutine(SlowMotion(1, 3, 1, 1));
                }
                slowMotion = null;
            }

        }

        GameManager.Instance.slowMotionFillBar.fillAmount = 1 - timer / slowMotionMaxTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameOver
        deathParticles.transform.position = self.position;
        deathParticles.Play();
        gameObject.SetActive(false);

        //Restart the game somehow
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
    }
}

