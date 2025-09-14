using System;
using UnityEngine;

public class DanioAgua : MonoBehaviour
{

    //public float damage;
    //public float attackspeed;
    //private float attacktimer;
    //private bool isAttacking;
    //GameObject player;

    //private void Start()
    //{
    //    player = GameObject.Find("Player");
    //    attacktimer = attackspeed;
    //}

    //private void Update()
    //{
    //    if (isAttacking)
    //    {
    //        attacktimer -= Time.deltaTime;
    //        if (attacktimer <= 0)
    //        {
    //            player.transform.SendMessage("takeDamage", damage);
    //            attacktimer = attackspeed;
    //        }
    //    }

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        isAttacking = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        isAttacking = false;
    //    }
    //}
    [Header("Daño por agua")]
    public int danioPorTick = 5;      // cuánto daño hace cada vez
    public float intervaloDanio = 1f; // cada cuántos segundos aplica el daño

    private bool enAgua = false;
    private float temporizador = 0f;
    private MovimientoJugador jugadorDentro = null;

    void Update()
    {
        if (enAgua && jugadorDentro != null)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= intervaloDanio)
            {
                jugadorDentro.serAtacado(Vector2.zero); // empuje nulo
                temporizador = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimientoJugador jugador = collision.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            enAgua = true;
            jugadorDentro = jugador;
            temporizador = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<MovimientoJugador>() != null)
        {
            enAgua = false;
            jugadorDentro = null;
            temporizador = 0f;
        }
    }
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   