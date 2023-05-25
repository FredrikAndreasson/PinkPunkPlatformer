using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public float attackDistance = 10.0f;
    public float hitDistance = 1.0f;
    public float attackInterval = 30.0f;
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
        timeSinceLastAttack = 0;
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
        projectile.GetComponent<Projectile>().SetDirection(new Vector3(-transform.localScale.x, 0).normalized);
        ChangeState(PlantState.Idle);
        timeSinceLastAttack = 0;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 hitDirection = (other.transform.position - transform.position).normalized;

            if (hitDirection.y > 0) // Player is jumping on the plant
            {
                ChangeState(PlantState.Hit);
            }
        }


        if (other.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        
    }
}
