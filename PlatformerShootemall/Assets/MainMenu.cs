using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseUI;
    [SerializeField] GameObject pauseBackground;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 

    public void QuitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
        
    }
    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        pauseBackground.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        
    }
    private void PauseGame()
    {
        pauseUI.SetActive(true);
        pauseBackground.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
}
