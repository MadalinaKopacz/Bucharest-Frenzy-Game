using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class dogMove : MonoBehaviour
{

    [SerializeField] private int hp = 20;

    [SerializeField] private List<Transform> points;
    [SerializeField] private int nextID = 0;
    private int idChangeValue = 1;

    private float initialPlayerPositionY;
    [SerializeField] private Transform player;

    [SerializeField] private float agroRange;
    [SerializeField] private float agroDash;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 4f;
    private float dashingTime = 3f;
    private float dashingCooldown = 2f;



    [SerializeField] private float moveSpeed;
    [SerializeField] private int scale = 4;



    private Rigidbody2D rb;

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        GameObject root = new GameObject(name + "_Root");

        root.transform.position = transform.position;
        transform.SetParent(root.transform);

        GameObject waypoints = new GameObject("Waypoints");
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform); p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform); p2.transform.position = root.transform.position;

        points = new List<Transform> { p1.transform, p2.transform };
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
        Physics2D.IgnoreLayerCollision(6, 8);
        print(hp);
        rb = GetComponent<Rigidbody2D>();
        initialPlayerPositionY = player.position.y;
    }

    private void Update()
    {

        if(isDashing)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        //print(distanceToPlayer);


        if (distanceToPlayer < agroRange)
        {
            //print('2');
            ChasePlayer();
        }
        else if(distanceToPlayer > agroRange && distanceToPlayer < agroDash)
        {
            StartCoroutine(Dash());
        }
        else
        {
            ChasePlayer();
        }
        /*if (distanceToPlayer < agroRange && player.position.y > initialPlayerPositionY + 0.1 && Mathf.Abs(player.position.x - transform.position.x) < 1.5)
        {
            print("1");
            MoveToNextPoint();
        }
        else if (distanceToPlayer < agroRange)
        {
            print('2');
            ChasePlayer();
        }
        else
        {
            //print("3");
            MoveToNextPoint();
        }*/
    }

    private void MoveToNextPoint()
    {
        Transform goalPoint = points[nextID];
        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = scale * new Vector2(-1, 1);
        else
            transform.localScale = scale * new Vector2(1, 1);

        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.2f)
        {
            if (nextID == points.Count - 1)
                idChangeValue = -1;
            if (nextID == 0)
                idChangeValue = 1;
            nextID += idChangeValue;
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = scale * new Vector2(-1, 1);
        }
        else if (transform.position.x > player.position.x)
        {
            rb.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = scale * new Vector2(1, 1);
        }
    }

    private IEnumerator Dash()
    {
        float playerPosX = player.position.x;
        //print(playerPosX);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        if (transform.position.x > playerPosX)
        {
            rb.velocity = new Vector2(playerPosX - dashingPower, 0f);
        }
        else
        {
            rb.velocity = new Vector2(playerPosX + dashingPower, 0f);

        }
        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;


    }


   /* void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            print(hp);
            // Get damage per hit from player
            GameObject player = GameObject.Find("Player");
            int damage = player.GetComponent<PlayerScript>().damagePerHit;
            hp -= damage;
            print("ghe");
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }

    } */

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            // Get damage per hit from player
            GameObject player = GameObject.Find("Player");
            int damage = player.GetComponent<PlayerScript>().damagePerHit;
            hp -= damage;

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}