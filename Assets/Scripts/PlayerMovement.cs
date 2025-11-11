using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;
    private float moveInput;
    private bool facingRight = true;

    [Header("Referencias")]
    public Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer entrada horizontal (A/D o ←/→)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Actualizar parámetro "Speed" en el Animator (valor del input)
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Actualizar "MotionSpeed" usando la velocidad real horizontal (unidad: unidades/seg)
        animator.SetFloat("MotionSpeed", Mathf.Abs(rb.linearVelocity.x));

        // Voltear sprite si cambia de dirección
        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();
    }

    void FixedUpdate()
    {
        // Aplicar movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
