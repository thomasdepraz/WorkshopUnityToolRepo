using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public float raycastDistance;
    public LayerMask obstacleLayer;
    public ParticleSystem destroyParticles;
    public Screenshake destroyShake;
    public ContactFilter2D obstacleFilter;
    RaycastHit2D[] hits = new RaycastHit2D[1];
    [HideInInspector] public Vector2 direction = Vector2.zero; 

    // Start is called before the first frame update
    void Start()
    {
        obstacleFilter.layerMask = obstacleLayer;
        obstacleFilter.useLayerMask = true;
    }


    // Update is called once per frame
    void Update()
    {
        transform.localPosition += (Vector3)direction * speed * Time.deltaTime;
        //transform.forward = direction;

        
        //Raycast to Destroy
        if(Physics2D.Raycast(transform.localPosition, direction, obstacleFilter, hits, speed * Time.deltaTime) > 0)
        {
            var wall = hits[0].collider.gameObject.GetComponent<Wall>();
            wall.OnHit();
            destroyParticles.transform.position = transform.position;
            destroyParticles.Play();
            destroyShake.trauma += 0.3f;
            gameObject.SetActive(false);
        }
    }

}
