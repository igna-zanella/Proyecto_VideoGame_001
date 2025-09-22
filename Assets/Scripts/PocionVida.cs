//using UnityEngine;

//public class PocionVida : MonoBehaviour
//{
//    [Header("Configuración de la poción")]
//    public int cantidadRecuperacion = 2;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

//        if (jugador != null)
//        {
//            jugador.RecuperarVida(cantidadRecuperacion);
//            Destroy(gameObject); // destruir la poción al recogerla
//        }
//    }
//}

using UnityEngine;

public class PocionVida : MonoBehaviour
{
    [Header("Curación")]
    public int cantidadRecuperacion = 2; // suma 2 de vida por poción

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            if (jugador != null)
            {
                jugador.RecibirCura(cantidadRecuperacion);
                Destroy(gameObject); // destruir la poción luego de recogerla
            }
        }
    }
}
