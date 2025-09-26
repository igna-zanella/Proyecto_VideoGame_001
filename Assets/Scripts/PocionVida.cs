//using UnityEngine;

//public class PocionVida : MonoBehaviour
//{
//    [Header("Configuraci�n de la poci�n")]
//    public int cantidadRecuperacion = 2;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

//        if (jugador != null)
//        {
//            jugador.RecuperarVida(cantidadRecuperacion);
//            Destroy(gameObject); // destruir la poci�n al recogerla
//        }
//    }
//}

using UnityEngine;

public class PocionVida : MonoBehaviour
{
    [Header("Curaci�n")]
    public int cantidadRecuperacion = 2; // suma 2 de vida por poci�n

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            if (jugador != null)
            {
                jugador.RecibirCura(cantidadRecuperacion);
                Destroy(gameObject); // destruir la poci�n luego de recogerla
            }
        }
    }
}
