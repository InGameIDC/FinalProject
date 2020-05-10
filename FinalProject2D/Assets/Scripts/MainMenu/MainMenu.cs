using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Author:OrS
/// This class is responsible on the UI of the Home Menu (not including the levels buttons and pause button)
/// it is attached to the canvas
/// </summary>
public class MainMenu : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    public GameObject gsObject;

    //-------Outside GameObjects------//
    public GameObject coinsText;
    public GameObject xpText;
    public GameObject xpLevelText;

    private int xpLevel;
    private float currXp;
    private float xpToNextLevel;

    private void Start()
    {
        gsObject.GetComponent<GameStatus>().GameStatusUpdate += UpdateBoard;

    }

    /// <summary>
    /// Author:OrS
    /// update the UI according to the game status data
    /// </summary>
    /// <param name="needToUpdate"></param>
    private void UpdateBoard(bool needToUpdate)
    {
        //gets the data from the game status script
        xpLevel = gsObject.GetComponent<GameStatus>().xpLevel;
        currXp = gsObject.GetComponent<GameStatus>().currentXP;
        xpToNextLevel = gsObject.GetComponent<GameStatus>().xpToNextLevel;

        // if the xp has reached the goal, the level changes
        if (currXp >= xpToNextLevel)
        {
            xpLevel += 1;
            currXp = currXp - xpToNextLevel;
            xpToNextLevel = xpLevel * 1000;
        }

        //Debug.Log("mainMenu stats: xpLevel = " + xpLevel + " currXp = " + currXp + " xpToNextLevel = " + xpToNextLevel);

        // update the data in the game status
        gsObject.GetComponent<GameStatus>().xpLevel = xpLevel;
        gsObject.GetComponent<GameStatus>().currentXP = currXp;
        gsObject.GetComponent<GameStatus>().xpToNextLevel = xpToNextLevel;

        //update the display
        coinsText.GetComponent<TMPro.TextMeshProUGUI>().text = gsObject.GetComponent<GameStatus>().coins.ToString();
        xpText.GetComponent<TMPro.TextMeshProUGUI>().text = currXp + "/" + xpToNextLevel;
        xpLevelText.GetComponent<TMPro.TextMeshProUGUI>().text = xpLevel.ToString();
    }
}
