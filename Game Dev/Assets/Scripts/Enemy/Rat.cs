using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [SerializeField] private int hp = 10;
    [SerializeField] private Transform player;
    private Rigidbody2D rb;

    //to be implemented in the next sprint
    //[SerializeField] private int damage = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            // Get damage per hit from player
            GameObject player = GameObject.Find("Player");
            int damage = player.GetComponent<PlayerScript>().getDamagePerHit();
            hp -= damage;

            if (hp <= 0)
            {
                //Destroy(gameObject);
                Destroy(transform.parent.gameObject);
            }
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            if(transform.position.y <= player.position.y)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Get damage per hit from player
            GameObject player = GameObject.Find("Player");
            int damage = player.GetComponent<PlayerScript>().getDamagePerHit();
            hp -= damage;

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.CompareTag("Mushroom"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.CompareTag("Powerup"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
