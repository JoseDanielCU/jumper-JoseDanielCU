using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 10f;

    // Ground Check variables
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    public float groundCheckRadius = 0.2f;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    public int extraJumps = 1;
    private int extraJumpsValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps; // Set initial jumps
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Coyote Time & Double Jump Reset ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps; // Reset double jumps
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffering Logic ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // --- COMBINED Jump Input Check ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f) // Priority 1: Ground Jump (uses coyote time)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f; // Consume coyote time
                jumpBufferCounter = 0f; // Consume buffer
            }
            else if (extraJumpsValue > 0) // Priority 2: Air Jump
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // You could use a different jumpForce here
                extraJumpsValue--; // Consume an air jump
                jumpBufferCounter = 0f; // Consume buffer
            }
        }
    }




    // Helper function to visualize the ground check radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
