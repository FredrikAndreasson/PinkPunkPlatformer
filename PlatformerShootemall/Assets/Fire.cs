using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    float timer;
    public float activationTimer;
    public Sprite off;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > activationTimer)
        {
            this.gameObject.GetComponent<Animator>().enabled = false;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = off;

            //There are 2 collders, ON collision with player, you can seperate by checking "on trigger" or not
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.TakeDamage();
            //take damage, and push player back!
            //maybe add "OnTriggerExit2D()"
        }
    }

}
