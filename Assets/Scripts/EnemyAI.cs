using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    // ---------------- PATROL VARIABLES ----------------
    public GameObject pointA;
    public GameObject pointB;
    private Transform currentPoint;
    public float speed;

    // ---------------- AGRO VARIABLES ----------------
    [SerializeField] public Transform player;
    [SerializeField] float agroRange;
    [SerializeField] float moveSpeed;

    // ---------------- COMPONENTS ----------------
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // ---------------- PLATFORM CHECK ----------------
    [SerializeField] Tilemap platformTilemap;
    [SerializeField] private Grid grid;

    // ---------------- ATTACK VARIABLES ----------------
    [SerializeField] float AttackRange = 0.8f;
    [SerializeField] float attackCooldown = 1f;
    float nextAttackTime;

    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int damageAmount = 10;

    private bool isAttacking = false;

    // ---------------- STATE MACHINE ----------------
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState = EnemyState.Patrol;


    // ---------------- UNITY METHODS ----------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentPoint = pointA.transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolState();
                break;

            case EnemyState.Chase:
                ChaseState();
                break;

            case EnemyState.Attack:
                AttackState();
                break;
        }

        FlippingPlayerLogic();
    }


    // ---------------- STATE LOGIC ----------------

    void PatrolState()
    {
        enemyPatrolling();
        anim.SetBool("enemyIsWalking", true);

        float dist = Mathf.Abs(transform.position.x - player.position.x);
        bool sameRow = SamePlatformRow();

        if (dist < agroRange && sameRow)
            currentState = EnemyState.Chase;
    }

    void ChaseState()
    {
        chasePlayer();
        anim.SetBool("enemyIsWalking", true);

        float dist = Mathf.Abs(transform.position.x - player.position.x);
        bool sameRow = SamePlatformRow();

        if (dist <= AttackRange && sameRow && Time.time >= nextAttackTime)
            currentState = EnemyState.Attack;

        if (dist > agroRange || !sameRow)
            currentState = EnemyState.Patrol;
    }

    void AttackState()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("enemyIsWalking", false);

        EnemyAttack();

        currentState = EnemyState.Chase;
    }


    // ---------------- HELPER LOGIC ----------------

    bool SamePlatformRow()
    {
        Vector3Int playerTile = GetTilePos(player);
        Vector3Int enemyTile = GetTilePos(transform);

        return Mathf.Abs(playerTile.y - enemyTile.y) <= 1;
    }

    Vector3Int GetTilePos(Transform target)
    {
        return grid.WorldToCell(target.position);
    }


    // ---------------- MOVEMENT ----------------

    void enemyPatrolling()
    {
        if (currentPoint == pointB.transform)
            rb.linearVelocity = new Vector2(speed, 0);
        else
            rb.linearVelocity = new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = (currentPoint == pointA.transform) ? pointB.transform : pointA.transform;
        }
    }

    void chasePlayer()
    {
        if (transform.position.x < player.position.x)
            rb.linearVelocity = new Vector2(moveSpeed, 0);
        else
            rb.linearVelocity = new Vector2(-moveSpeed, 0);
    }


    // ---------------- ATTACK ----------------

    public void EnemyAttack()
    {
        isAttacking = true;

        anim.SetBool("enemyIsWalking", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        anim.SetTrigger("enemyIsAttacking");

        EnemyDealDamage();

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
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
                player.TakeDamage(damageAmount);
            }
        }
    }


    // ---------------- FLIPPING ----------------

    public void FlippingPlayerLogic()
    {
        if (isAttacking) return;

        if (rb.linearVelocity.x > 0.1f)
        {
            sr.flipX = false;
            attackPoint.localPosition = new Vector3(
                Mathf.Abs(attackPoint.localPosition.x),
                attackPoint.localPosition.y,
                0
            );
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            sr.flipX = true;
            attackPoint.localPosition = new Vector3(
                -Mathf.Abs(attackPoint.localPosition.x),
                attackPoint.localPosition.y,
                0
            );
        }
    }


    // ---------------- GIZMOS ----------------

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}


