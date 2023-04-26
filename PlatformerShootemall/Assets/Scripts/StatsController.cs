using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Canvas UI;
    private int life;
    private float dashCooldown;
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
   
    // Start is called before the first frame update
    void Start()
    {
        playerLife = player.GetComponent<PlayerLife>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //updateUI
        dashCooldown = playerMovement.dashCooldown;
        life = playerLife.life;
        
    }
}
