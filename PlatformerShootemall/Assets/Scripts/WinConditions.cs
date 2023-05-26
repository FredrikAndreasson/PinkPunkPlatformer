using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    private int totalFruit;
    private int fruitLimit;
    [SerializeField] private TextMeshPro text;

    [SerializeField] private Fire[] fires;

    private void Start()
    {
        totalFruit = GameObject.FindGameObjectsWithTag("Fruit").Length;
        fruitLimit = totalFruit / 2;
        text.text = "You need a total of: " + fruitLimit + " fruit to pass";
    }

    void Update()
    {
        Debug.Log("fruit limit:" + fruitLimit + " current fruit:" + GameObject.FindGameObjectsWithTag("Fruit").Length + " total fruit:" + totalFruit);
        //Authenticate
        if (GameObject.FindGameObjectsWithTag("Fruit").Length > fruitLimit)
        {
            return;
        }

        //Log
        Log(totalFruit, 1);

        //Authorize
        Authorize();
    }

    private void Log(int totalFruits, int collected)
    {
        text.text = "You passed the level!!! You have " + collected + "/" + totalFruits + " fruits";
    }

    private void Authorize()
    {
        for (int i = 0; i < fires.Length; i++)
        {
            fires[i].TurnOff();
        }
    }
}
