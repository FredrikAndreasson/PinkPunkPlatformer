using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 dir = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //move projectile each frame
    private void Update()
    {
        transform.position += vel * Time.deltaTime;
    }
    //set direction projectile moves
    public void SetDirection(Vector3 direction)
    {
        vel = direction * speed;
    }
    //destroy when hitting a shield
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield")) Destroy(gameObject);
    }
    //destroy when hitting thge player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }
}
