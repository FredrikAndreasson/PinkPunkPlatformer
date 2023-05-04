using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class UIcontroller : Singleton<UIcontroller>
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject uiDashCooldown;
    [SerializeField] private GameObject player;
    public Image[] hearts;
    public Sprite dashCharging;
    public Sprite dashReady;
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
        float dashCooldown = player.GetComponent<PlayerMovement>().dashCooldown;
        uiDashCooldown.GetComponent<CanvasGroup>().alpha = 1f - dashCooldown;
        if (dashCooldown < 0.1f)
        {
            uiDashCooldown.GetComponent<Image>().overrideSprite = dashReady;
        }
        else
        {
            uiDashCooldown.GetComponent<Image>().overrideSprite = dashCharging;
        }

    }

    private void UpdateScore()
    {
        int score = player.GetComponent<ItemCollector>().collectedFruit;
        scoreText.text = "Speed: " + score;
    }

    private void UpdateHealth()
    {
        int health = player.GetComponent<PlayerLife>().health;
        int maxHealth = player.GetComponent<PlayerLife>().maxHealth;
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
