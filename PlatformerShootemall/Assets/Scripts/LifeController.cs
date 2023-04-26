using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Canvas UI;
    public int life = 5;
    private PlayerLife playerLifeScript;
   
    // Start is called before the first frame update
    void Start()
    {
        playerLifeScript = player.GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        //updateUI
    }

    //update life when hit
    internal void DealDamageFrom(GameObject gameObject, int damage)
    {
        //get damage from gameobject
        life -= damage;

        if (life <= 0)
        {
            playerLifeScript.Die();
        }
        else
        {

            playerLifeScript.GetHit(gameObject);
        }

    }
}
