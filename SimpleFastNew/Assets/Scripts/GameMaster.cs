using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour
{
    public GameObject restartPanel;

    public GameObject timerDisplay;

    private bool asLost = false;

    public float timer;

    public void GameOver()
    {
        asLost = true;
        restartPanel.SetActive(true);
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (asLost == false)
        {
            timerDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = timer.ToString("F0");
        }

        if (timer <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
