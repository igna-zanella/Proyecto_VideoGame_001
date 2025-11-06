//using UnityEngine;
//using System.Collections;

//public class Boss : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    [SerializeField] private float distanciaAtaque = 12.08f;
//    private Transform jugadorTransform;
//    [SerializeField] private float fuerzaEmpujeX = 5f;
//    [SerializeField] private float fuerzaEmpujeY = 2.5f;
//    [SerializeField] private float velocidadX = 2f;
//    private Animator Boss02;
//    private SpriteRenderer spriteRenderer;

//    [Header("Ataque a distancia")]
//    [SerializeField] private GameObject prefabBola;
//    [SerializeField] private float velocidadBola = 5f;
//    [SerializeField] private float bolasPorSegundo = 1f;
//    [SerializeField] private float rangoDisparo = 8f;

//    private float tiempoEntreDisparos;
//    private float proximoDisparo;


//    private int golpesRecibidos = 0;
//    private bool estaParpadeando = false;

//    private int golpesParaMorir = 8;
//    private float tiempoParpadeoBase = 0.25f;


//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        Boss02 = GetComponent<Animator>();
//        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
//        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;

//        tiempoEntreDisparos = 1f / bolasPorSegundo;
//        proximoDisparo = Time.time + tiempoEntreDisparos;
//    }

//    void FixedUpdate()
//    {
//        if (golpesRecibidos > 0) return; // mientras está herido, no se mueve

//        float direccion = 0f;

//        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) < distanciaAtaque)
//        {
//            direccion = Mathf.Sign(jugadorTransform.position.x - transform.position.x);
//            rb.linearVelocity = new Vector2(velocidadX * direccion, rb.linearVelocityY);

//            if (direccion != 0)
//                transform.localScale = new Vector3(-direccion, 1, 1);
//        }

//        if (Boss02)
//            Boss02.SetFloat("Boss02", Mathf.Abs(rb.linearVelocity.x));

//        // --- Disparo de proyectil ---
//        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) <= rangoDisparo)
//        {
//            if (Time.time >= proximoDisparo)
//            {
//                DispararBola();
//                proximoDisparo = Time.time + tiempoEntreDisparos;
//            }
//        }


//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            // Verificamos si el jugador viene desde arriba
//            float alturaJugador = collision.transform.position.y;
//            float alturaEnemigo = transform.position.y;

//            // Si el jugador está más alto que el enemigo → no recibe daño
//            if (alturaJugador > alturaEnemigo + 0.2f) return;

//            // Si no, aplica daño
//            float direccionEmpuje = Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x);
//            Vector2 fuerzaEmpuje = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);
//            collision.gameObject.GetComponent<MovimientoJugador>().serAtacado(fuerzaEmpuje);
//        }
//    }

//    // Detecta el golpe en la cabeza del enemigo
//    //private void OnTriggerEnter2D(Collider2D collision)
//    //{
//    //    if (collision.gameObject.CompareTag("Player"))
//    //    {
//    //        golpesRecibidos++;

//    //        if (golpesRecibidos == 1)
//    //        {
//    //            // Primer golpe: parpadea y se empuja hacia atrás
//    //            if (!estaParpadeando)
//    //                StartCoroutine(ReaccionarAlGolpe());
//    //        }
//    //        else if (golpesRecibidos >= 2)
//    //        {
//    //            Destroy(gameObject); // Segundo golpe: muerte
//    //        }
//    //    }
//    //}

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!collision.CompareTag("Player")) return;

//        golpesRecibidos++;

//        if (!estaParpadeando)
//            StartCoroutine(ReaccionarAlGolpe());

//        if (golpesRecibidos >= golpesParaMorir)
//        {
//            Morir();
//        }
//    }

//    //private IEnumerator ReaccionarAlGolpe()
//    //{
//    //    estaParpadeando = true;

//    //    // retroceso físico
//    //    float direccionEmpuje = jugadorTransform != null
//    //        ? Mathf.Sign(transform.position.x - jugadorTransform.position.x)
//    //        : 1f;
//    //    rb.linearVelocity = new Vector2(direccionEmpuje * 4f, 2f);

//    //    // buscamos el material (es independiente del Animator)
//    //    Material material = spriteRenderer.material;
//    //    Color colorOriginal = material.color;

//    //    float duracion = 2f;
//    //    float tiempo = 0f;
//    //    bool encendido = false;

//    //    while (tiempo < duracion)
//    //    {
//    //        tiempo += Time.deltaTime;
//    //        encendido = !encendido;
//    //        material.color = encendido ? new Color(1f, 0.3f, 0.3f, 0.4f) : colorOriginal;
//    //        yield return new WaitForSeconds(0.35f);
//    //    }

//    //    material.color = colorOriginal;
//    //    estaParpadeando = false;
//    //}

//    private IEnumerator ReaccionarAlGolpe()
//    {
//        estaParpadeando = true;
//        float duracion = Mathf.Clamp(1.5f + (golpesRecibidos * 0.3f), 1.5f, 4f);

//        Color colorOriginal = spriteRenderer.color;
//        float tiempo = 0f;
//        bool alternar = false;

//        while (tiempo < duracion)
//        {
//            tiempo += Time.deltaTime;
//            alternar = !alternar;
//            spriteRenderer.color = alternar ? new Color(1f, 0.3f, 0.3f) : colorOriginal;
//            yield return new WaitForSeconds(tiempoParpadeoBase);
//        }

//        spriteRenderer.color = colorOriginal;
//        estaParpadeando = false;
//    }



//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
//    }

//    private void DispararBola()
//    {
//        if (prefabBola == null || jugadorTransform == null) return;

//        // Crear instancia del proyectil
//        GameObject bola = Instantiate(prefabBola, transform.position, Quaternion.identity);

//        // Calcular dirección hacia el jugador
//        Vector2 direccion = jugadorTransform.position - transform.position;

//        // Configurar el proyectil
//        BolaBoss scriptBola = bola.GetComponent<BolaBoss>();
//        if (scriptBola != null)
//        {
//            scriptBola.Configurar(direccion, velocidadBola);
//        }

//        // Opcional: rotar el enemigo hacia el jugador
//        transform.localScale = new Vector3(Mathf.Sign(direccion.x) * -1f, 1, 1);
//    }

//    private void Morir()
//    {
//        Debug.Log("Boss derrotado 🐜💥");
//        Destroy(gameObject);

//        // Buscar el ascensor y activarlo
//        PlataformaAscensor02 ascensor = FindFirstObjectByType<PlataformaAscensor02>();
//        if (ascensor != null)
//        {
//            ascensor.enabled = true;
//        }
//    }

//}

using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float distanciaAtaque = 12f;
    private Transform jugadorTransform;
    [SerializeField] private float fuerzaEmpujeX = 5f;
    [SerializeField] private float fuerzaEmpujeY = 2.5f;
    [SerializeField] private float velocidadX = 2f;
    private Animator bossAnimator;
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

    [Header("Configuración de resistencia")]
    [SerializeField] private int golpesParaMorir = 8;
    private float tiempoParpadeoBase = 0.25f;

    private bool jefeDerrotado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;

        tiempoEntreDisparos = 1f / bolasPorSegundo;
        proximoDisparo = Time.time + tiempoEntreDisparos;
    }

    void FixedUpdate()
    {
        if (jefeDerrotado) return;

        // --- Movimiento básico hacia el jugador ---
        float direccion = 0f;
        if (jugadorTransform && Vector2.Distance(jugadorTransform.position, transform.position) < distanciaAtaque)
        {
            direccion = Mathf.Sign(jugadorTransform.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(velocidadX * direccion, rb.linearVelocity.y);
            if (direccion != 0)
                transform.localScale = new Vector3(-direccion, 1, 1);
        }

        //if (bossAnimator)
        //    bossAnimator.SetFloat("velocidad", Mathf.Abs(rb.linearVelocity.x));

        // --- Ataque a distancia ---
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
            float alturaJugador = collision.transform.position.y;
            float alturaBoss = transform.position.y;

            // Solo daña si el jugador no viene desde arriba
            if (alturaJugador <= alturaBoss + 0.2f)
            {
                float direccionEmpuje = Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x);
                Vector2 fuerzaEmpuje = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);
                collision.gameObject.GetComponent<MovimientoJugador>().serAtacado(fuerzaEmpuje);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (jefeDerrotado) return;

        golpesRecibidos++;

        if (!estaParpadeando)
            StartCoroutine(ReaccionarAlGolpe());

        // 🔥 Aumentar frecuencia de disparos a partir del sexto golpe
        if (golpesRecibidos == 6)
        {
            bolasPorSegundo *= 1.5f;
            tiempoEntreDisparos = 1f / bolasPorSegundo;
            Debug.Log("💥 El Boss se enfurece y dispara más rápido!");
        }

        // ☠️ Morir al octavo golpe
        if (golpesRecibidos >= golpesParaMorir)
        {
            Morir();
        }
    }

    private IEnumerator ReaccionarAlGolpe()
    {
        estaParpadeando = true;

        Color colorOriginal = spriteRenderer.color;
        float duracion = Mathf.Clamp(1.5f + (golpesRecibidos * 0.3f), 1.5f, 4f);
        float tiempo = 0f;
        bool alternar = false;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            alternar = !alternar;
            spriteRenderer.color = alternar ? new Color(1f, 0.2f, 0.2f) : colorOriginal;
            yield return new WaitForSeconds(tiempoParpadeoBase);
        }

        spriteRenderer.color = colorOriginal;
        estaParpadeando = false;
    }

    private void DispararBola()
    {
        if (prefabBola == null || jugadorTransform == null) return;

        GameObject bola = Instantiate(prefabBola, transform.position, Quaternion.identity);
        Vector2 direccion = (jugadorTransform.position - transform.position).normalized;

        BolaBoss scriptBola = bola.GetComponent<BolaBoss>();
        if (scriptBola != null)
            scriptBola.Configurar(direccion, velocidadBola);

        transform.localScale = new Vector3(Mathf.Sign(direccion.x) * -1f, 1, 1);
    }

    private void Morir()
    {
        jefeDerrotado = true;
        Debug.Log("Boss derrotado 🐜💥");

        // Buscar y activar la plataforma de salida
        PlataformaAscensor02 ascensor = FindFirstObjectByType<PlataformaAscensor02>();
        if (ascensor != null)
        {
            ascensor.enabled = true;
            Debug.Log("🚀 PlataformaAscensorSalida activada");
        }

        // Opcional: pequeña animación o retardo antes de destruir
        StartCoroutine(DestruirDespuesDe(1.2f));
    }

    private IEnumerator DestruirDespuesDe(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}

