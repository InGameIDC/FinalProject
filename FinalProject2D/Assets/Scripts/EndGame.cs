using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: OrS
/// This class handles the end of a battle
/// </summary>
public class EndGame : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    public GameObject tc;                   //timerCountdown object
    public GameObject bm;                   //BattleManager object
    public GameObject gsObject;             //gamestatus object
    private GameStatus gs;

    public GameObject endPanel;             //the pannel to display
    public GameObject endDisplay;           //end Text (win/lose)
    public GameObject coinsDisplay;         //added coins text
    public GameObject xpDisplay;            //added xp text
    public GameObject chest;                //chest to open 
    public GameObject nextLevelButton;      //go to next level button
    public GameObject restartButton;        //restart the level button


    void Start()
    {
        //initiating the gameobjects for future use
        gs = gsObject.GetComponent<GameStatus>();


        Time.timeScale = 0f;                //stopping the game

        // check what was the reason for the battle to end - score reached, or time ended
        int currScore = (int)bm.GetComponent<BattleManager>().gameScore;
        if(tc.GetComponent<TimerCountdown>().secondsLeft <= 0)
        {
            endTimeGame(currScore);
        }
        else
        {
            endScoreGame(currScore);
        }
    }

    /// <summary>
    /// Author: OrS
    /// upon ending the game because of time, the score is checked to determine who won
    /// change the message according to the score balance
    /// </summary>
    /// <param name="score"> the score that was reached </param>
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

    /// <summary>
    /// Author: OrS
    /// upon ending the game because the score, it is checked to determine who won
    /// change the message according to the score balance
    /// </summary>
    /// <param name="score">the score that was reached</param>
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

    /// <summary>
    /// Author: OrS
    /// This method is changing the end pannel according to the senario that the player won
    /// </summary>
    private void wonMessage()
    {
        //TODO: update the message according to real xml data (coins, xp, chest...)

        endPanel.SetActive(true);
        endDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "You Won!";
        coinsDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "+400";
        xpDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "+1000";
        chest.SetActive(true);
        nextLevelButton.SetActive(true); 
        restartButton.SetActive(false);

        //update the game data
        gsObject.GetComponent<GameStatus>().coins += 400;
        gsObject.GetComponent<GameStatus>().currentXP += 1000;
    }

    /// <summary>
    /// Author: OrS
    /// This method is changing the end pannel according to the senario that the player lost
    /// </summary>
    private void lostMessage()
    {
        endPanel.SetActive(true);
        endDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lost!";
        coinsDisplay.SetActive(false);
        xpDisplay.SetActive(false);
        chest.SetActive(false);
        nextLevelButton.SetActive(false);
        restartButton.SetActive(true);
    }
}
