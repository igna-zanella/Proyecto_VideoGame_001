using UnityEngine;

public class BolaEnemigo : MonoBehaviour
{
    [Header("Da�o")]
    public int dano = 1;

    [Header("Tiempo de vida (segundos)")]
    public float duracion = 5f;

    private Vector2 direccion;
    private float velocidad = 5f;

    void Start()
    {
        Destroy(gameObject, duracion); // destruir autom�ticamente despu�s de unos segundos
    }

    public void Configurar(Vector2 dir, float vel)
    {
        direccion = dir.normalized;
        velocidad = vel;
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // aplicar da�o al jugador
            MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.serAtacado(Vector2.zero);
            }

            Destroy(gameObject); // destruir la bola al impactar
        }
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject); // desaparecer si toca el suelo
        }
    }
}