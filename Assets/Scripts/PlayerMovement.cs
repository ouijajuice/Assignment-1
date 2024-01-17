using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    private float lastDirectionMoved;

    public float speed;
    public float jumpForce;
    public float maxSpeed;
    public float wallMaxSpeed;

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
        //adding force to rb in fixed so its not frame dependant i think idk
        rb.AddForce(move * speed * Time.deltaTime);

        //slowing the velocity of the rb when on a wall
        //this shit kinda sucks probably should change later
        if (IsOnWall() == true)
        {
            rb.velocity = new Vector2(0,rb.velocity.y * wallMaxSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if space press and on ground and not on wall then add impulse up on rb
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && IsOnWall() == false)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
        //if x velocity is less than the max speed then accept player input
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            
        }
        
        //if not, ignore player input
        //probably a better way to do this but it works
        else
        {
            move = Vector2.zero;
        }

        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            lastDirectionMoved = Input.GetAxisRaw("Horizontal");
        }

        if (IsFacingRight() == false)
        {
            this.gameObject.transform.localScale = new Vector2(-0.5f, transform.localScale.y);
        }
        if (IsFacingRight() == true)
        {
            this.gameObject.transform.localScale = new Vector2(0.5f, transform.localScale.y);
        }

        lastVelocity = rb.velocity;

        //Debug.Log(IsOnWall());
        //Debug.Log(IsFacingRight());
        //Debug.Log(lastKeyDown);

        animator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ground detection
        if (collision.gameObject.CompareTag("Ground"))
        {
            //this velocity line is so the player doesn't lose all momentum upon landing
            //i could just give everything 0 friction but i feel like this is better long term
            rb.velocity = new Vector2(lastVelocity.x, 0f);

            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //detect is player is leaving ground
        //i forgot why i did this like this but whatever
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private bool IsOnWall()
    {
        //detect if a wall is within range of the wall detection transform
        if (Physics2D.OverlapCircle(wallCheck.position, 0.05f, wallLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsFacingRight()
    {
        if (lastDirectionMoved < 0)
        {
            return false;
        }
        if (lastDirectionMoved > 0)
        {
            return true;
        }
        else
        {
            return true;
        }
    }
}