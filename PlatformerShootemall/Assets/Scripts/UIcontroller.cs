using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIcontroller : Singleton<UIcontroller>
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject UIDashCooldown;
    [SerializeField] private GameObject Player;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private int score = 0;

    private void Update()
    {
        UpdateHealth();
        UpdateScore();
        UpdateDash();
        
    }

    private void UpdateDash()
    {
        float dashCooldown = Player.GetComponent<PlayerMovement>().dashCooldown;
        UIDashCooldown.GetComponent<CanvasGroup>().alpha = 1f - dashCooldown;
        
          
    }

    private void UpdateScore()
    {
        int score = Player.GetComponent<ItemCollector>().collectedFruit;
        scoreText.text = "Fruit: " + score;
    }

    private void UpdateHealth()
    {
        int health = Player.GetComponent<PlayerLife>().health;
        int maxHealth = Player.GetComponent<PlayerLife>().maxHealth;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
