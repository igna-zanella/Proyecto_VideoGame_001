using UnityEngine;

public class EnemigoControlador : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float distanciaAtaque = 5f;
    private Transform jugadorTransform;
    [SerializeField] private float fuerzaEmpujeX = 5f;
    [SerializeField] private float fuerzaEmpujeY = 2.5f;
    [SerializeField] private float velocidadX = 5f;
    private Animator animatorEnemigo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorEnemigo = GetComponent<Animator>();
        //busca al jugador
        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        float direccion = 0f;
        //verifica si existe el jugador y esta dentro de la distancia para atacar
        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) < distanciaAtaque)
        {
            Debug.Log("ATACAR");
            //-1,0,1
            direccion = Mathf.Sign(jugadorTransform.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(velocidadX * direccion, rb.linearVelocityY);
            if (direccion != 0)
            {
                //actualiza la direccion del sprite enemigo hacia el jugador
                transform.localScale = new Vector3(-direccion, 1, 1);
            }
        }
        animatorEnemigo.SetFloat("movimientoEnemigo", direccion);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        float direccionEmpuje = 0f;
        Vector2 fuerzaEmpuje;
        if (collision.gameObject.CompareTag("Player"))
        {
            //-1,0,1
            direccionEmpuje = Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x);
            //genera una fuerza de empuje y hacia arriba para el jugador al impactar
            fuerzaEmpuje = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);
            //usa el comportamiento serAtacado del script MovimientoJugador
            collision.gameObject.GetComponent<MovimientoJugador>().serAtacado(fuerzaEmpuje);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        //Muestra el rango de distancia para el ataque con el jugador
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //destruye el objeto Enemigo que detecta la colision
            Destroy(gameObject);
        }
    }
}