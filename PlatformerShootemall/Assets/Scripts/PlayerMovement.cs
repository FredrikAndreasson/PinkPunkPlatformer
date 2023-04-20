using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float dirX;
    private MovementState currentMovementState;

    [SerializeField] private float jumpForce = 15f;
    private bool doubleJumpAvailable;

    [SerializeField] private float moveSpeed = 10f;
    private Vector2 facingDirection;

    [SerializeField] private float dashLength = 1f;
    private float adjustedDashLength;
    private float dashTimer = 0f;
    [SerializeField] private float dashTimerLength = 0.5f;
    [SerializeField] private Transform dashEffect;
    private bool dashAvailable = false;

    [SerializeField] private LayerMask ground;
    [SerializeField] private AudioSource jumpSFX;
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
                //jumpSFX.Play();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = true;
            }
            else if (doubleJumpAvailable)
            {
                //jumpSFX.Play();
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                doubleJumpAvailable = false;
                //animation only triggers if double jumping
                animator.SetTrigger("doubleJump");
            }
        }

    }
    //handle dashing countdown
    private void HandleDash()
    {
        if (dashAvailable)
        {
            if (Input.GetButton("Fire1") && CanDash())
            {
                dashTimer = dashTimerLength;
                dashAvailable = false;
                animator.SetTrigger("dash");
                animator.ResetTrigger("dashRecharged");
                Dash(facingDirection);
            }
        }
        else
        {
            if (dashTimer >= 0)
            {
                dashTimer -= Time.deltaTime;
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
        Debug.Log("TestCanDash");

        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, dashLength, ~playerLayerMask);
        adjustedDashLength = dashLength;
        if (hit.collider != null)
        {
            adjustedDashLength = Mathf.Abs(hit.distance);
            Debug.DrawRay(transform.position, facingDirection * adjustedDashLength, Color.red, 1f);
            if (adjustedDashLength < 1f)
            {
                canDash = false;
            }
        }
        Debug.DrawRay(transform.position, facingDirection * adjustedDashLength, Color.green);
        Debug.Log("dashlength: " + adjustedDashLength);
        return canDash;
    }

    private void OnDrawGizmos()
    {

        Debug.DrawRay(transform.position, facingDirection * dashLength, Color.green);
    }

    //dash in direction player is facing
    private void Dash(Vector2 direction)
    {
        Vector2 initialPosition = transform.position;
        //dash in facing direction
        transform.position = Vector2.MoveTowards(transform.position, direction * adjustedDashLength, adjustedDashLength);
        //create dash sprite
        Transform dashEffectTransform = Instantiate(dashEffect, initialPosition, Quaternion.identity);
        StartCoroutine(FadeOut(dashEffectTransform, 1f));
    }
    //fadeout prefab while moving towards object
    IEnumerator FadeOut(Transform trans, float duration)
    {
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, counter / duration);
            Color color = transform.GetComponent<Renderer>().material.color;
            trans.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);
            trans.position = Vector2.MoveTowards(trans.position, transform.position, counter);
            yield return null;
        }
        Destroy(trans.gameObject);
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
        else if (body.velocity.x == 0 && body.velocity.y == 0)//if still
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
}
