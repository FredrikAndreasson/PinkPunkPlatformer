using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float dirX;
    private Vector2 facingDirection;
    private MovementState currentMovementState;

    [SerializeField] private float jumpForce = 15f;
    private bool doubleJumpAvailable;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldownLength = 0.5f;
    private float adjustedDashDistance;
    public float dashCooldown = 0f;

    private bool dashAvailable = false;

    [SerializeField] private LayerMask ground;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource doubleJumpSFX;
    [SerializeField] private AudioSource dashSFX;

    //enum for storing different movement states
    private enum MovementState
    {
        idling, //0
        running, //1
        jumping, //2
        falling, //3
        dashing, //4
    }


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMovementState = MovementState.idling;
    }

    // Update is called once per frame
    void Update()
    {
        //check if direction pressed
        HandleMovement();
        //check if jump pressed
        HandleJump();
        //orient player
        LookInMouseDirection();
        //check if dash pressed
        HandleDash();
        //update animation
        UpdateAnimationState();

    }
    //check for horizontal movement
    private void HandleMovement()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);
    }

    //checks what time of jump to perform
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsOnGround())
            {
                
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = true;
                jumpSFX.Play();
            }
            else if (doubleJumpAvailable)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = false;
                //animation only triggers if double jumping
                animator.SetTrigger("doubleJump");
                doubleJumpSFX.Play();
            }
        }
    }
    //Creates a boxcollider slightly lower than the player sprite's
    //returns true if it is touching the ground
    private bool IsOnGround()
    {
        bool onGround = false;
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, ground))
        {
            onGround = true;
        }
        return onGround;
    }
    //Orients player sprite in mouse's direction
    private void LookInMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        facingDirection = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, facingDirection);
        if (angle < 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    //handle dashing countdown
    private void HandleDash()
    {
        if (dashAvailable)
        {
            if (Input.GetButton("Fire1") && CanDash())
            {
                dashCooldown = dashCooldownLength;
                dashAvailable = false;
                animator.SetTrigger("dash");
                animator.ResetTrigger("dashRecharged");

                Dash(facingDirection);
            }
        }
        else
        {
            if (dashCooldown >= 0)
            {
                dashCooldown -= Time.deltaTime;
            }
            else
            {
                dashAvailable = true;
                animator.SetTrigger("dashRecharged");
                animator.ResetTrigger("dash");
            }
        }

    }

    //Check if obstacle in way before dashing
    private bool CanDash()
    {
        bool canDash = true;
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, dashDistance, ~playerLayerMask);
        adjustedDashDistance = dashDistance;
        if (hit.collider != null)
        {
            //shorten dash length if obstacle in way
            adjustedDashDistance = Mathf.Abs(hit.distance);
            if (adjustedDashDistance < 1f)
            {
                canDash = false;
                Debug.Log("Cannot dash");
            }
        }
        return canDash;
    }

    private void OnDrawGizmos()
    {

        Debug.DrawRay(transform.position, facingDirection * dashDistance, Color.green);
    }

    //dash in direction player is facing
    private void Dash(Vector2 direction)
    {
        //dash in facing direction
        transform.position = Vector2.MoveTowards(transform.position, direction * adjustedDashDistance, adjustedDashDistance);
        dashSFX.Play();
    }

    //Updates the sprite's animation based on movement
    private void UpdateAnimationState()
    {

        if (dirX > 0f) //if moving to right
        {
            currentMovementState = MovementState.running;

        }
        else if (dirX < 0f) //if moving to left
        {
            currentMovementState = MovementState.running;
        }
        else //if still
        {
            currentMovementState = MovementState.idling;
        }

        if (body.velocity.y > 0.1f) //if jumping
        {
            currentMovementState = MovementState.jumping;
        }
        else if (body.velocity.y < -0.1f) //if falling
        {
            currentMovementState = MovementState.falling;
        }

        //set animation based on moveState
        animator.SetInteger("currentMovementState", (int)currentMovementState);

    }

}
