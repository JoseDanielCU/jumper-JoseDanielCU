using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Jump Logic")]
    private bool isGrounded;
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    public int extraJumps = 1;
    private int extraJumpsValue;

    [Header("Referencias")]
    public Animator animator;
    public float freeFallThreshold = -0.1f; // umbral para considerar ca�da

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Actualizar Animator: grounded
        animator.SetBool("Grounded", isGrounded);

        // --- Coyote Time & Reset ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffer ---
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- L�gica de salto ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f) // Salto normal
            {
                Jump();
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumpsValue > 0) // Doble salto
            {
                Jump();
                extraJumpsValue--;
                jumpBufferCounter = 0f;
            }
        }

        // Actualizar animaciones de aire
        bool isFalling = rb.linearVelocity.y < freeFallThreshold;
        animator.SetBool("FreeFall", isFalling);

        // Si sube o inicia salto, podr�as usar Jump (trigger ya activado en Jump() )
        // Actualizar MotionSpeed por si usas MotionSpeed tambi�n aqu�
        animator.SetFloat("MotionSpeed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // Disparar trigger de salto (si tu par�metro Jump es trigger)
        animator.SetTrigger("Jump");
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
