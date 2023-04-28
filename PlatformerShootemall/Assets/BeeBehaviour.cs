using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackRange = 5f;
    public GameObject projectilePrefab;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private float timeSinceLastAttack = 0f;

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
    }

    void Update()
    {
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
        movement.x = Random.Range(-1f, 1f);
        movement.y = Random.Range(-1f, 1f);

        if (Vector3.Distance(transform.position, player.position) < 300)
        {
            currentState = State.FollowState;
        }
    }

    void FollowUpdate()
    {
        // Move towards the player
        movement = (player.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, player.position) > 300)
        {
            currentState = State.PatrolState;
        }

        if (Vector3.Distance(transform.position, player.position) < 100)
        {
            currentState = State.AttackState;
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
    }

    void FixedUpdate()
    {
        // Move the enemy
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        // Instantiate a projectile and shoot it at the player
        //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        //projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized);
    }
}
