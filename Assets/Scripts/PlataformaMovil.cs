using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Movimiento")]
    public Vector3 puntoA;
    public Vector3 puntoB;
    public float velocidad = 2f;

    private Vector3 objetivoActual;

    void Start()
    {
        transform.position = puntoA;
        objetivoActual = puntoB;
    }

    void Update()
    {
        // Movimiento entre A y B
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual, velocidad * Time.deltaTime);

        // Cambio de dirección al llegar
        if (Vector3.Distance(transform.position, objetivoActual) < 0.05f)
        {
            objetivoActual = (objetivoActual == puntoA) ? puntoB : puntoA;
        }
    }

    // Permitir que el jugador "se mueva" con la plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
