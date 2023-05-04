using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float droptimer = 0.5f; 
    private float timeBeforeDrop = 0.5f;
    public int dropSpeed = 5;
    public float resetTimer = 3;
    private bool currentlyColliding;
    private Vector2 startPos;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
            currentlyColliding = true;
            resetTimer = 3;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            currentlyColliding = false;
        }
        
    }
    public void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        DropPlatform();
    }

    private void DropPlatform()
    {
        if (!currentlyColliding)
            resetTimer -= Time.deltaTime;

        if (currentlyColliding)
        {
            if (timeBeforeDrop < 0)
            {
                transform.position += Vector3.down * Time.deltaTime * dropSpeed;
            }
            timeBeforeDrop -= Time.deltaTime;
        }
        else
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer < 0 && transform.position.y <= startPos.y)
            {
                transform.position += Vector3.up * Time.deltaTime * dropSpeed / 2;
            }
            if(transform.position.y >= startPos.y)
            {
                timeBeforeDrop = droptimer;
            }
        }
    }
}
