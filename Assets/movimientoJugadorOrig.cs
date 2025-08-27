using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animatorPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
    }

    void Update()
    {
        //Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (moveInput * speed, rb.linearVelocity.y);
        animatorPlayer.SetFloat("movimiento", Mathf.Abs(Input.GetAxis("Horizontal")));
        
        if (Input.GetAxisRaw("Horizontal") != 0)
        { 
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        animatorPlayer.SetBool("estaSuelo", isGrounded);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
        }
    }
}
