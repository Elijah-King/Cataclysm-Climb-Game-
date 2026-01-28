using UnityEngine;

public class ObjectLadderRoll : MonoBehaviour
{
    [Header("Movement")]
    public float rollSpeed = 2f;
    public float fallSpeed = 3f; // positive number, code forces downward

    [Header("Ground Check")]
    public Transform ObjectGroundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D col;

    private float direction = 1f; // 1 = right, -1 = left

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        // Check if touching ground
        bool grounded = Physics2D.OverlapCircle(
            ObjectGroundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (grounded)
        {
            // ON GROUND  normal rolling
            col.enabled = true;
            rb.gravityScale = 1f;

            rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
        }
        else
        {
            // NOT ON GROUND falling mode
            col.enabled = false;
            rb.gravityScale = 0f;

            rb.linearVelocity = new Vector2(0, -Mathf.Abs(fallSpeed));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Reverse direction when hitting platform edge
        if (other.CompareTag("platEdge"))
        {
            direction *= -1f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (ObjectGroundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ObjectGroundCheck.position, groundCheckRadius);
        }
    }
}
































