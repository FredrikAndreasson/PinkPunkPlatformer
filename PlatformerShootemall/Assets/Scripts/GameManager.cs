using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : Singleton<GameManager>
{

    public event Action<int> ScoreUpdated;
    public event Action HealthUpdated;

    private int score = 0;

    private int health = 3; 

    public void AddScore(int _score)
    {
        score += _score;
        ScoreUpdated.Invoke(score);

        //scoreText.text = "Score: " + score; // Seperate UI script should not subcribe and update itself 
    }

    public void RemoveScore(int _score)
    {
        score -= _score;
        ScoreUpdated?.Invoke(score);

        //scoreText.text = "Score: " + score;

    }

    public void TakeDamage()
    {
        health--;
        HealthUpdated?.Invoke();

    }

    public void Heal()
    {
        health++;
        HealthUpdated?.Invoke();
    }


}
