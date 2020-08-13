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


    public LevelData levelData;
    public int levelId;
    public GameObject endPanel;             //the pannel to display
    public GameObject wonTitle;           //end Text (win/lose)
    public GameObject lostTitle;           //end Text (win/lose)
    public GameObject wonMes;           //end Text (win/lose)
    public GameObject lostMes;           //end Text (win/lose)
    public GameObject youGot;
    public GameObject youGotCoin;
    public GameObject youGotXp;
    public GameObject coinsDisplay;         //added coins text
    public GameObject xpDisplay;            //added xp text
    //public GameObject nextLevelButton;      //go to next level button
    public GameObject restartButton;        //restart the level button
    public GameObject goldStar1;
    public GameObject goldStar2;
    public GameObject goldStar3;


    void Start()
    {
        //initiating the gameobjects for future use
        gs = gsObject.GetComponent<GameStatus>();
        levelId = levelData.getlevelId();


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
            wonMessage(score);
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
            wonMessage(score);
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
    private void wonMessage(int score)
    {
        //TODO: update the message according to real xml data (coins, xp, chest...)

        endPanel.SetActive(true);
        wonTitle.SetActive(true);
        wonMes.SetActive(true);

        // Play Win sound
        SoundManager.Instance.PlaySound(Sound.Win);

        //xpDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "+1000";
        //nextLevelButton.SetActive(true); 
        restartButton.SetActive(false);

        gs.levelsPlayed[gs.lastLevelCosen - 1] = 1;
        int starsNum = 0;
        if(score < 75)
        {
            goldStar1.SetActive(true);
            starsNum = 1;
        }
        else if(score >= 75 && score < 90)
        {
            goldStar1.SetActive(true);
            goldStar2.SetActive(true);
            starsNum = 2;
        }
        else
        {
            goldStar1.SetActive(true);
            goldStar2.SetActive(true);
            goldStar3.SetActive(true);
            starsNum = 3;
        }

        //update the game data
        gs.starsInLevels[gs.lastLevelCosen - 1] = starsNum;
        gsObject.GetComponent<GameStatus>().coins += levelData.getCoinsReward(starsNum);

        coinsDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = levelData.getCoinsReward(starsNum).ToString();
        //gsObject.GetComponent<GameStatus>().currentXP += 1000;
    }

    /// <summary>
    /// Author: OrS
    /// This method is changing the end pannel according to the senario that the player lost
    /// </summary>
    private void lostMessage()
    {
        gs.levelsPlayed[gs.lastLevelCosen - 1] = 1;

        // Play Lose sound
        SoundManager.Instance.PlaySound(Sound.Lose);

        endPanel.SetActive(true);
        lostTitle.SetActive(true);
        lostMes.SetActive(true);
        coinsDisplay.SetActive(false);
        xpDisplay.SetActive(false);
        youGotCoin.SetActive(false);
        youGotXp.SetActive(false);
        youGot.SetActive(false);
        //nextLevelButton.SetActive(false);
        //restartButton.SetActive(true);
    }
}
