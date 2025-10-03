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

    public float teleportBoost;

    private int jumpCount = 0;

    private bool teleporting = false;

    //Audio and Visual
    public AudioClip teleportSound;
    public AudioClip jumpSound;
    public AudioClip wallJumpSound;
    private AudioSource source;

    public ParticleSystem jumpSmoke;
    public ParticleSystem wallJumpSmokeTrail;
    public ParticleSystem teleportTrail;

    //Wall Jump
    public Transform wallCheck;
    public LayerMask wallLayer;
    bool isSliding;
    public float wallSlidingSpeed;
    public float wallJumpDuration = 0.4f;
    public Vector2 wallJumpForce;
    bool wallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;

    bool isFacingRight = true;

    private float gravityScale;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 15f);
        gravityScale = rb.gravityScale;
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (!wallJumping)
        {
            FlipSprite();
        }


        if (isGrounded())
        {
            jumpCount = 1;
            teleporting = false;
        }

        if (Input.GetButtonDown("Jump") && !wallJumping && !isSliding)
        {

            if (jumpCount < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                //rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                source.PlayOneShot(jumpSound);
                jumpSmoke.Play();
                Invoke(nameof(StopTrail), 0.2f);

                jumpCount++;

            }
        }

        WallSlide();
        WallJump();

    }

    private void FixedUpdate()
    {

        if(!wallJumping){

            float currentVelX = rb.velocity.x;
            float targetSpeed = horizontal * moveSpeed;
            float accelRate;

            if (teleporting)
            {
                if (horizontal != 0 && Mathf.Sign(horizontal) != Mathf.Sign(currentVelX))
                {
                    // Opposite input → brake to 0 first
                    targetSpeed = 0f;
                    accelRate = decceleration * 0.2f; // strong braking

                    // Once nearly stopped, exit teleport state
                    if (Mathf.Abs(currentVelX) < 0.2f)
                    {
                        Debug.Log(currentVelX);
                        teleporting = false;
                    }
                }
                else
                {
                    // Same direction or no input → preserve teleport momentum
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

                    if (Mathf.Abs(currentVelX) > moveSpeed &&
                        Mathf.Sign(currentVelX) == Mathf.Sign(horizontal) &&
                        !isGrounded())
                    {
                        targetSpeed = currentVelX;
                    }
                }
            }
            else
            {
                // --- NORMAL MOVEMENT RULES ---
                if (Mathf.Abs(currentVelX) > moveSpeed &&
                    Mathf.Sign(currentVelX) == Mathf.Sign(horizontal) &&
                    !isGrounded())
                {
                    targetSpeed = currentVelX;
                }

                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            }

            // Apply force
            float speedDif = targetSpeed - currentVelX;
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
        wallJumpSmokeTrail.Stop();
    }

    private void WallJump()
    {
        if (isSliding)
        {
            wallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            wallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpForce.x, wallJumpForce.y);
            source.PlayOneShot(wallJumpSound);
            wallJumpSmokeTrail.Play();
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                gameObject.transform.GetChild(2).gameObject.GetComponent<Cursor>().Flip();
            }

            Invoke(nameof(StopWallJump), wallJumpDuration);
        }
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
        rb.velocity = daggerVelocity.normalized * teleportBoost;
        teleporting = true;
        jumpCount = 1;
        source.PlayOneShot(teleportSound);
        teleportTrail.Play();
        Invoke(nameof(stopTeleport), 0.5f);
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.28f, 0.48f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private bool isWallTouch()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.38f, 2.46f), 0, wallLayer);
    }

    private void stopTeleport()
    {
        teleporting = false;
        teleportTrail.Stop();
    }

    private void WallSlide()
    {
        if (isWallTouch() && !isGrounded() && horizontal != 0f)
        {
            isSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            teleporting = false;
        }
        else
        {
            isSliding = false;
        }
    }

    public void StopTrail()
    {
        jumpSmoke.Stop();
    }


}
