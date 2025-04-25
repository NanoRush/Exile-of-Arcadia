using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    private float moveSpeed = 8f;
    [SerializeField] float jumpPower;

    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;

    private int jumpCount = 0;

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

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        }

        if (wallJumping)
        {
            rb.velocity = new Vector2(-horizontal * wallJumpForce.x, wallJumpForce.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
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
        rb.velocity = daggerVelocity;
    }

}
