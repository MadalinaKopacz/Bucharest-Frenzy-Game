using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSize;
    [SerializeField] private float scale;

    private Rigidbody2D body;
    private Animator animator;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        float direction = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(direction * speed, body.velocity.y);

        // flip the player when moving left-right
        if (direction >= 0) 
            transform.localScale = scale * Vector3.one;
        else
            transform.localScale = scale * new Vector3(-1, 1, 1);

        if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
            Jump();

        // set the running animation
        animator.SetBool("isRunning", direction != 0);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSize);
        animator.SetTrigger("jump");
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
}
