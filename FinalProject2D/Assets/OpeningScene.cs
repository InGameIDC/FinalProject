using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    public GameObject gs;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartDelay");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartDelay()
    {
        float pauseTime = Time.realtimeSinceStartup + 3f;
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }

        if (PlayerPrefs.GetInt("tutorialPlayed") == 1)
        {
            SceneManager.LoadScene("HomeMenu");
        }
        else
        {
            GetComponent<ResetGameStatus>().ResetGameStatusDataOpenScene();
            PlayerPrefs.SetInt("tutorialPlayed", 1);
            gs.GetComponent<GameStatus>().tutorialPlayed = 1;
            SceneManager.LoadScene("Tutorial1.1");
        }

    }
}
