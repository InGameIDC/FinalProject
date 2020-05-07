using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject tc;          //timerCountdown object
    public GameObject bm;          //BattleManager object
    public GameObject endPanel;    //the pannel to display

    public GameObject endDisplay;
    public GameObject coinsDisplay;
    public GameObject xpDisplay;
    public GameObject Chest;
    public GameObject nextLevelButton;
    public GameObject restartButton;


    // Start is called before the first frame update
    void Start()
    {
        int currScore = (int)bm.GetComponent<BattleManager>().gameScore;
        Time.timeScale = 0f;
        if(tc.GetComponent<TimerCountdown>().secondsLeft <= 0)
        {
            endTimeGame(currScore);
        }
        else
        {
            endScoreGame(currScore);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void endTimeGame(int score)
    {
        

        if (score >= 0)
        {
            wonMessage();
        }
        else
        {
            lostMessage();
        }
    }

    private void endScoreGame(int score)
    {

        if (score >= 0)
        {
            wonMessage();
        }
        else
        {
            lostMessage();
        }
    }

    private void wonMessage()
    {
        endPanel.SetActive(true);
        endDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "You Won!";
        coinsDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "+400";
        xpDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "+1000";
        Chest.SetActive(true);
        nextLevelButton.SetActive(true); 
        restartButton.SetActive(false);
    }

    private void lostMessage()
    {
        endPanel.SetActive(true);
        endDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lost!";
        coinsDisplay.SetActive(false);
        xpDisplay.SetActive(false);
        Chest.SetActive(false);
        nextLevelButton.SetActive(false);
        restartButton.SetActive(true);
    }
}
