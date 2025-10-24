using UnityEngine;
using System.Collections;

public class PlataformaAscensor : MonoBehaviour
{
    [Header("Puntos de movimiento")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("Configuración de movimiento")]
    public float velocidad = 2f;
    public float tiempoDeEspera = 1f; // pausa en los extremos

    private bool jugadorSobrePlataforma = false;
    private bool moviendoArriba = false;
    private bool enMovimiento = false;

    void Update()
    {
        if (jugadorSobrePlataforma && !enMovimiento)
        {
            StartCoroutine(MoverAscensor());
        }
    }

    private IEnumerator MoverAscensor()
    {
        enMovimiento = true;

        // Primer tramo: subir
        Vector3 destino = puntoB.position;
        while (Vector3.Distance(transform.position, destino) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(tiempoDeEspera);

        // Segundo tramo: bajar
        destino = puntoA.position;
        while (Vector3.Distance(transform.position, destino) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(tiempoDeEspera);

        enMovimiento = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorSobrePlataforma = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorSobrePlataforma = false;
            collision.transform.SetParent(null);
        }
    }
}