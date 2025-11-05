using UnityEngine;
using System.Collections;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator anim;
    private VidaUIControlador controladorVida;

    [Header("Movimiento terrestre")]
    public float speed = 5f;
    public float jumpForce = 7f;
    private bool isGrounded;
    private bool bajoAtaque = false;

    [Header("Vuelo")]
    public bool puedeVolar = false;
    public float velocidadVuelo = 5f;
    public float limiteAltura = 15f;

    [Header("Vida del jugador")]
    [SerializeField] private int vidaMaxima = 10;
    [SerializeField] private int vidaActual;

    private GameUIController gameController;
    private bool muriendoEnLava = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        controladorVida = FindFirstObjectByType<VidaUIControlador>();
        gameController = FindFirstObjectByType<GameUIController>();

        vidaActual = vidaMaxima;

        if (controladorVida != null)
            controladorVida.ActualizarVida(vidaActual);
    }

    void Update()
    {
        if (puedeVolar)
            ControlarVuelo();
        else
            ControlarMovimientoTerrestre();

        anim?.SetFloat("movimiento", Mathf.Abs(Input.GetAxis("Horizontal")));
        anim?.SetBool("estaSuelo", isGrounded);
    }

    // ---------------- MOVIMIENTO ----------------
    private void ControlarMovimientoTerrestre()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            SoundFXController.Instance?.JugadorSalto(transform);
        }

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
    }

    private void ControlarVuelo()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movimiento = new Vector2(moveX, moveY) * velocidadVuelo;
        rb.linearVelocity = movimiento;

        if (transform.position.y > limiteAltura)
            transform.position = new Vector3(transform.position.x, limiteAltura, transform.position.z);

        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
    }

    // ---------------- COLISIONES ----------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            bajoAtaque = false;
        }
    }

    // ---------------- DAÑO Y VIDA ----------------
    public void SerAtacado(Vector2 empuje)
    {
        if (bajoAtaque) return;

        bajoAtaque = true;
        rb.linearVelocity = empuje;
        vidaActual--;

        if (controladorVida != null)
            controladorVida.ActualizarVida(vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }

        Invoke(nameof(ResetAtaque), 0.4f);
    }

    private void ResetAtaque()
    {
        bajoAtaque = false;
    }

    private void Morir()
    {
        if (gameController != null)
            gameController.JugadorMurio();
        else
            Debug.LogWarning("[MovimientoJugador] GameUIController no encontrado.");
    }

    public void MorirEnLava()
    {
        // Desactivar controles mientras se ejecuta el efecto
        this.enabled = false;

        EfectoMuerteLava efecto = GetComponent<EfectoMuerteLava>();
        if (efecto != null)
        {
            StartCoroutine(efecto.Quemarse(() =>
            {
                // Cuando termina el efecto, notificar al Game Controller
                if (gameController != null)
                {
                    gameController.JugadorMurio();
                }
                else
                {
                    Debug.LogWarning("[MovimientoJugador] GameUIController no encontrado al morir en lava.");
                }

                // Reactivar controles después del respawn
                this.enabled = true;
            }));
        }
        else
        {
            // Si no existe el efecto, al menos notificar muerte directa
            if (gameController != null)
            {
                gameController.JugadorMurio();
            }

            this.enabled = true;
        }
    }



    public void RecibirCura(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMaxima);
        if (controladorVida != null)
            controladorVida.ActualizarVida(vidaActual);
    }

    public void ReiniciarEnergia()
    {
        vidaActual = vidaMaxima;
        if (controladorVida != null)
            controladorVida.ActualizarVida(vidaActual);

        rb.linearVelocity = Vector2.zero;
        bajoAtaque = false;

        // 🟢 Restaurar opacidad del sprite si fue desvanecido por EfectoMuerteLava
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }

    public int GetVida()
    {
        return vidaActual;
    }

    // ==========================================================
    // 🧩 BLOQUE DE COMPATIBILIDAD COMPLETO
    // ==========================================================

    // Alias antiguos usados por enemigos, trampas y otros scripts
    public void serAtacado(Vector2 empuje)
    {
        SerAtacado(empuje);
    }

    public void serAtacado()
    {
        SerAtacado(Vector2.zero);
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual <= 0)
            Morir();
        else if (controladorVida != null)
            controladorVida.ActualizarVida(vidaActual);
    }

    public void RecuperarEnergia(int cantidad)
    {
        RecibirCura(cantidad);
    }

    public void recuperarEnergia(int cantidad)
    {
        RecibirCura(cantidad);
    }

    public int getVida()
    {
        return GetVida();
    }

    public void MorirInstantaneamente()
    {
        Morir();
    }

    public void MorirInstantaneo()
    {
        Morir();
    }

    public void MorirEnAgua()
    {
        Morir();
    }

    public void MorirPorTrampa()
    {
        Morir();
    }

    public void RecibirCuraGradual(int cantidad)
    {
        RecibirCura(cantidad);
    }

    public void RecibirCuracion(int cantidad)
    {
        RecibirCura(cantidad);
    }

    // ------------------------------------------------------------
    // 🪽 Método llamado por AlasPickup.cs para permitir el vuelo
    // ------------------------------------------------------------
    public void ActivarVuelo()
    {
        puedeVolar = true;
        rb.gravityScale = 0f;

        // Opcional: reproducir sonido o animación de alas
        //SoundFXController.Instance?.ReproducirFX("AlasPickup");

        Debug.Log("[MovimientoJugador] Vuelo activado: el jugador ahora puede volar.");
    }


}
