using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    private int totalFruit;
    private int requiredFruit;
    [SerializeField] private TextMeshPro text;

    [SerializeField] private Fire[] fires;

    private void Start()
    {
        totalFruit = GameObject.FindGameObjectsWithTag("Fruit").Length;
        requiredFruit = totalFruit / 2;
        text.text = "You need a total of: " + requiredFruit + " fruit to pass";
    }

    void Update()
    {
        int remainingFruit = GameObject.FindGameObjectsWithTag("Fruit").Length;
        int collectedFruit = totalFruit - remainingFruit;
        Debug.Log("required fruit:" + requiredFruit + " current fruit:" + collectedFruit + " total fruit:" + totalFruit);
        //Authenticate
        if (collectedFruit < requiredFruit)
        {
            return;
        }

        //Log
        Log(totalFruit, totalFruit-remainingFruit);

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
