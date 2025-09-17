using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float velPower;
    public float fallGravityMultiplier;
    [SerializeField] float jumpPower;

    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;

    public float teleportBoost;

    private int jumpCount = 0;

    private bool teleporting = false;

    //Wall Jump
    public Transform wallCheck;
    public LayerMask wallLayer;
    bool isWallTouch;
    bool isSliding;
    public float wallSlidingSpeed;
    public float wallJumpDuration = 0.1f;
    public Vector2 wallJumpForce;
    bool wallJumping;

    bool isFacingRight = true;

    private float gravityScale;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 15f);
        gravityScale = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        FlipSprite();

        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.28f, 0.48f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        isWallTouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.38f, 2.46f), 0, wallLayer);


        if (isGrounded)
        {
            jumpCount = 1;
            teleporting = false;
        }

        if(isWallTouch && !isGrounded && horizontal != 0f)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }


        if (Input.GetButtonDown("Jump"))
        {

            if (isSliding)
            {
                wallJumping = true;
                Invoke("StopWallJump", wallJumpDuration);
            }
            else if (jumpCount < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                //rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                jumpCount++;
            }
        }

    }

    private void FixedUpdate()
    {
        
        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            teleporting = false;
        }

        if (wallJumping)
        {
            rb.AddForce(new Vector2(-horizontal * wallJumpForce.x, wallJumpForce.y), ForceMode2D.Impulse);
        }
        else
        {
            /*
            else if (teleporting)
            {
                rb.AddForce(new Vector2(horizontal * moveSpeed * 2, rb.velocity.y));
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, 20f);
            }
            else
            {
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            }*/

            float targetSpeed = horizontal * moveSpeed;

            float speedDif = targetSpeed - rb.velocity.x;

            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    void StopWallJump()
    {
        wallJumping = false;
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            gameObject.transform.GetChild(2).gameObject.GetComponent<Cursor>().Flip();
        }
    }

    public void Teleport(Vector3 daggerLocation, Vector3 daggerVelocity)
    {
        transform.position = daggerLocation;
        rb.velocity = daggerVelocity * teleportBoost;
        teleporting = true;
        jumpCount = 1;
    }

}
