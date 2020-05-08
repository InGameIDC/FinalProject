using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject gsObject;
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

    private void UpdateBoard(bool needToUpdate)
    {
        xpLevel = gsObject.GetComponent<GameStatus>().xpLevel;
        currXp = gsObject.GetComponent<GameStatus>().currentXP;
        xpToNextLevel = gsObject.GetComponent<GameStatus>().xpToNextLevel;



        if (currXp >= xpToNextLevel)
        {
            xpLevel += 1;
            currXp = currXp - xpToNextLevel;
            xpToNextLevel = xpLevel * 1000;
        }

        Debug.Log("mainMenu stats: xpLevel = " + xpLevel + " currXp = " + currXp + " xpToNextLevel = " + xpToNextLevel);

        gsObject.GetComponent<GameStatus>().xpLevel = xpLevel;
        gsObject.GetComponent<GameStatus>().currentXP = currXp;
        gsObject.GetComponent<GameStatus>().xpToNextLevel = xpToNextLevel;

        coinsText.GetComponent<TMPro.TextMeshProUGUI>().text = gsObject.GetComponent<GameStatus>().coins.ToString();
        xpText.GetComponent<TMPro.TextMeshProUGUI>().text = currXp + "/" + xpToNextLevel;
        xpLevelText.GetComponent<TMPro.TextMeshProUGUI>().text = xpLevel.ToString();
    }
}
