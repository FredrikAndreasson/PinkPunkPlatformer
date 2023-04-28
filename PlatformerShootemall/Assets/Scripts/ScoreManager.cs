using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private Text scoreText;

    private int score = 0;

    public void AddScore(int _score)
    {
        score += _score;
        scoreText.text = "Score: " + score; 
    }

    public void RemoveScore(int _score)
    {
        score -= _score;
        scoreText.text = "Score: " + score;

    }
}
