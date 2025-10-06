
using UnityEngine;
using UnityEngine.UI;

public class MovimientoJugador : MonoBehaviour
{
    Rigidbody2D rb;
    bool isGrounded;
    Animator animationPlayer;
    public Slider VidaUIControlador;


    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Vida")]
    public int vidas = 10;
    public VidaUIControlador controladorVida;

    private bool bajoAtaque = false;

    public int getVida()
    {
        return vidas;
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationPlayer = GetComponent<Animator>();

        // Inicializar barra de vida en la UI
        if (controladorVida != null)
        {
            controladorVida.ConfigurarVidaTotal(vidas);
        }
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (!bajoAtaque)
        {
            // Salto
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
                SoundFXController.Instance.JugadorSalto(transform);
            }
            // Movimiento lateral con flip
            else if (Input.GetAxis("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") != 0)
            {
                rb.linearVelocity = new Vector2(5f * Input.GetAxis("Horizontal"), rb.linearVelocity.y);
                transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            }
        }

        // Animaciones
        animationPlayer.SetFloat("movimiento", Mathf.Abs(Input.GetAxis("Horizontal")));
        animationPlayer.SetBool("estaSuelo", isGrounded);

        VidaUIControlador.GetComponent<Slider>().value = vidas;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            bajoAtaque = false;
        }
    }

    public void serAtacado(Vector2 empuje)
    {
        bajoAtaque = true;
        rb.linearVelocity = empuje;
        vidas--;

        //if (vidas <= 0)
        //{
        //    Destroy(gameObject);
        //}

        //if (controladorVida != null)
        //{
        //    controladorVida.ActualizarVida(vidas);
        //}
        if (vidas <= 0)
        {
            vidas = 0;

            GameUIController ui = FindFirstObjectByType<GameUIController>();
            if (ui != null)
            {
                ui.JugadorMurio();
            }
            else
            {
                Destroy(gameObject); // fallback
            }
        }

    }
    public void ReiniciarEnergia()
    {
        vidas = controladorVida.getVidaTotal();
        if (controladorVida != null)
        {
            controladorVida.ActualizarVida(vidas);
        }
    }

    //public void RecuperarVida(int cantidad)
    //{
    //    vidas += cantidad;

    //    // Evitar que la vida supere el máximo
    //    if (vidas > controladorVida.getVidaTotal())
    //    {
    //        vidas = controladorVida.getVidaTotal();
    //    }

    //    if (controladorVida != null)
    //    {
    //        controladorVida.ActualizarVida(vidas);
    //    }
    //}

    public void RecibirCura(int cantidad)
    {
        // Solo curar si no está ya en el máximo
        if (vidas < controladorVida.getVidaTotal())
        {
            vidas += cantidad;

            if (vidas > controladorVida.getVidaTotal())
            {
                vidas = controladorVida.getVidaTotal();
            }

            if (controladorVida != null)
            {
                controladorVida.ActualizarVida(vidas);
            }
        }
    }
    public void MorirEnLava()
    {
        this.enabled = false; // Desactivar controles

        EfectoMuerteLava efecto = GetComponent<EfectoMuerteLava>();
        if (efecto != null)
        {
            StartCoroutine(efecto.Quemarse(() =>
            {
                NotificarMuerte();
                this.enabled = true; // Reactivar controles tras respawn
            }));
        }
        else
        {
            // Si no hay efecto, al menos notificar muerte directo
            NotificarMuerte();
            this.enabled = true;
        }
    }

    private void NotificarMuerte()
    {
        GameUIController ui = FindFirstObjectByType<GameUIController>();
        if (ui != null)
        {
            ui.JugadorMurio();
        }

        // Reactivar controles al respawnear
        this.enabled = true;
    }

}