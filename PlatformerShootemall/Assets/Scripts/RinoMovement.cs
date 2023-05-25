using UnityEngine;

public class RinoMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5.0f, 0.0f);
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
        int wallLayerMask = 1 << LayerMask.NameToLayer("Ground");

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("IsRunning", false);
                hit = Physics2D.Raycast(transform.position, -transform.right, detectionDistance, playerLayerMask);

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

                hit = Physics2D.Raycast(transform.position, -transform.right, 1.0f, wallLayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        animator.SetBool("IsRunning", false);
                        currentState = State.WallHit;
                    }
                }
                else
                {
                    transform.position += Vector3.right * speed.x * Time.deltaTime * -transform.right.x;
                }
                break;

            case State.WallHit:
                animator.SetTrigger("HitWall");
                break;

            case State.HitPlayer:
                animator.SetTrigger("Hit");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsRunning", false);
            currentState = State.HitPlayer;
        }
    }

    void OnWallHitAnimationEnd()
    {
        currentState = State.Idle;
        Debug.Log("Turning");
        animator.ResetTrigger("HitWall");
        ChangeDirection();
    }

    void OnHitPlayerAnimationEnd()
    {
        currentState = State.Idle;
        animator.ResetTrigger("Hit");
    }
    private void ChangeDirection()
    {
        transform.Rotate(0, 180, 0);
    }
}
