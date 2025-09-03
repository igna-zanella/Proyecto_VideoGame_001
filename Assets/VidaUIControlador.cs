//using UnityEngine;
//using UnityEngine.UI;

//public class VidaUIControlador : MonoBehaviour
//{
//    private Image imagenVida;
//    private int vidaTotal = 0;
//    void Start()
//    {
//        imagenVida = transform.Find("Vida").GetComponent<Image>();
//        vidaTotal = GetComponentInParent<MovimientoJugador>().getVida();
//    }

//    public void ActualizarVida(int vidaActual)
//    {
//        imagenVida.fillAmount = (float)vidaActual / vidaTotal;
//    }

//}

using UnityEngine;
using UnityEngine.UI;

public class VidaUIControlador : MonoBehaviour
{
    private Image imagenVida;
    private int vidaTotal;

    void Start()
    {
        // Buscar el objeto hijo llamado "Vida"
        imagenVida = transform.Find("Vida").GetComponent<Image>();
    }

    public void ConfigurarVidaTotal(int vidaInicial)
    {
        vidaTotal = vidaInicial;
        ActualizarVida(vidaInicial);
    }

    public void ActualizarVida(int vidaActual)
    {
        if (imagenVida != null && vidaTotal > 0)
        {
            imagenVida.fillAmount = (float)vidaActual / vidaTotal;
        }
    }
}