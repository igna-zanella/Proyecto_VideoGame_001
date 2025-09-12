using System;
using UnityEngine;

public class DanioAgua : MonoBehaviour
{

    private float damage;
    private float attackspeed;
    private float attacktimer;
    private bool isAttacking;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        attacktimer = attackspeed;
    }

    private void Update()
    {
        if (isAttacking)
        {
            attacktimer -= Time.deltaTime;
            if (attacktimer <= 0)
            {
                player.transform.SendMessage("takeDamage", damage);
                attacktimer = attackspeed;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player");
    }

}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   