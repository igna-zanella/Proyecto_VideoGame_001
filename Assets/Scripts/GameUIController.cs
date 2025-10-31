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

    // Identificar la escena actual ---
    private string escenaActual;

    void Start()
    {
        if (menuSonido != null)
            menuSonido.SetActive(false);

        vidasRestantes = vidasTotales;

        jugador = FindFirstObjectByType<MovimientoJugador>();
        escenaActual = SceneManager.GetActiveScene().name;

        if (jugador != null)
        {
            checkpoint = jugador.transform.position;
        }

        Debug.Log($"[GameUIController] Escena actual: {escenaActual}");
    }

    void Update()
    {
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
        Time.timeScale = menuActivo ? 0f : 1f;
    }

    // --- Checkpoints ---
    public void EstablecerCheckpoint(Vector3 pos)
    {
        checkpoint = pos;
        Debug.Log("Checkpoint actualizado en: " + pos);
    }

    // --- Cuando el jugador muere ---
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

            if (escenaActual == "Nivel_001")
            {
                SceneManager.LoadScene("Nivel_001"); // reinicia nivel 1
            }
            else if (escenaActual == "Nivel_002")
            {
                SceneManager.LoadScene("Nivel_002"); // reinicia nivel 2
            }
            else
            {
                // fallback genérico
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
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
        else
        {
            Debug.LogWarning("[GameUIController] No se encontró el jugador para hacer respawn.");
        }
    }
}
