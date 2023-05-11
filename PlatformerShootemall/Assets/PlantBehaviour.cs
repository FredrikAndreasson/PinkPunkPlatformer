using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public float attackDistance = 5.0f;
    public float hitDistance = 1.0f;
    public int health = 3;
    public float attackInterval = 1.0f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private Animator animator;
    private GameObject player;
    private float timeSinceLastAttack;

    private enum PlantState { Idle, Attack, Hit }
    private PlantState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = PlantState.Idle;
        timeSinceLastAttack = attackInterval;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        timeSinceLastAttack += Time.deltaTime;

        switch (currentState)
        {
            case PlantState.Idle:
                if (distanceToPlayer <= attackDistance && distanceToPlayer > hitDistance && timeSinceLastAttack > attackInterval)
                {
                    ChangeState(PlantState.Attack);
                }
                break;

            case PlantState.Attack:
                if (distanceToPlayer > attackDistance || distanceToPlayer <= hitDistance)
                {
                    ChangeState(PlantState.Idle);
                }
                break;

            case PlantState.Hit:
                break;
        }
    }


    private void ChangeState(PlantState newState)
    {
        currentState = newState;
        animator.SetTrigger(newState.ToString());
    }

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        timeSinceLastAttack = 0;
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 hitDirection = (other.transform.position - transform.position).normalized;

            if (hitDirection.y > 0) // Player is jumping on the plant
            {
                ChangeState(PlantState.Hit);
                health--;

                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
