using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;


    private void Update()
    {
       
       if(Input.GetKeyDown(KeyCode.Escape))
            {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            }
        }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void EndGame()
    {
        Debug.Log("The game has ended.");
        Application.Quit();
    }
}

