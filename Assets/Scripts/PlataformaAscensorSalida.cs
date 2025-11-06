using UnityEngine;
using System.Collections;

public class PlataformaAscensorSalida : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Configuración de movimiento")]
    public float velocidad = 2f;
    public float tiempoDeEspera = 1f; // pausa en los extremos

    private bool enMovimiento = false;

    void Start()
    {
        // El ascensor comienza desactivado (su script está deshabilitado)
        // No necesitamos poner enabled = false aquí si ya se hace desde el editor
    }

    void OnEnable()
    {
        // Cuando se active, comenzar el movimiento
        if (!enMovimiento)
            StartCoroutine(MoverAscensor());
    }

    private IEnumerator MoverAscensor()
    {
        enMovimiento = true;

        while (true) // Movimiento en loop
        {
            // Subir
            yield return StartCoroutine(MoverHacia(puntoB.position));
            yield return new WaitForSeconds(tiempoDeEspera);

            // Bajar
            yield return StartCoroutine(MoverHacia(puntoA.position));
            yield return new WaitForSeconds(tiempoDeEspera);
        }
    }

    private IEnumerator MoverHacia(Vector3 destino)
    {
        while (Vector3.Distance(transform.position, destino) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
    }
}
