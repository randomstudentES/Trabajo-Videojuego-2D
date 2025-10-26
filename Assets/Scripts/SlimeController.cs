using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float velocidad = 1f;
    public Transform sensorSuelo;
    public float distanciaSensor = 0.2f;
    public LayerMask capaSuelo;

    private bool mirandoDerecha = true;

    void Update()
    {
        // Movimiento constante
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);

        Vector2 direccionRaycast = mirandoDerecha ? new Vector2(1, -1).normalized : new Vector2(-1, -1).normalized;

        // Lanzar raycast
        RaycastHit2D hit = Physics2D.Raycast(sensorSuelo.position, direccionRaycast, distanciaSensor, capaSuelo);

        // Si no detecta suelo, girar
        if (hit.collider == null)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1; // Invertir sprite
        transform.localScale = escala;

        velocidad *= -1;
    }

    private void OnDrawGizmosSelected()
    {
        if (sensorSuelo != null)
        {
            Gizmos.color = Color.red;
            Vector2 direccion = mirandoDerecha ? new Vector2(1, -1).normalized : new Vector2(-1, -1).normalized;
            Gizmos.DrawLine(sensorSuelo.position, sensorSuelo.position + (Vector3)direccion * distanciaSensor);
        }
    }
}
