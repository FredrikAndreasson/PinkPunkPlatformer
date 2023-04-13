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
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float dashForce = 22f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource dashSFX;
    private bool doubleJumpAvailable;

    //enum for storing different movement states
    private enum MovementState
    {
        idling,
        running,
        jumping,
        falling,
        dashing,
        doubleJumping
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
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);
        if (Input.GetButtonDown("Jump")) 
        {
            Jump();
        }
        LookInMouseDirection();
        if (Input.GetButton("Fire1"))
        {
            //dashSFX.Play();
            body.velocity = new Vector2(dashForce, body.velocity.y);
        }
        UpdateAnimationState();
        
    }
    //checks what time of jump to perform
    private void Jump()
    {
        if(IsOnGround())
        {
            //jumpSFX.Play();
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            return;
        }
        if (doubleJumpAvailable && (currentMovementState == MovementState.jumping || currentMovementState == MovementState.falling))
        {
            //jumpSFX.Play();
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            doubleJumpAvailable = false;
            animator.SetTrigger("doubleJump");
        }

    }
    //Orients player sprite in mouse's direction
    //Sets dash force to same direction
    private void LookInMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        if (angle < 0.1f)
        {
            spriteRenderer.flipX = false;
            dashForce = Math.Abs(dashForce);
        }
        else
        {
            spriteRenderer.flipX = true;
            dashForce = -(Math.Abs(dashForce));
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
        else //if still
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
        if(Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.2f, ground))
        {
            doubleJumpAvailable = true;
            onGround = true;
        }
        return onGround;
    }
}
