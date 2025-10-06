using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EfectoMuerteLava : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Quemarse(System.Action onFinish)
    {
        // Color inicial
        Color original = sr.color;

        // Tintar a rojo y empezar a desvanecer
        float duracion = 1f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            // Interpola entre original y rojo
            sr.color = Color.Lerp(original, Color.red, t);

            // También baja la opacidad
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - t);

            yield return null;
        }

        // Restaurar color
        sr.color = original;

        // Avisar que terminó
        if (onFinish != null)
            onFinish();
    }
}
