using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;  
    private float lifetime = 5f;
    private Rigidbody2D rb;
    private GameObject player;
    private SpriteRenderer sr;
    
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Destroy(gameObject, lifetime);

        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            sr.flipX = true;
            rb.velocity = Vector2.right * (speed * -1);
        } else
        {
            rb.velocity = Vector2.right * speed;
        }

        
    }

}
