using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GhostBehaviour : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float detectionRange = 10.0f;
    private GhostState currentState;
    private Transform playerTransform;

    private Animator animator;

    public enum GhostState
    {
        Idle,
        Chasing,
        Hit
    }

    void Start()
    {
        currentState = GhostState.Idle;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        bool playerFacingGhost = false;
        if ((playerTransform.position.x < transform.position.x && !playerTransform.gameObject.GetComponent<SpriteRenderer>().flipX)
            || (playerTransform.position.x > transform.position.x && playerTransform.gameObject.GetComponent<SpriteRenderer>().flipX))
        {playerFacingGhost= true;}
        else{playerFacingGhost = false; }

        if (distanceToPlayer <= detectionRange)
        {
            if (playerFacingGhost)
            {
                currentState = GhostState.Idle;
                animator.ResetTrigger("Appear");
                animator.ResetTrigger("Disappear");
            }
            else
            {
                currentState = GhostState.Chasing;
                animator.ResetTrigger("Idle");
            }
        }
        else
        {
            currentState = GhostState.Idle;
            animator.ResetTrigger("Appear");
            animator.ResetTrigger("Disappear");
        }
    }

    void Update()
    {
        UpdateState();
        switch (currentState)
        {
            case GhostState.Chasing:
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
                break;
            default:
                break;
        }
    }


    public void OnAppearAnimationEnd()
    {
        animator.ResetTrigger("Appear");
        animator.SetTrigger("Dissapear");
    }

    public void OnDissapearAnimationEnd()
    {
        animator.ResetTrigger("Dissapear");
        animator.SetTrigger("Appear");
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

            if (hitDirection.y > 0)
            {
                currentState = GhostState.Hit;
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
