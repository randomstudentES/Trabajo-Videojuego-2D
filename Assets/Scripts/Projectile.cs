using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;  
    private float lifetime = 2f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("Dead");
            StartCoroutine(DestroyEnemyAfterDelay(collision.gameObject, 0.4f));

            player.GetComponent<PlayerController>().puntuacion += 100;
            Debug.Log(player.GetComponent<PlayerController>().puntuacion);

        }
    }

    private IEnumerator DestroyEnemyAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(enemy);
    }

}
