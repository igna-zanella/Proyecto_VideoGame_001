using UnityEngine;

public class DanioAguaInstantaneo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            // Quita toda la vida de una sola vez
            //jugador.serAtacado(Vector2.zero);

            // Opci�n m�s directa: destruir al jugador
            //Destroy(jugador.gameObject);
            // Avisamos al GameUIController de que el jugador muri�
            GameUIController ui = FindFirstObjectByType<GameUIController>();
            if (ui != null)
            {
                jugador.ReiniciarEnergia();
                ui.JugadorMurio();
            }
        }
    }
}
