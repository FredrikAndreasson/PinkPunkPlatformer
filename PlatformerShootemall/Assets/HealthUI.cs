using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.HealthUpdated += HealthChange;
    }

    private void HealthChange()
    {
        //probably not the best way 
        gameObject.GetComponent<Text>().text = "Health: " + 2;
    }
}
