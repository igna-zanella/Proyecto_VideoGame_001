using UnityEngine;
using System.Collections;

public class EnemigoControlador2 : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float distanciaAtaque = 5f;
    private Transform jugadorTransform;
    [SerializeField] private float fuerzaEmpujeX = 5f;
    [SerializeField] private float fuerzaEmpujeY = 2.5f;
    [SerializeField] private float velocidadX = 5f;
    private Animator animatorEnemigo;
    private SpriteRenderer spriteRenderer;

    [Header("Ataque a distancia")]
    [SerializeField] private GameObject prefabBola;
    [SerializeField] private float velocidadBola = 5f;
    [SerializeField] private float bolasPorSegundo = 1f;
    [SerializeField] private float rangoDisparo = 8f;

    private float tiempoEntreDisparos;
    private float proximoDisparo;


    private int golpesRecibidos = 0;
    private bool estaParpadeando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorEnemigo = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;

        tiempoEntreDisparos = 1f / bolasPorSegundo;
        proximoDisparo = Time.time + tiempoEntreDisparos;
    }

    void FixedUpdate()
    {
        if (golpesRecibidos > 0) return; // mientras está herido, no se mueve

        float direccion = 0f;

        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) < distanciaAtaque)
        {
            direccion = Mathf.Sign(jugadorTransform.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(velocidadX * direccion, rb.linearVelocityY);

            if (direccion != 0)
                transform.localScale = new Vector3(-direccion, 1, 1);
        }

        if (animatorEnemigo)
            animatorEnemigo.SetFloat("movimientoEnemigo", Mathf.Abs(rb.linearVelocity.x));

        // --- Disparo de proyectil ---
        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) <= rangoDisparo)
        {
            if (Time.time >= proximoDisparo)
            {
                DispararBola();
                proximoDisparo = Time.time + tiempoEntreDisparos;
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verificamos si el jugador viene desde arriba
            float alturaJugador = collision.transform.position.y;
            float alturaEnemigo = transform.position.y;

            // Si el jugador está más alto que el enemigo → no recibe daño
            if (alturaJugador > alturaEnemigo + 0.2f) return;

            // Si no, aplica daño
            float direccionEmpuje = Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x);
            Vector2 fuerzaEmpuje = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);
            collision.gameObject.GetComponent<MovimientoJugador>().serAtacado(fuerzaEmpuje);
        }
    }

    // Detecta el golpe en la cabeza del enemigo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            golpesRecibidos++;

            if (golpesRecibidos == 1)
            {
                // Primer golpe: parpadea y se empuja hacia atrás
                if (!estaParpadeando)
                    StartCoroutine(ReaccionarAlGolpe());
            }
            else if (golpesRecibidos >= 2)
            {
                Destroy(gameObject); // Segundo golpe: muerte
            }
        }
    }

    private IEnumerator ReaccionarAlGolpe()
    {
        estaParpadeando = true;

        // retroceso físico
        float direccionEmpuje = jugadorTransform != null
            ? Mathf.Sign(transform.position.x - jugadorTransform.position.x)
            : 1f;
        rb.linearVelocity = new Vector2(direccionEmpuje * 4f, 2f);

        // buscamos el material (es independiente del Animator)
        Material material = spriteRenderer.material;
        Color colorOriginal = material.color;

        float duracion = 2f;
        float tiempo = 0f;
        bool encendido = false;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            encendido = !encendido;
            material.color = encendido ? new Color(1f, 0.3f, 0.3f, 0.4f) : colorOriginal;
            yield return new WaitForSeconds(0.35f);
        }

        material.color = colorOriginal;
        estaParpadeando = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }

    private void DispararBola()
    {
        if (prefabBola == null || jugadorTransform == null) return;

        // Crear instancia del proyectil
        GameObject bola = Instantiate(prefabBola, transform.position, Quaternion.identity);

        // Calcular dirección hacia el jugador
        Vector2 direccion = jugadorTransform.position - transform.position;

        // Configurar el proyectil
        BolaEnemigo scriptBola = bola.GetComponent<BolaEnemigo>();
        if (scriptBola != null)
        {
            scriptBola.Configurar(direccion, velocidadBola);
        }

        // Opcional: rotar el enemigo hacia el jugador
        transform.localScale = new Vector3(Mathf.Sign(direccion.x) * -1f, 1, 1);
    }


}
