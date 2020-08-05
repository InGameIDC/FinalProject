using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttonCheck : MonoBehaviour
{
    public int levelnum;
    public GameObject gs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void backToMenu(bool fromButton)
    {
        if (fromButton)
        {
            gs.GetComponent<GameStatus>().starsInLevels[levelnum] = 0;
            gs.GetComponent<GameStatus>().levelsPlayed[levelnum] = 0;
        }
        
        SceneManager.LoadScene("HomeMenu");
    }

    public void FinishedStars(int numOfStars)
    {
        gs.GetComponent<GameStatus>().starsInLevels[levelnum] = numOfStars;
        gs.GetComponent<GameStatus>().levelsPlayed[levelnum] = 1;
        backToMenu(false);
    }
}
