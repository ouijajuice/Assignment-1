using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject dashParticles;
    private bool dashingBool;
    public LayerMask shooterKillerLayer;
    public GameObject shooterKillerPrefab;

    public Animator animator;
    private float lastDirectionMoved;
    private float slideCoolDown = 0f;
    public float slideCoolDownDuration;
    private bool isFacingRight = true;
    private float horizontal;

    public float speed;
    public float jumpForce;
    public float maxSpeed;
    public float wallMaxSpeed;
    public float slideForce;
    public float dashingTime;

    private Vector2 lastVelocity;
    private bool isGrounded;
    private Vector2 move;
    private Rigidbody2D rb;

    public Transform wallCheck;
    public Transform groundCheck;
    private bool onWall;
    public float wallSlideSpeed;
    public LayerMask wallLayer;
    public LayerMask groundLayer;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(20f, 20f);

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
        horizontal = Input.GetAxisRaw("Horizontal");

        //if space press and on ground and not on wall then add impulse up on rb
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && IsOnWall() == false)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //if x velocity is less than the max speed then accept player input
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            move = new Vector2(horizontal, 0f);
        }
        

        //if not, ignore player input
        //probably a better way to do this but it works
        else
        {
            move = Vector2.zero;
        }

        if (horizontal != 0f)
        {
            lastDirectionMoved = horizontal;
        }

        if (slideCoolDown <= 0f)
        {
            if (Input.GetAxis("Fire1") > 0f)
            {
                if (horizontal > 0f)
                {
                    dashingBool = true;
                    rb.AddForce(Vector2.right * slideForce, ForceMode2D.Impulse);
                    dashParticles.SetActive(true);
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    slideCoolDown = slideCoolDownDuration;
                    animator.SetBool("animDashing",true);
                    
                    Invoke("UnfreezeY", dashingTime);
                    Invoke("DespawnParticles", 0.5f);
                }
                else if (horizontal < 0f)
                {
                    dashingBool = true;
                    rb.AddForce(Vector2.left * slideForce, ForceMode2D.Impulse);
                    dashParticles.SetActive(true);
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    slideCoolDown = slideCoolDownDuration;
                    animator.SetBool("animDashing", true);
                    
                    Invoke("UnfreezeY", dashingTime);
                    Invoke("DespawnParticles", 0.5f);
                }
            }
        }
        else
        {
            
            slideCoolDown -= Time.deltaTime;
            animator.SetBool("animDashing", false);
        }
        if (slideCoolDown > 2f)
        {
            dashingBool = true;
        }
        else 
        { 
            dashingBool = false; 
        }


        Flip();
        

        if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            isGrounded = true;
        }

        if (isGrounded == false && rb.velocity.y > 0f)
        {
            animator.SetBool("animJumpUp", true);
        }
        if (isGrounded == false && rb.velocity.y < 0f && IsOnWall() == false)
        {
            animator.SetBool("animFalling", true);
        }

        WallJump();

        lastVelocity = rb.velocity;

        //Debug.Log(IsOnWall());
        //Debug.Log(IsFacingRight());
        //Debug.Log(lastKeyDown);
        Debug.Log(dashingBool);
        //Debug.Log(slideCoolDown);
        animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
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
            animator.SetBool("animJumpUp", false);
            animator.SetBool("animFalling", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shooter") && dashingBool == true)
        {
            Instantiate(shooterKillerPrefab, transform.position, Quaternion.identity);
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
            animator.SetFloat("Horizontal", 0f);
            animator.SetBool("animDashing", false);
            animator.SetBool("animJumpUp", false);
            animator.SetBool("animFalling", false);
            animator.SetBool("animOnWall", true);
            return true;
        }
        else
        {
            animator.SetBool("animOnWall", false);
            return false;
        }
    }

    private void WallJump()
    {
        if (IsOnWall())
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && horizontal == 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void UnfreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void DespawnParticles()
    {
        dashParticles.SetActive(false);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}