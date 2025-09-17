using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Menús ya en escena (Variante A)")]
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject menuSonido;

    [Header("Prefab del menú sonido (Variante B)")]
    [SerializeField] private GameObject menuSonidoPrefab;
    [SerializeField] private GameObject volver;
    private GameObject menuSonidoInstancia;
    public void IniciarJuego()
    {
        print("Iniciando Juego");
        SceneManager.LoadScene("Nivel_001");
    }

    public void SalirJuego()
    {
        Debug.LogWarning("Saliendo...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    //public void ActivarConfigSonido()
    //{
    // SceneManager.UnloadSceneAsync("Menu");
    //    print("l31");
    // SceneManager.LoadScene("MenuSonido", LoadSceneMode.Additive);
    //    print("l32");
    //}

    // --------------------
    // VARIANTE A (activar/desactivar en escena)
    // --------------------
    public void ActivarConfigSonido_A()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (menuSonido != null) menuSonido.SetActive(true);
        print("opción A");
    }

    public void VolverAlMenuPrincipal_A()
    {
        if (menuSonido != null) menuSonido.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        Debug.LogWarning("Saliendo...");
        print("vuelve");
    }

    // --------------------
    // VARIANTE B (instanciar prefab en runtime)
    // --------------------
    public void ActivarConfigSonido_B()
    {
        if (menuSonidoInstancia == null && menuSonidoPrefab != null)
        {
            // Instanciar el prefab dentro del mismo Canvas
            Transform parentCanvas = transform.parent;
            menuSonidoInstancia = Instantiate(menuSonidoPrefab, parentCanvas);
        }

        gameObject.SetActive(false); // ocultar menú actual
        if (menuSonidoInstancia != null) menuSonidoInstancia.SetActive(true);
    }

    public void VolverAlMenuPrincipal_B()
    {
        if (menuSonidoInstancia != null) menuSonidoInstancia.SetActive(false);
        gameObject.SetActive(true); // reactivar menú principal
        Debug.LogWarning("Saliendo...");
        print("vuelve");
    }

}
