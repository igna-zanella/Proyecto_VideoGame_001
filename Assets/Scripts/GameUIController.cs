
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [Header("Menú de sonido en el HUD")]
    [SerializeField] private GameObject menuSonido;

    private bool menuActivo = false;

    [Header("Vidas del jugador")]
    [SerializeField] private int vidasTotales = 3;
    private int vidasRestantes;

    [Header("Checkpoint")]
    private Vector3 checkpoint;
    private MovimientoJugador jugador;

    void Start()
    {
        if (menuSonido != null)
            menuSonido.SetActive(false); // arranca oculto

        vidasRestantes = vidasTotales;

        jugador = FindFirstObjectByType<MovimientoJugador>();

        if (jugador != null)
        {
            checkpoint = jugador.transform.position; // primera posición como checkpoint inicial
        }
    }

    void Update()
    {
        // Abrir/cerrar con la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenuSonido();
        }
    }

    public void ToggleMenuSonido()
    {
        if (menuSonido == null) return;

        menuActivo = !menuActivo;
        menuSonido.SetActive(menuActivo);

        // Pausar/reanudar juego
        Time.timeScale = menuActivo ? 0f : 1f;
    }

    // --- NUEVO: Sistema de vidas ---
    public void EstablecerCheckpoint(Vector3 pos)
    {
        checkpoint = pos;
        Debug.Log("Checkpoint actualizado en: " + pos);
    }

    public void JugadorMurio()
    {
        vidasRestantes--;

        if (vidasRestantes > 0)
        {
            Debug.Log("Jugador respawnea. Vidas restantes: " + vidasRestantes);
            RespawnJugador();
        }
        else
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("MenuInicio"); // ajusta el nombre de tu escena
        }
    }

    private void RespawnJugador()
    {
        if (jugador == null)
        {
            jugador = FindFirstObjectByType<MovimientoJugador>();
        }

        if (jugador != null)
        {
            jugador.transform.position = checkpoint;
            jugador.ReiniciarEnergia();
        }
    }
}