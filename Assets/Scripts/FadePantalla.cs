using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadePantalla : MonoBehaviour
{
    private Image imagen;
    public float duracionFade = 1f;


    void Start()
    {
        StartCoroutine(FundidoDesdeNegro());
    }

    void Awake()
    {
        imagen = GetComponent<Image>();
    }

    public IEnumerator FundidoYCambioEscena(string nombreEscena)
    {
        // Fundido a negro
        float t = 0f;
        Color color = imagen.color;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / duracionFade);
            imagen.color = color;
            yield return null;
        }

        // Cargar la nueva escena
        SceneManager.LoadScene("Nivel_002");
    }

    public IEnumerator FundidoDesdeNegro()
    {
        float t = 0f;
        Color color = imagen.color;
        color.a = 1f;
        imagen.color = color;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / duracionFade);
            imagen.color = color;
            yield return null;
        }

        color.a = 0f;
        imagen.color = color;
    }


}
