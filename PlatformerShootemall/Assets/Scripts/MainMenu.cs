using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu;
    [SerializeField] public GameObject optionsMenu;
    [SerializeField] GameObject pauseBackground;
    //load first level (scene)
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 
    //exit game
    public void QuitGame()
    {
        Application.Quit();
    }
    //show/hide pause menu when escape pressed
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
    //hide menus and resume game
    public void ResumeGame()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        pauseBackground.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

    }
    //pause game and show menu
    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseBackground.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
}
