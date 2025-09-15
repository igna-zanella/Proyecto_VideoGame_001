using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
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
    
    public void ActivarConfigSonido()
    {
       //SceneManager.UnloadSceneAsync("Menu");
       //SceneManager.LoadScene("MenuSonido", LoadSceneMode.Additive);
    }
    
}
