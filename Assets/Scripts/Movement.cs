using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 16f;
    private bool isFacingRight = true;

    [Header("References")]
    [SerializeField] private Handleanimations animationHandler;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isCrouching;
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float wallHangTime = 1.5f;
    private float wallHangCounter;
    private bool wallTimeUsed = false;
    [SerializeField] private float wallSlipSpeed = 20f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Dashing")]
    [SerializeField] private float dashingTime = 0.5f;
    [SerializeField] private int maxDashes = 2;
    private int dashesLeft;
    private Vector2 dashingDir;
    private bool isDashing;
    [SerializeField] private float groundDashVelocity = 14f;
    [SerializeField] private float airDashVelocity = 8f;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Jump Timing")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    private void Start()
    {
        dashesLeft = maxDashes;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animationHandler.SetWalking(horizontal != 0f);
        isCrouching = Input.GetKey(KeyCode.S) && IsGrounded();
        animationHandler.SetCrouching(isCrouching);

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            animationHandler.SetFalling(false);
        }
        else
        {
            animationHandler.SetFalling(true);
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            animationHandler.SetJumping(true);
        }

        if (!isDashing)
        {
            HandleCoyoteJump();
            WallSlide();
            WallJump();
        }

        HandleDashInput();
        FlipIfNeeded(); // 👈 importante
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        FlipIfNeeded();
        if (!isWallJumping)
        {
            if (!isCrouching)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    private void HandleCoyoteJump()
    {
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (IsGrounded() && animationHandler != null)
        {
            animationHandler.SetJumping(false);
        }
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void HandleDashInput()
    {
        if (Input.GetButtonDown("Dash") && dashesLeft > 0)
        {
            isDashing = true;
            trailRenderer.emitting = true;
            dashesLeft--;

            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dashingDir == Vector2.zero)
                dashingDir = new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);

            StartCoroutine(StopDashing());

            float dashSpeed = IsGrounded() ? groundDashVelocity : airDashVelocity;
            rb.velocity = dashingDir.normalized * dashSpeed;
            rb.gravityScale = 0f;
        }

        if (IsGrounded())
        {
            dashesLeft = maxDashes;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
        rb.gravityScale = 3f;

        if (!IsGrounded())
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f && !wallTimeUsed)
        {
            wallHangCounter -= Time.deltaTime;

            if (wallHangCounter > 0f)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, -wallSlipSpeed);
                wallTimeUsed = true;
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        if (IsGrounded())
        {
            wallTimeUsed = false;
            wallHangCounter = wallHangTime;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void FlipIfNeeded()
{
    if (horizontal > 0)
    {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            spriteRenderer.flipX = false;
        isFacingRight = true;
    }
    else if (horizontal < 0)
    {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            spriteRenderer.flipX = true;
        isFacingRight = false;
    }
}


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
}
