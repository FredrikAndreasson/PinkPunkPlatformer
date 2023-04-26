using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] int life = 5;
    [SerializeField] Transform player;
    [SerializeField] Canvas UI;

    [SerializeField] AudioSource getHitSFX;
    //update life when hit
    internal void GetHit(int damage)
    {
        life -= damage;
        getHitSFX.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //updateUI
        //checkif dead
        if (life <= 0)
        {
           
        }
    }
}
