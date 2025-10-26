using UnityEngine;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
    public float rangoDeteccion = 5f;
    private Transform player;
    private bool persiguiendo = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        StartCoroutine(WaitForPlayer());
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null)
        {
            float distancia = Vector2.Distance(transform.position, player.position);
            persiguiendo = distancia <= rangoDeteccion;
        }

        movimientoRender();
    }

    void FixedUpdate()
    {
        if (persiguiendo && player != null)
        {
            Vector2 direccion = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direccion.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void movimientoRender()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            sr.flipX = rb.velocity.x > 0;
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    private IEnumerator WaitForPlayer()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            yield return null;
        }
    }
}
