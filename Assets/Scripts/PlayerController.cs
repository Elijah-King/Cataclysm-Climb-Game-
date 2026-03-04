using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables for movment 

    [SerializeField] float moveSpeed = 5.0f;

    [SerializeField] float acceleration = 10.0f;

    [SerializeField] float decceleration = 8f;

    [SerializeField] float velPower = 1.2f;

    [SerializeField] float jumpSpeed = 5f;

    [SerializeField] float gravityMultiplier = 2f;

    public Rigidbody2D rb;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask PlayerGroundLayer;
    SpriteRenderer spriteRenderer;

    bool isGrounded;

    Animator walk;
    Animator Jump;
    Animator Attack;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    LayerMask enemyLayer;


    private bool hasHit = false;

    [SerializeField]
    float radius = 0.2f;

    Vector2 moveInput;

    [SerializeField] float playerAttackCooldown = 1f;
    float nextAttackTime;

    public bool isFrozen;

    public bool isMoving;

    //  REQUIRED FOR LADDER
    public bool isOnLadder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walk = GetComponent<Animator>();
        Jump = GetComponent<Animator>();
        Attack = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            walk.SetBool("isRunning", false);
            Jump.SetBool("isJumping", false);
            isMoving = false;
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, PlayerGroundLayer);
        isGrounded = hit != null;

        //  DO NOT FLIP WHILE ON LADDER
        if (!isOnLadder)
        {
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
        }

        //  DO NOT OVERRIDE CLIMB ANIMATION
        if (isOnLadder)
        {
            walk.SetBool("isRunning", false);
            Jump.SetBool("isJumping", false);
            return;
        }

        isMoving = Mathf.Abs(moveInput.x) > 0.1f;
        walk.SetBool("isRunning", isMoving);

        Jump.SetBool("isJumping", !isGrounded);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput.x * moveSpeed;

        float speedDif = targetSpeed - rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector2.down * gravityMultiplier, ForceMode2D.Force);
        }

        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void PlayerJump(InputAction.CallbackContext ctx)
    {
        if (isFrozen)
        {
            return;
        }

        if (ctx.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            Jump.SetBool("isJumping", true);
        }
    }

    public void PlayerAttack(InputAction.CallbackContext ctx)
    {
        if (isFrozen) return;

        if (ctx.performed && Time.time >= nextAttackTime)
        {
            Attack.SetBool("isAttacking", true);
            hasHit = false; // reset hit flag
            nextAttackTime = Time.time + playerAttackCooldown;
        }
        else if (ctx.canceled)
        {
            Attack.SetBool("isAttacking", false);
        }
    }


    public void GiveDamage()
    {

        if (hasHit)
        {
            return;
        }
        
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
                enemy.TakeDamage(50);
                hasHit = true;
                break;
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
        Gizmos.DrawWireSphere(attackPoint.position, 0.5f);
    }
}


