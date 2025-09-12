using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Daño al jugador")]
    public int danio = 1;
    public Vector2 empuje = new Vector2(-3f, 2f);

    [Header("Salto del jugador al matar")]
    public float reboteAlMatar = 6f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            // Obtener el punto de contacto
            ContactPoint2D contacto = collision.contacts[0];
            Vector2 normal = contacto.normal;

            // Si la normal apunta hacia arriba => el jugador cayó desde arriba
            if (normal.y > 0.5f)
            {
                // Jugador mata al enemigo
                Destroy(gameObject);

                // Hacer rebotar al jugador hacia arriba
                Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, reboteAlMatar);
            }
            else
            {
                // El jugador recibe daño
                Vector2 direccionEmpuje = new Vector2(
                    Mathf.Sign(jugador.transform.position.x - transform.position.x) * empuje.x,
                    empuje.y
                );

                jugador.serAtacado(direccionEmpuje);
            }
        }
    }
}