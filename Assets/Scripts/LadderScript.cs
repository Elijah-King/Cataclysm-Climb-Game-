using UnityEngine;
using UnityEngine.InputSystem;

public class LadderScript : MonoBehaviour
{
    public float climbSpeed = 3f;

    private bool playerOnLadder = false;
    private Rigidbody2D playerRb;
    private PlayerInput playerInput;

    private float verticalInput;

    public BoxCollider2D col;





    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }





    void Update()
    {

        
        
        
        
        if (playerOnLadder)
        {
            verticalInput = playerInput.actions["Move"].ReadValue<Vector2>().y;

          

            // Override gravity while climbing
            playerRb.gravityScale = 0f;

            // Move vertically
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, verticalInput * climbSpeed);
        }

     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnLadder = true;
            col.usedByEffector = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerInput = collision.GetComponent<PlayerInput>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnLadder = false;

            col.usedByEffector = false;
            // Restore normal physics
            playerRb.gravityScale = 1f;
        }
    }
}

