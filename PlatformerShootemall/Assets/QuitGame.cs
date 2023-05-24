using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] float delay = 15;
    [SerializeField] float delayCountdown; // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(QuitAfter(delay));
    }
    IEnumerator QuitAfter(float timeToDelay)
    {
        delayCountdown = timeToDelay;
        yield return new WaitForSeconds(timeToDelay); //wait x secconds
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        delayCountdown -= Time.deltaTime;
    }
}
