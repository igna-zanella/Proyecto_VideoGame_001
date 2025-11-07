//
//BLOQUE COMENTADO ANTERIORMENTE
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

//
//
//}
//FIN BLOQUE COMENTADO ANTERIORMENTE

//using UnityEngine;
//using UnityEngine.UI;

//public class VidaUIControlador : MonoBehaviour
//{
//    private Image imagenVida;
//    private int vidaTotal;

//    void Start()
//    {
//        // tomar el componente Image del propio objeto
//        imagenVida = GetComponent<Image>();

//        // el total de vida lo debe configurar el jugador en Start

//        //vidaTotal = FindObjectOfType<MovimientoJugador>().getVida();
//        vidaTotal = FindFirstObjectByType<MovimientoJugador>().getVida();
//    }

//    public void ActualizarVida(int vidaActual)
//    {
//        if (imagenVida != null && vidaTotal > 0)
//        {
//            imagenVida.fillAmount = (float)vidaActual / vidaTotal;
//        }
//    }

//    public int getVidaTotal()
//    {
//        return vidaTotal;
//    }

//    // Permite al jugador configurar explícitamente la vida total
//    public void ConfigurarVidaTotal(int total)
//    {
//        vidaTotal = total;
//        ActualizarVida(total);
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class VidaUIControlador : MonoBehaviour
{
    private Slider sliderVida;
    private int vidaTotal = 10; // valor por defecto

    void Start()
    {
        sliderVida = GetComponent<Slider>();

        var jugador = FindFirstObjectByType<MovimientoJugador>();
        if (jugador != null)
        {
            vidaTotal = jugador.GetVida();
        }

        if (sliderVida != null)
        {
            sliderVida.maxValue = 10;
            sliderVida.value = vidaTotal;
        }
    }

    public void ActualizarVida(int vidaActual)
    {
        if (sliderVida != null)
        {
            sliderVida.value = vidaActual;
                       
        }
        else
        {
            Debug.LogWarning("Error sin slider");
        }
    }

    // Permite al jugador configurar explícitamente la vida total
    public void ConfigurarVidaTotal(int total)
    {
        vidaTotal = total;
        if (sliderVida != null)
        {
            sliderVida.maxValue = total;
            sliderVida.value = total;
        }
    }

    public int getVidaTotal()
    {
        return vidaTotal;
    }
}

