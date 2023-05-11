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
        Chasing
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
                ChangeState(GhostState.Idle);
            }
            else
            {
                ChangeState(GhostState.Chasing);
            }
        }
        else
        {
            ChangeState(GhostState.Idle);
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

    private void ChangeState(GhostState newState)
    {
        currentState = newState;
        animator.SetTrigger(newState.ToString());
    }

    public void Reset()
    {
        
    }
}
