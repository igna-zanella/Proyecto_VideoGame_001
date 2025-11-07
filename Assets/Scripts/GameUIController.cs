using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIController : MonoBehaviour
{
    [Header("Menú de sonido en el HUD")]
    [SerializeField] private GameObject menuSonido;

    [Header("Jugador")]
    [SerializeField] private MovimientoJugador jugador;
    [SerializeField] private Vector3 checkpointInicial;
    private Vector3 checkpointActual;

    [Header("Vidas del jugador")]
    [SerializeField] private int vidasTotales = 3;
    private int vidasRestantes;

    private bool menuActivo = false;

    // Identificar la escena actual ---
    private string escenaActual;
    void Start()
    {
        if (menuSonido != null)
            menuSonido.SetActive(false);

        vidasRestantes = vidasTotales;

        jugador = FindFirstObjectByType<MovimientoJugador>();
        escenaActual = SceneManager.GetActiveScene().name;

        if (jugador == null)
            jugador = FindFirstObjectByType<MovimientoJugador>();

        checkpointActual = checkpointInicial;
        vidasRestantes = vidasTotales;
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
        Debug.Log("Intentando activar menú sonido. Estado actual: " + menuSonido.activeSelf);
        menuSonido.SetActive(menuActivo);
        Time.timeScale = menuActivo ? 0f : 1f;
    }

    public void ActualizarCheckpoint(Vector3 nuevoPunto)
    {
        checkpointActual = nuevoPunto;
        Debug.Log("[GameUIController] Checkpoint actualizado: " + nuevoPunto);
    }

    public void JugadorMurio()
    {
        vidasRestantes--;

        if (vidasRestantes > 0)
        {
            Debug.Log("[GameUIController] Jugador murió. Reiniciando desde checkpoint...");
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            Debug.Log("[GameUIController] Sin vidas restantes. Reiniciando nivel...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(1f); // pequeña pausa

        if (jugador == null)
            jugador = FindFirstObjectByType<MovimientoJugador>();

        if (jugador != null)
        {
            jugador.transform.position = checkpointActual;
            jugador.ReiniciarEnergia();
            Debug.Log("[GameUIController] Jugador respawneado en " + checkpointActual);
        }
        else
        {
            Debug.LogWarning("[GameUIController] No se encontró jugador para respawnear.");
        }
    }

    public int GetVidasRestantes()
    {
        return vidasRestantes;
    }

    // ------------------------------------------------------------
    // 🎯 Compatibilidad con scripts antiguos de checkpoint
    // ------------------------------------------------------------
    public void EstablecerCheckpoint(Vector3 nuevoPunto)
    {
        ActualizarCheckpoint(nuevoPunto);
    }


}
