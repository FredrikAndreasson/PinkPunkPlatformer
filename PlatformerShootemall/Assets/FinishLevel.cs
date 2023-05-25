using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private Animator flagAnimator;
    [SerializeField] private AudioSource finishSFX;    private bool isFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        flagAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.name == ("Player") && !isFinished)
        {
            isFinished = true;
            flagAnimator.SetTrigger("PlayerHit");
            finishSFX.Play();
            Invoke("CompleteLevel", 1f);
        }
    }

    private void CompleteLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
