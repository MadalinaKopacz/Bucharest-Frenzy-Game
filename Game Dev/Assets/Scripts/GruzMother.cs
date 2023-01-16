using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GruzMother : MonoBehaviour
{
    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    [SerializeField] private int hp = 50;
    [SerializeField] private Transform player2;
    private AudioSource finishSound;


    void Start()
    {
        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);; 
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);
    }

    void attackPlayer(){
        enemyAnim.SetTrigger("Slamed");
        enemyRB.velocity = Vector2.zero;
        hasPlayerPositon = false;
    }

    private IEnumerator ChangeScene(float numOfSeconds)
    {
        Time.timeScale = 0;
        //finishSound.Play();
        yield return new WaitForSecondsRealtime(numOfSeconds);
        Time.timeScale = 1;
        SceneManager.LoadScene("EndOfTheGame");
    }

    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

   public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    } 
   public void AttackUpNDownState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
            playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }
        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
           
        }
        

        if (isTouchingWall || isTouchingDown)
        {
            attackPlayer();
        }
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
       Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWall.position, groundCheckRadius);
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.CompareTag("Powerup"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeDirection();
        }

    }
    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Bullet"))
        {
            // Get damage per hit from player
            GameObject player2 = GameObject.Find("Player");
            int damage = player2.GetComponent<PlayerScript>().getDamagePerHit();
            hp -= damage;

            if (hp <= 0)
            {
                Destroy(gameObject);
                //Time.timeScale = 0;
                //yield return new WaitForSecondsRealtime(1f);
                //Time.timeScale = 1;
                SceneManager.LoadScene("EndOfTheGame");
            }
        }
    }

}
