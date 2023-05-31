using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : Singleton<GameManager>
{
    public IntEventSO _ScoreUpdatedEvent;
    public IntEventSO _ScoreAddedEvent;
    public IntEventSO _HealthUpdatedEvent;
    public IntEventSO _HealthDamagedEvent;

    private int score = 0;

    public int health = 5;

    private void Start()
    {
        _ScoreAddedEvent.Event += UpdateScore;
        _HealthDamagedEvent.Event += UpdateHealth;
    }
    protected override void OnDestroy()
    {
        _ScoreAddedEvent.Event -= UpdateScore;
        _HealthDamagedEvent.Event -= UpdateHealth;
        base.OnDestroy();
    }

    public void UpdateScore(int _score)
    {
        score += _score;
        _ScoreUpdatedEvent?.Invoke(score);
    }

    public void UpdateHealth(int damage)
    {
        health -= damage;
        _HealthUpdatedEvent?.Invoke(health);

    }


}
