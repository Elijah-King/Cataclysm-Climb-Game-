using UnityEngine;
using UnityEngine.InputSystem;

public class LadderScript : MonoBehaviour
{
    public float climbSpeed = 3f;

    private bool playerOnLadder = false;
    private Rigidbody2D playerRb;
    private PlayerInput playerInput;

    private float verticalInput;
    private float lockedX;

    public BoxCollider2D ladderTrigger;
    public Animator anim;

    [Header("Ladder Entry Detection")]
    public Vector2 entrySize = new Vector2(0.5f, 2f);
    public Vector2 entryOffset = Vector2.zero;
    public LayerMask playerLayer;

    private void Start()
    {
        ladderTrigger.enabled = false;

        if (anim == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                anim = player.GetComponent<Animator>();
        }
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapBox(
            (Vector2)transform.position + entryOffset,
            entrySize,
            0f,
            playerLayer
        );

        ladderTrigger.enabled = (hit != null);

        if (playerOnLadder && hit == null)
        {
            playerOnLadder = false;
            anim.SetBool("isClimbing", false);

            if (playerRb != null)
                playerRb.gravityScale = 1f;

            return;
        }

        if (playerOnLadder)
        {
            Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            verticalInput = moveInput.y;

            playerRb.position = new Vector2(lockedX, playerRb.position.y);

            playerRb.gravityScale = 0f;

            playerRb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);

            bool isMoving = Mathf.Abs(verticalInput) > 0.1f;

            anim.SetBool("isClimbing", isMoving);

            Debug.Log("Trigger fired");
            Debug.Log("Animator reference: " + anim);
            Debug.Log("Setting isClimbing = " + isMoving);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnLadder = true;

            playerRb = collision.GetComponent<Rigidbody2D>();
            playerInput = collision.GetComponent<PlayerInput>();

            lockedX = playerRb.position.x;

            collision.GetComponent<PlayerController>().isOnLadder = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnLadder = false;

            if (playerRb != null)
                playerRb.gravityScale = 1f;

            anim.SetBool("isClimbing", false);


            collision.GetComponent<PlayerController>().isOnLadder = false;


        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + entryOffset, entrySize);
    }
}








