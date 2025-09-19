using UnityEngine;

public class PocionVida : MonoBehaviour
{
    [Header("Configuraci�n de la poci�n")]
    public int cantidadRecuperacion = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            jugador.RecuperarVida(cantidadRecuperacion);
            Destroy(gameObject); // destruir la poci�n al recogerla
        }
    }
}