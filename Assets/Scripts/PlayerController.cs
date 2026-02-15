
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

    Animator Jump;

    Animator Attack;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    LayerMask enemyLayer;

    [SerializeField]
    float radius = 0.2f;


    Vector2 moveInput;


    [SerializeField] float playerAttackCooldown = 1f;
    float nextAttackTime;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walk = GetComponent<Animator>();
        Jump = GetComponent<Animator>();
        Attack = GetComponent<Animator>();
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
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);

        }
        else if (moveInput.x < 0)
        {

            spriteRenderer.flipX = true;
          
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);

        }


        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f;
        walk.SetBool("isRunning", isMoving);

        Jump.SetBool("isJumping", !isGrounded);

       


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


            Jump.SetBool("isJumping", true);



        }

    
     


    }

    
    
   public void PlayerAttack(InputAction.CallbackContext ctx)
    {

        
        if (ctx.performed && Time.time >= nextAttackTime)
        {
            
            Attack.SetBool("isAttacking", true);
            GiveDamage();

            nextAttackTime = Time.time + playerAttackCooldown;
        }
        else if(ctx.canceled)
        {
            Attack.SetBool("isAttacking", false);
        }


    }

    
    
    public void GiveDamage()
    {

   

        Collider2D[] hits = Physics2D.OverlapCircleAll(

            attackPoint.position,
            radius,
            enemyLayer

            );





        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(20);
            }
        }


    }
    
    
    
    
    
    
    
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

       

        }



        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, 0.5f); // same radius you use in OverlapCircle



    }




    








}
