using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUIController : MonoBehaviour
{
    [Header("Jugador")]
    [SerializeField] private MovimientoJugador jugador;
    [SerializeField] private Vector3 checkpointInicial;
    private Vector3 checkpointActual;

    [Header("Vidas del jugador")]
    [SerializeField] private int vidasTotales = 3;
    private int vidasRestantes;

    void Start()
    {
        if (jugador == null)
            jugador = FindFirstObjectByType<MovimientoJugador>();

        checkpointActual = checkpointInicial;
        vidasRestantes = vidasTotales;
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
