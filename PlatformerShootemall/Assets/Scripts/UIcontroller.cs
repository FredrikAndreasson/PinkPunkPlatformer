using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//observer that updates UI
public class UIcontroller : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject uiDashCooldown;
    [SerializeField] private GameObject player;
    [SerializeField] private IntEventSO _scoreEvent;
    [SerializeField] private IntEventSO _healthEvent;

    public Image[] hearts;
    public Sprite dashCharging;
    public Sprite dashReady;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private int score = 0;

    private void Start()
    {
        _scoreEvent.Event += UpdateScore;
        _healthEvent.Event += UpdateHealth;
    }

    private void OnDestroy()
    {
        _scoreEvent.Event -= UpdateScore;
        _healthEvent.Event -= UpdateHealth;
    }

    private void Update()
    {
        UpdateDash();
    }
    //make sprite reflect cooldown status
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
    //check score and update UI
    public void UpdateScore(int score)
    {
        scoreText.text = "Fruit: " + score;
    }
    //check health and update UI
    public void UpdateHealth(int health)
    {
        int maxHealth = player.GetComponent<PlayerLife>().maxHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            //number of full hearts = current health
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            //only show as many hearts as max health allows
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
