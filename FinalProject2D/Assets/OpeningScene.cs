using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    public GameObject gas;
    private GameStatus gs;
    // Start is called before the first frame update
    void Start()
    {

        gs = gas.GetComponent<GameStatus>();
        StartCoroutine("StartDelay");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartCor(bool s)
    {
        StartCoroutine("StartDelay");
    }

    IEnumerator StartDelay()
    {
        float pauseTime = Time.realtimeSinceStartup + 3f;
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }

        if (gas.GetComponent<GameStatus>().tutorialPlayed == 1)
        {
            SceneManager.LoadScene("HomeMenu");
        }

        PlayerPrefs.SetInt("tutorialPlayed", 1);
        gas.GetComponent<GameStatus>().tutorialPlayed = 1;
        SceneManager.LoadScene("Tutorial1.1");

    }
}
