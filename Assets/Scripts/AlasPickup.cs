using UnityEngine;

public class AlasPickup : MonoBehaviour
{
    [SerializeField] private AudioClip sonidoAlas; // opcional
    private bool recogido = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recogido) return;

        if (collision.CompareTag("Player"))
        {
            MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.ActivarVuelo();

                if (sonidoAlas != null)
                    AudioSource.PlayClipAtPoint(sonidoAlas, transform.position);

                Destroy(gameObject); // eliminar las alas del suelo
                recogido = true;
            }
        }
    }
}
