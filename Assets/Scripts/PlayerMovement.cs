using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    public float maxSpeed;

    private Vector2 lastVelocity;
    private bool isGrounded;
    private Vector2 move;
    private Rigidbody2D rb;

    public Transform wallCheck;
    private bool onWall;
    public float wallSlideSpeed;
    public LayerMask wallLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(move * speed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && IsOnWall() == false)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        else
        {
            move = Vector2.zero;
        }

        lastVelocity = rb.velocity;

        //Debug.Log(IsOnWall());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = new Vector2(lastVelocity.x, 0f);
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private bool IsOnWall()
    {
        if (Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}