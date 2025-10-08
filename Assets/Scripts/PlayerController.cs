    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform transform;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject spawn;
    bool enSuelo = false;
    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        setCameraPosition();
    }

    private void setCameraPosition(){
        Vector3 newCameraPosition = cameraObject.transform.position;
        newCameraPosition.x = transform.position.x;  // Sincroniza la posición X
        cameraObject.transform.position = newCameraPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Suelo"))
            {
                enSuelo = true;
                animator.SetBool("touchingFloor", true);
            }

            if (collision.gameObject.CompareTag("Muerte"))
            {
                Vector3 posicion = spawn.transform.position;
                transform.position = posicion;
            }

        }

    public void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new UnityEngine.Vector2(+speed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new UnityEngine.Vector2(-speed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            rb.velocity = new UnityEngine.Vector2(0, rb.velocity.y);
        }

        if (Math.Abs(rb.velocity.x) > 0.1)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.W) & enSuelo)
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, +jumpSpeed);
            enSuelo = false;
            animator.SetBool("touchingFloor", false);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            animator.SetTrigger("attack");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            animator.SetTrigger("attack");
        }

        if (Input.GetKey(KeyCode.G))
        {
            animator.SetTrigger("dance");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetTrigger("crouch");
        }

    }

}
