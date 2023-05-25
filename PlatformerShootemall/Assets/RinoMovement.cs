using UnityEngine;

public class RinoMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(1.0f, 0.0f);
    public float detectionDistance = 10.0f;
    private enum State { Idle, RunState, WallHit, HitPlayer };
    private State currentState;
    private Animator animator;
    void Start()
    {
        currentState = State.Idle;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D hit;
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("IsRunning", false);
                hit = Physics2D.Raycast(transform.position, Vector2.right * (-transform.localScale.x), detectionDistance, playerLayerMask);
                // maybe change the logic to if the player is close in any kind of way?

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        currentState = State.RunState;
                    }
                }
                break;

            case State.RunState:
                animator.SetBool("IsRunning", true);

                hit = Physics2D.Raycast(transform.position, Vector2.right * (-transform.localScale.x), transform.localScale.x + 0.5f, wallLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        currentState = State.WallHit;
                    }
                }
                else
                {
                    transform.position += Vector3.right * speed.x * Time.deltaTime * (-transform.localScale.x);
                }
                break;

            case State.WallHit:
                animator.SetTrigger("WallHit");
                speed = -speed;
                ChangeDirection();
                currentState = State.Idle;
                break;

            case State.HitPlayer:
                animator.SetTrigger("Hit");
                currentState = State.Idle;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = State.HitPlayer;
        }
    }

    void OnWallHitAnimationEnd()
    {
        currentState = State.Idle;
    }

    void OnHitPlayerAnimationEnd()
    {
        currentState = State.Idle;
    }
    private void ChangeDirection()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
