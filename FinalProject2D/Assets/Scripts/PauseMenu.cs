using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    /// <summary>
    /// when the resume button is pressed, this method is activated.
    /// resume the game
    /// Author: OrS
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// <summary>
    /// when the pause button is pressed, this method is activated.
    /// pause the game
    /// Author: OrS
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// <summary>
    /// when the menu button is pressed, this method is activated.
    /// load the HomeMenu Scene
    /// Author: OrS
    /// </summary>
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeMenu");
    }

    /// <summary>
    /// when the quit button is pressed, this method is activated.
    /// quit the game
    /// Author: OrS
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
