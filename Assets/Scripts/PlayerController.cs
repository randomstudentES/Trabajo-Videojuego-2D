    using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private GameObject spawnProjectile;
    [SerializeField] private GameObject projectilePrefab;
    private float delayDisparos = 0.3f;
    private float ultimoDisparo = 0;
    bool enSuelo = false;
    [SerializeField] private int maxDisparos;
    private int disparosDisponibles;
    private bool recargando = false;
    [SerializeField] private int maxVidas;
    private int vidas;

    public Image[] corazones;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private GameObject puntuacionTexto;
    private TextMeshProUGUI tmp;
    
    public int puntuacion = 0;

    public List<Sprite> inventario = new List<Sprite>();
    [SerializeField] Image[] objetosInventario;
    [SerializeField] GameObject inventarioEntero;

    private AudioSource audiosource;
    [SerializeField] public AudioClip recogerPocion;

    private bool hielo = false;

    void Start()
    {
        inventarioEntero.SetActive(false);
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        disparosDisponibles = maxDisparos;
        vidas = maxVidas;
        tmp = puntuacionTexto.GetComponent<TextMeshProUGUI>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        setCameraPosition();
    }

    public void sumarPuntos(int puntos)
    {
        puntuacion += 100;
        tmp.text = "Puntuacion: " + puntuacion;
    }

    private void setCameraPosition(){
        Vector3 newCameraPosition = cameraObject.transform.position;
        newCameraPosition.x = transform.position.x;
        cameraObject.transform.position = newCameraPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Hielo"))
            {
                enSuelo = true;
                animator.SetBool("touchingFloor", true);
            }

            if (collision.gameObject.CompareTag("Suelo"))
            {
                hielo = false;
            } else
            {
                hielo = true;
            }

            if (collision.gameObject.CompareTag("Muerte"))
        {
            revivir();
        }

            if (collision.gameObject.tag == "Enemy")
            {
                actualizarCorazones();
                vidas -= 1;
                if (vidas == 0)
                {
                    revivir();
                }
            }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pincha")
        {
            actualizarCorazones();
            vidas -= 1;
            if (vidas == 0)
            {
                revivir();
            }
        }

        if (collision.gameObject.tag == "Object")
        {
            actualizarInvetario(collision);
        }
    }

    public void actualizarInvetario(Collider2D collision)
    {
        inventario.Add(collision.gameObject.GetComponent<SpriteRenderer>().sprite);
        for (int i = 0; i < inventario.Count; i++)
        {
            objetosInventario[i].sprite = inventario[i];
            Image img = objetosInventario[i];  // Tu Image
            Color color = img.color;  // Copiar color
            color.a = 1f;             // Cambiar alfa
            img.color = color;
        }

        audiosource.PlayOneShot(recogerPocion);

        Destroy(collision.gameObject);

    }

    public void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new UnityEngine.Vector2(+speed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
            spawnProjectile.GetComponent<Transform>().position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new UnityEngine.Vector2(-speed, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
            spawnProjectile.GetComponent<Transform>().position = new Vector2(transform.position.x - 0.5f, transform.position.y);
        }
        else if (!hielo)
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

        if (Input.GetKey(KeyCode.Q) && Time.time >= ultimoDisparo + delayDisparos && disparosDisponibles > 0 && !recargando)
        {
            animator.SetTrigger("attack");
            CreateProjectile();
            ultimoDisparo = Time.time;
        }

        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(recargar());
        }

        if (Input.GetKey(KeyCode.G))
        {
            animator.SetTrigger("dance");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetTrigger("crouch");
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            inventarioEntero.SetActive(!inventarioEntero.activeSelf);
        }

    }

    void CreateProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnProjectile.GetComponent<Transform>().position, spawnProjectile.GetComponent<Transform>().rotation);
        disparosDisponibles -= 1;
    }

    IEnumerator recargar()
    {
        recargando = true;
        disparosDisponibles = maxDisparos;
        yield return new WaitForSeconds(3);
        recargando = false;
    }

    void revivir()
    {
        Vector3 posicion = spawn.transform.position;
        transform.position = posicion;
        vidas = maxVidas;
        llenarTodosCorazones();
    }

    void actualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidas - 1)
                corazones[i].sprite = fullHeart;
            else
                corazones[i].sprite = emptyHeart;
        }
    }

    void llenarTodosCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].sprite = fullHeart;
        }
    }

}
