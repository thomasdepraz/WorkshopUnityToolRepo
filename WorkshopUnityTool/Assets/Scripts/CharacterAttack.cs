using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform self;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public List<Bullet> bullets = new List<Bullet>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector2 dir)
    {
        //Go through the pool and shoot if inactive
        for (int i = 0; i < bullets.Count; i++)
        {
            if(!bullets[i].gameObject.activeSelf)
            {
                //Shoot bullet;
                bullets[i].self.position = self.position;
                bullets[i].gameObject.SetActive(true);
                bullets[i].direction = dir;
                break;
            }
        }
    }
}
