
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 5.0f;

    float jumpSpeed = 5f;
    
    
    public Rigidbody2D rb;
    
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    SpriteRenderer spriteRenderer;

    bool isGrounded;

    Animator walk;
        

    
    
    Vector2 moveInput;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walk = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y); // New x vector value is as shown but y stays the same

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = hit != null;

        if (hit != null)
        {
           // UnityEngine.Debug.Log("Hit: " + hit.gameObject.name);
        }
        else
        {
           // UnityEngine.Debug.Log("Hit: null");
        }


        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {

            spriteRenderer.flipX = true;
        
        }


        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f;
        walk.SetBool("isRunning", isMoving);
    
    
    
    }


    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

       
    }



    public void PlayerJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded) // allows jump is you are grounded
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

           



        }



    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

       

        }
    }




}
