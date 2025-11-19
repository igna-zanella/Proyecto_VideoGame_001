//using UnityEngine;

//public class SoundController : MonoBehaviour
//{
//    public static SoundController Instance { get; private set; } = null;
//    private void Awake()
//    {
//        if (Instance != null && Instance != this )
//        {
//            Destroy(this);
//        }
//        else
//        {
//            Instance = this;
//            DontDestroyOnLoad(this);
//        }
//    }


//}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [Header("Audio general del juego")]
    [SerializeField] private AudioSource musicaFondo;
    [SerializeField] private AudioClip musicaInicio;
    [SerializeField] private AudioClip musicaNivel1;
    [SerializeField] private AudioClip musicaNivel2;

    [Header("Transición de música")]
    [SerializeField] private float duracionFade = 1.5f;

    private string escenaActual;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        escenaActual = SceneManager.GetActiveScene().name;
        ReproducirMusicaPorNivel(escenaActual);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        escenaActual = scene.name;
        ReproducirMusicaPorNivel(escenaActual);
    }

    private void ReproducirMusicaPorNivel(string nombreEscena)
    {
        if (musicaFondo == null)
        {
            musicaFondo = GetComponent<AudioSource>();
            if (musicaFondo == null)
            {
                Debug.LogWarning("[SoundController] No se encontró AudioSource.");
                return;
            }
        }

        AudioClip clip = null;

        if (nombreEscena == "MenuInicio")
        {
            clip = musicaInicio;
        }
        else if (nombreEscena == "Nivel_001")
        {
            clip = musicaNivel1;
        }
        else if (nombreEscena == "Nivel_002")
        {
            clip = musicaNivel2;
        }

        if (clip == null) return;

        if (musicaFondo.clip == clip && musicaFondo.isPlaying)
            return; // ya está sonando el tema correcto

        // Detenemos cualquier fade anterior
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusic(clip));
    }

    private IEnumerator FadeMusic(AudioClip nuevoClip)
    {
        // Fade-out
        float volumenInicial = musicaFondo.volume;
        float t = 0f;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            musicaFondo.volume = Mathf.Lerp(volumenInicial, 0f, t / duracionFade);
            yield return null;
        }

        musicaFondo.Stop();
        musicaFondo.clip = nuevoClip;
        musicaFondo.Play();

        // Fade-in
        t = 0f;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            musicaFondo.volume = Mathf.Lerp(0f, volumenInicial, t / duracionFade);
            yield return null;
        }

        musicaFondo.volume = volumenInicial;
    }
}

