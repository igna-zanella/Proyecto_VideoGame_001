
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Referencias a submenús (prefabs instanciados en escena)")]
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuSonido;

    public void IniciarJuego()
    {
        Debug.Log("Iniciando Juego...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel_001");
    }

    public void SalirJuego()
    {
        //Debug.LogWarning("Saliendo...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // --- Submenús ---
    public void ActivarConfigSonido()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (menuSonido != null) menuSonido.SetActive(true);
    }

    public void VolverAlMenuPrincipal()
    {
        if (menuSonido != null) menuSonido.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
    }
}