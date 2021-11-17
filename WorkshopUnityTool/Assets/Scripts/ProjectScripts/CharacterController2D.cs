using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] float speed;
    [SerializeField] AnimationCurve accelerationCurve;
    [Range(0f, 1f)] [SerializeField] float accelerationSpeed;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode fire;


    [Header("References")]
    [SerializeField] Transform self;
    [SerializeField] Transform graphTransfrom;
    [SerializeField] CharacterAttack attack;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] Camera mainCam;

    private Vector2 movement;
    private Vector2 lastMovement;
    float currentAcceleration;
    float accelerationTimeStamp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameOver
        deathParticles.transform.position = self.position;
        deathParticles.Play();
        gameObject.SetActive(false);

        //Restart the game somehow
    }
}

