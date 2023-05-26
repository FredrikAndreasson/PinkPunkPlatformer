using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    public float riseTimer = 0.5f;
    private float timeBeforeRise = 0.5f;
    public int riseSpeed = 5;
    public float resetTimer = 2;
    private bool currentlyColliding = false;
    private Vector2 startPos;


    //move player with platform
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
            currentlyColliding = true;
            resetTimer = 2;
        }

    }
    //decouple from player
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            currentlyColliding = false;
        }

    }
    //set return position
    public void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        RaisePlatform();
    }
    //move platform if player present, else return to start position after a countdown
    private void RaisePlatform()
    {
        if (!currentlyColliding)
            resetTimer -= Time.deltaTime;

        if (currentlyColliding)
        {
            if (timeBeforeRise < 0)
            {
                transform.position += Vector3.up * Time.deltaTime * riseSpeed;
            }
            timeBeforeRise -= Time.deltaTime;
        }
        else
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer < 0 && transform.position.y >= startPos.y)
            {
                transform.position += Vector3.down * Time.deltaTime * riseSpeed / 2;
            }
            if (transform.position.y <= startPos.y)
            {
                timeBeforeRise = riseTimer;
            }
        }
    }
}

