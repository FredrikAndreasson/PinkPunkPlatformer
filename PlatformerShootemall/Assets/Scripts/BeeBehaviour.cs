using UnityEngine;

public class BeeBehaviour : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public GameObject projectilePrefab;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private WaypointTraverser traverser;
    private float timeSinceLastAttack = 0f;
    private float dist;

    const string BEE_IDLE = "BeeIdle";
    const string BEE_ATTACK = "BeeAttack";
    const string BEE_HIT = "BeeHit";

    // Finite State Machine States
    private enum State
    {
        PatrolState,
        FollowState,
        AttackState
    }

    private State currentState = State.PatrolState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        traverser = GetComponent<WaypointTraverser>();
        traverser.speed = moveSpeed;
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        switch (currentState)
        {
            case State.PatrolState:
                PatrolUpdate();
                break;
            case State.FollowState:
                FollowUpdate();
                break;
            case State.AttackState:
                AttackUpdate();
                break;
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    void PatrolUpdate()
    {
        // Fly around randomly
        traverser.traverse = true;

        if (dist < 11)
        {
            currentState = State.FollowState;
            traverser.traverse = false;
        }
    }

    void FollowUpdate()
    {
        // Move towards the player
        movement = (player.position - transform.position).normalized;

        if (dist > 11)
        {
            currentState = State.PatrolState;
            anim.Play(BEE_IDLE);
        }

        if (dist < 7)
        {
            currentState = State.AttackState;
            anim.Play(BEE_ATTACK);
        }
    }

    void AttackUpdate()
    {
        // Stop moving and shoot projectiles at the player
        movement = Vector2.zero;

        if (timeSinceLastAttack > 2f)
        {
            Shoot();
            timeSinceLastAttack = 0f;
        }

        if (dist > 7)
        {
            currentState = State.FollowState;
            anim.Play(BEE_IDLE);
        }
    }

    void FixedUpdate()
    {
        // Move the enemy
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        // Instantiate a projectile and shoot it at the player
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized);
    }
    //ignore collisions with other enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
