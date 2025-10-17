using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPinchos : MonoBehaviour
{
    private Transform transform;
    private float posicionAbajo;
    private float posicionArriba;
    private bool subiendo = true;
    private bool esperando = false;
    void Start()
    {
        transform = GetComponent<Transform>();
        posicionAbajo = transform.position.y;
        posicionArriba = posicionAbajo + 0.40f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!esperando)
        {
            if (subiendo)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
                if (transform.position.y > posicionArriba)
                {
                    StartCoroutine(cambiarDireccion());
                }
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                if (transform.position.y < posicionAbajo)
                {
                    StartCoroutine(cambiarDireccion());
                }
            }
        }
    }

    IEnumerator cambiarDireccion()
    {
        subiendo = !subiendo;
        esperando = true;
        yield return new WaitForSeconds(1.5f);
        esperando = false;
    }

}
