using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{

   // variables for enemy patrolling
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    // variables for enemy agro
    [SerializeField]
    public Transform player;
    [SerializeField]
    float agroRange;
    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    private SpriteRenderer sr;

  


    // variables for enemy only chasing you while on same platform

    [SerializeField]
    Tilemap platformTilemap;

    [SerializeField]

   private Grid grid;





    // variables below is for enemy attacking

    [SerializeField] float AttackRange = 0.8f;


    [SerializeField] float attackCooldown = 1f;
    float nextAttackTime;

    Animator anim;



    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int damageAmount = 10;


    private bool isAttacking = false;







    Vector3Int GetTilePos(Transform target)
    {
        return grid.WorldToCell(target.position);
    }
    
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPoint = pointA.transform;

        anim = GetComponent<Animator>();

      
    }

    // Update is called once per frame
    void Update()
    {


       



            float distToPlayer = Mathf.Abs(transform.position.x - player.position.x);










        Vector3Int playerTile = GetTilePos(player);
        Vector3Int enemyTile = GetTilePos(transform);

    

        bool samePlatformRow = Mathf.Abs(playerTile.y - enemyTile.y) <= 1;



        if (distToPlayer <= AttackRange && samePlatformRow && Time.time >= nextAttackTime)
        {

            Debug.Log("IN RANGE | Distance: " + distToPlayer + " | AttackRange: " + AttackRange);
            
            
            EnemyAttack();
           

        }

        else if (distToPlayer < agroRange && samePlatformRow && !isAttacking)
        {
            // code to chase player

          
            chasePlayer();

        }
        else 
        {
          
            enemyPatrolling();
        }




            bool enemyMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;

            anim.SetBool("enemyIsWalking", enemyMoving);



        if (rb.linearVelocity.x > 0.1f)
        {
            sr.flipX = false;   // facing right
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);

        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            sr.flipX = true;    // facing left

            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }






    }






    void enemyPatrolling()
    {
    
        
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }

        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
    }
    
    
    
    
    void stopChasingPlayer()
    {
     
    }

     void chasePlayer()
    {

        
        
        
        if (transform.position.x < player.position.x )
        {
            rb2d.linearVelocity = new Vector2(moveSpeed, 0); // enenmy is to left side of player so mover right
        }
        else if(transform.position.x > player.position.x)
        {
            rb2d.linearVelocity = new Vector2(-moveSpeed, 0); // enemy is to the right of the player so move left 
        }

      
    
    
    }




 

    public void EnemyAttack()
    {

        isAttacking = true;

        anim.SetBool("enemyIsWalking", false);

        rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
        
       anim.SetTrigger("enemyIsAttacking");

        EnemyDealDamage();

        nextAttackTime = Time.time + attackCooldown;

    }






   public void EnemyDealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(25);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }




   
       
           
       
           
       








}
