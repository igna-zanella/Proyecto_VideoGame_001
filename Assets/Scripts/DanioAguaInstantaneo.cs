using UnityEngine;

public class DanioAguaInstantaneo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            // Quita toda la vida de una sola vez
            jugador.serAtacado(Vector2.zero);

            // Opción más directa: destruir al jugador
            //Destroy(jugador.gameObject);
        }
    }
}
