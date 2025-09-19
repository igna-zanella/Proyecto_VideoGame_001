
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("Menú de sonido en el HUD")]
    [SerializeField] private GameObject menuSonido;

    private bool menuActivo = false;

    void Start()
    {
        if (menuSonido != null)
            menuSonido.SetActive(false); // arranca oculto
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
}