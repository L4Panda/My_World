using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

    // ----------------------------------------------------------------
    public void BeginGame()
    {
        SceneManager.LoadScene("Dialogue"); // Start the dialogue scene
    }
    // ----------------------------------------------------------------
    public void EndGame()
    {
        Debug.Log("The game has ended.");
        Application.Quit();
    }
    // ----------------------------------------------------------------
}
