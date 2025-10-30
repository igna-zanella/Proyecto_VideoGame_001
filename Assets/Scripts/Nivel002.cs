using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDeNivel : MonoBehaviour
{
    [Header("Nivel_002")]
    [SerializeField] private string siguienteNivel = "Nivel2";

    [Header("Efecto opcional")]
    [SerializeField] private float retrasoAntesDeCargar = 1f;

    private bool jugadorEntró = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jugadorEntró) return; // evita múltiples disparos

        if (other.CompareTag("Player"))
        {
            jugadorEntró = true;
            StartCoroutine(CargarSiguienteNivel());
        }
    }

    private System.Collections.IEnumerator CargarSiguienteNivel()
    {
        // Pequeña pausa antes del cambio de escena
        yield return new WaitForSeconds(retrasoAntesDeCargar);

        // Cargar la siguiente escena
        SceneManager.LoadScene("Nivel_002");
    }
}
