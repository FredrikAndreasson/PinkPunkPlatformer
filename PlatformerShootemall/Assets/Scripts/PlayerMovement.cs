using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private float dirX;
    private Vector2 facingDirection;
    private MovementState currentMovementState;
    private Vector2 dashTarget;
    private Vector2 dashStart;
    private bool dashing = false;
    [SerializeField] private float DashSpeed = 50f;


    [SerializeField] private float jumpForce = 15f;
    private bool doubleJumpAvailable;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldownLength = 3f;
    private float adjustedDashDistance;
    public float dashCooldown = 0f;

    private bool dashAvailable = false;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip doubleJumpSFX;
    [SerializeField] private AudioClip dashSFX;

    //enum for storing different movement states
    private enum MovementState
    {
        idling, //0
        running, //1
        jumping, //2
        falling, //3
        dashing, //4
    }


    // instantiate variables
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMovementState = MovementState.idling;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MainMenu.isPaused) return;
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

    //checks what type of jump to perform
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsOnGround())
            {
                
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = true;
                audioSource.PlayOneShot(jumpSFX);
            }
            else if (doubleJumpAvailable)
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = false;
                //animation only triggers if double jumping
                animator.SetTrigger("doubleJump");
                audioSource.PlayOneShot(doubleJumpSFX);
            }
        }
    }
    //Creates a boxcollider slightly lower than the player sprite's
    //returns true if it is touching the ground
    private bool IsOnGround()
    {
        bool onGround = false;
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, ground) ||
            Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, enemy))
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
        if (angle < 0.1f) //facing right
        {
            spriteRenderer.flipX = false;
        }
        else //facing left
        {
            spriteRenderer.flipX = true;
        }
    }
    //handle dashing countdown
    private void HandleDash()
    {
        if (dashAvailable)
        {
            //if (Input.GetButtonDown("Fire1") && CanDash())
            if (Input.GetButton("Fire1") && CanDash())
            {
                dashCooldown = dashCooldownLength;
                dashAvailable = false;
                animator.SetTrigger("dash");
                animator.ResetTrigger("dashRecharged");

                //Dash(facingDirection);

                dashTarget = new Vector2(transform.position.x + (facingDirection.x * adjustedDashDistance), transform.position.y + (facingDirection.y * adjustedDashDistance));
                dashStart = transform.position;

                Debug.Log(dashTarget);
                audioSource.PlayOneShot(dashSFX);
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                dashing = true;
                
            }
        }
        else
        {
            if (dashCooldown >= 0)
            {
                dashCooldown -= Time.deltaTime;
                
                if(dashing == true) //won't this mean that we dash the whole cooldown time?
                    Dash(dashTarget);
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
                dashAvailable = true;
                animator.SetTrigger("dashRecharged");
                animator.ResetTrigger("dash");
            }
        }

    }

    //Check if obstacle in way before dashing, adjust dash distance to just short of object
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
    private void Dash(Vector2 target)
    {
        //dash in facing direction

        //Vector2 target = new Vector2(transform.position.x + (direction.x * adjustedDashDistance), transform.position.y + (direction.y * adjustedDashDistance));
        //transform.position = Vector2.MoveTowards(transform.position, target, adjustedDashDistance);
        //audioSource.PlayOneShot(dashSFX);

        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * DashSpeed);
        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            dashing = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        }

        if(Vector2.Distance(transform.position, dashStart) > 6)
        {
            dashing = false;
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        }
        //audioSource.PlayOneShot(dashSFX);
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
    //stop dashing if wall or ground in way
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            if (dashing == true)
            {
                dashing = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
                transform.position -= new Vector3(facingDirection.x * 0.1f, facingDirection.y * 0.1f, 0);
            }
        }
    }

}
