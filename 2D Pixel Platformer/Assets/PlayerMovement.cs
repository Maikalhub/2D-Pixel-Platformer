using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;
    private bool isFacingRight = true;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    private int jumpRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool wasGrounded; // Для отслеживания приземления

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask wallLayer;
    private bool isWallSliding;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.5f;
    private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSliding();
        ProcessWallJump();
        Flip();

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }

        if (animator != null)
        {
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
            animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
            animator.SetBool("isWallSliding", isWallSliding);
        }

        // Проверка приземления
        if (!wasGrounded && isGrounded) 
        {
            SoundEffectManager.Play("PlayerLand");
        }

        wasGrounded = isGrounded;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isWallSliding)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = wallJumpTime;
            Flip();
            Invoke(nameof(CancelWallJump), wallJumpTime);
            Debug.Log("Wall Jump!");
            SoundEffectManager.Play("PlayerJump"); // Звук прыжка при отталкивании от стены
            return;
        }

        if (context.performed && jumpRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpRemaining--;
            Debug.Log("Jump triggered");
            SoundEffectManager.Play("PlayerJump"); // Звук прыжка

            if (animator != null)
            {
                animator.SetTrigger("jump");
            }
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        if (isGrounded)
        {
            jumpRemaining = maxJumps;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSliding()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
