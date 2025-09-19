//using UnityEngine;

//public class MovimientoJugador : MonoBehaviour
//{
//    Rigidbody2D rb;
//    bool isGrounded;
//    Animator animationPlayer;
//    public float speed = 5f;
//    public float jumpForce = 7f;
//    private bool bajoAtaque = false;
//    private int vidas = 3;
//    private VidaUIControlador controladorVida = null;

//    public int getVida()
//    { 
//        return vidas; 
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        animationPlayer = GetComponent<Animator>();
//        controladorVida = GetComponentInChildren<VidaUIControlador>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float moveInput = Input.GetAxis("Horizontal");
//        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
//        if (!bajoAtaque)
//        {
//            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//            {
//                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//                isGrounded = false;
//            }
//            else if (Input.GetAxis("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") != 0)
//            {
//                rb.linearVelocityX = 5f * Input.GetAxis("Horizontal");

//                transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
//            }
//        }
//        animationPlayer.SetFloat("movimiento", Mathf.Abs(Input.GetAxis("Horizontal")));
//        animationPlayer.SetBool("estaSuelo", isGrounded);
//    }

//    // Detecta si el jugador esta tocando el suelo
//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Suelo")) // Asegarate de que el suelo tenga el tag "Ground"
//        {
//            isGrounded = true;
//            bajoAtaque = false;
//        }
//    }

//    public void serAtacado(Vector2 empuje)
//    {
//        bajoAtaque = true;
//        rb.linearVelocity = empuje;
//        vidas--;
//        if (vidas <= 0)
//        {
//            Destroy(gameObject);
//        }
//        controladorVida.ActualizarVida(vidas);
//    }
//}

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
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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

        if (vidas <= 0)
        {
            Destroy(gameObject);
        }

        if (controladorVida != null)
        {
            controladorVida.ActualizarVida(vidas);
        }
    }

    public void RecuperarVida(int cantidad)
    {
        vidas += cantidad;

        // Evitar que la vida supere el máximo
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