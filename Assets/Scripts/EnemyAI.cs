using System;
using Unity.VisualScripting;
using UnityEngine;
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

    Animator EnemyWalk;



    // variables below is for enemy attacking

    bool isEnemyAttacking;

    [SerializeField] float attackRange = 1.5f;

    Animator enemyAttack;



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

        EnemyWalk = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

       
     
        
        
        // distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        Vector3Int playerTile = GetTilePos(player);
        Vector3Int enemyTile = GetTilePos(transform);

        //Debug.Log("distToPlayer" + distToPlayer);

        bool samePlatformRow = Mathf.Abs(playerTile.y - enemyTile.y) <= 1;
        
        
        if(distToPlayer < agroRange && samePlatformRow)
        {
            // code to chase player
            
                chasePlayer();

        }
        else 
        {
            enemyPatrolling();
        }





    




            bool enemyMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        EnemyWalk.SetBool("enemyIsWalking", enemyMoving);


        if (rb.linearVelocity.x > 0.1f)
        {
            sr.flipX = false;   // facing right
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            sr.flipX = true;    // facing left
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




 




}
