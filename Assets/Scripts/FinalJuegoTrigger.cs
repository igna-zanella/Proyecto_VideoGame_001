using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

public class FinalJuegoTrigger : MonoBehaviour
{
    [Header("UI Final del Juego")]
    [SerializeField] private GameObject imagenFin; // PNG con "FIN" (activar/desactivar)
    [Tooltip("Overlay negro (Image) que cubre toda la pantalla — opcional. Si se deja vacío, intentará usar FadePantalla.IniciarFadeOutYCambioEscena si existe.")]
    [SerializeField] private Image overlayImage;

    [Header("Tiempos")]
    [SerializeField] private float duracionCongelado = 2f;
    [SerializeField] private float duracionFadeOut = 2f;

    private bool finActivado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finActivado) return;

        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();
        if (jugador != null)
        {
            finActivado = true;
            StartCoroutine(SecuenciaFinal(jugador));
        }
    }

    private IEnumerator SecuenciaFinal(MovimientoJugador jugador)
    {
        // 1) Detener el movimiento del jugador y controles
        jugador.enabled = false;
        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 2) Mostrar la imagen de “FIN”
        if (imagenFin != null)
            imagenFin.SetActive(true);

        // 3) Silenciar la música (fade out en tiempo real)
        if (SoundController.Instance != null)
        {
            // intenta usar un método público del SoundController si existe
            // si no existe, hacemos fade directo del AudioSource que encuentre
            StartCoroutine(FadeOutMusicaSoundController(duracionFadeOut));
        }
        else
        {
            // fallback: intentar hacer fade en cualquier AudioSource de la escena
            StartCoroutine(FadeOutAllAudioSources(duracionFadeOut));
        }

        // 4) Congelar el juego en pantalla (tiempo real)
        Time.timeScale = 0f;

        // 5) Esperar el tiempo de "congelado" en tiempo real
        yield return new WaitForSecondsRealtime(duracionCongelado);

        // 6) Fade visual: preferimos overlayImage (si está asignada)
        if (overlayImage != null)
        {
            yield return StartCoroutine(FadeImageAlpha(overlayImage, 0f, 1f, duracionFadeOut));
            // restaurar tiempo antes de cambiar escena
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuInicio");
            yield break;
        }

        // 7) Si no hay overlayImage, intentamos llamar a FadePantalla.IniciarFadeOutYCambioEscena("MenuInicio") si existe
        FadePantalla fade = FindObjectOfType<FadePantalla>();
        if (fade != null)
        {
            MethodInfo mi = fade.GetType().GetMethod("IniciarFadeOutYCambioEscena", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (mi != null)
            {
                // Restaurar timeScale antes de permitir que el fade manager haga la carga
                Time.timeScale = 1f;
                mi.Invoke(fade, new object[] { "MenuInicio" });
                yield break;
            }
        }

        // 8) Fallback: no hay overlay ni FadePantalla adecuado -> cargar escena directamente
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    // Fade de la imagen (usa tiempo real -> unscaledDeltaTime)
    private IEnumerator FadeImageAlpha(Image img, float fromAlpha, float toAlpha, float duration)
    {
        if (img == null) yield break;
        Color c = img.color;
        c.a = fromAlpha;
        img.color = c;
        img.gameObject.SetActive(true);

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            c.a = a;
            img.color = c;
            yield return null;
        }
        c.a = toAlpha;
        img.color = c;
    }

    // Fade out del AudioSource del SoundController (si existe)
    private IEnumerator FadeOutMusicaSoundController(float duracion)
    {
        // Intentamos obtener AudioSource del SoundController (si tiene uno)
        AudioSource musica = null;
        var sc = SoundController.Instance;
        if (sc != null)
        {
            musica = sc.GetComponent<AudioSource>();
        }

        if (musica == null)
        {
            yield break;
        }

        float inicial = musica.volume;
        float t = 0f;
        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            musica.volume = Mathf.Lerp(inicial, 0f, t / duracion);
            yield return null;
        }
        musica.volume = 0f;
        musica.Stop();
    }

    // Fallback: fade out de todos los AudioSources en la escena
    private IEnumerator FadeOutAllAudioSources(float duracion)
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        float t = 0f;
        // guardar volúmenes iniciales
        float[] iniciales = new float[sources.Length];
        for (int i = 0; i < sources.Length; i++) iniciales[i] = sources[i].volume;

        while (t < duracion)
        {
            t += Time.unscaledDeltaTime;
            float factor = 1f - Mathf.Clamp01(t / duracion);
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null)
                    sources[i].volume = iniciales[i] * factor;
            }
            yield return null;
        }

        foreach (var s in sources)
            if (s != null) s.Stop();
    }
}
