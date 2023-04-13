using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float dashLength = 4f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private float dashTimerLength = 0.5f;
    private bool dashAvailable = false;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource dashSFX;
    private bool doubleJumpAvailable;

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
/*
        if (dirX != 0)
        {
            currentMovementState = MovementState.running;
        }
*/
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
    private void HandleDash ()
    {
       if (dashAvailable)
        {
            if (Input.GetButton("Fire1"))
            {
                dashTimer = dashTimerLength;
                dashAvailable = false;
                animator.SetTrigger("dash");
                animator.ResetTrigger("dashRecharged");
                Dash();
            }
        }
       else
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                dashAvailable = true;
                animator.SetTrigger("dashRecharged");
                animator.ResetTrigger("dash");
            }
        }
        
    }
    //dash in direction player is facing
    private void Dash ()
    {
        if (!spriteRenderer.flipX) //if facing right
        {
            transform.position = new Vector2(transform.position.x + dashLength, transform.position.y);
        }
        else // facing left
        {
            transform.position = new Vector2(transform.position.x - dashLength, transform.position.y);
        }

    }

    //Orients player sprite in mouse's direction
    private void LookInMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
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

        if(dirX > 0f) //if moving to right
        {
            //spriteRenderer.flipX = false;
            currentMovementState = MovementState.running;

        }
        else if(dirX < 0f) //if moving to left
        {
            //spriteRenderer.flipX = true;
            currentMovementState = MovementState.running;
        }
        else if (body.velocity.x == 0 && body.velocity.y == 0)//if still
        {
            currentMovementState = MovementState.idling;
        }
        
        if(body.velocity.y > 0.1f) //if jumping
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
        if(Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, ground))
        {
            onGround = true;
        }
        return onGround;
    }
}
