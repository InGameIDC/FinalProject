using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: OrS
/// this class is responsible for all UI in the choose a hero scene
/// it is connected to an empty object with the same name
/// </summary>
public class heroesToChoose : MonoBehaviour
{
    //-------Outside scripts and classes-------//
    GameObject gs;
    
    
    public string levelToPlay;
    GameObject[] cards;

    public GameObject coinsText;
    private int coins;
    public GameObject xpText;
    private float currXp;
    public GameObject xpLevelText;
    private int xpLevel;

    public GameObject goButton;
    public GameObject backButton;


    void Start()
    {
        //initiating the gameobjects for future use
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        cards = GameObject.FindGameObjectsWithTag("GeneralHeroCard");

        //if the player got to this scene through clicking the collection button, the goButton needs to be inactive
        if(gs.GetComponent<GameStatus>().isToLevel == 0)
        {
            goButton.SetActive(false);
        }

        //updates the bars of the coins and the xp at the top
        updateBars();

    }

    /// <summary>
    /// Author: OrS
    /// upon clicking go, this method loads the level the player wanted to play
    /// </summary>
    public void GoToLevel()
    {
        SceneManager.LoadScene(gs.GetComponent<GameStatus>().lastLevelCosen);
    }

    /// <summary>
    /// Author: OrS
    /// upon clicking the back button, this method loads the home menu scene
    /// </summary>
    public void backToMainMenu()
    {
        SceneManager.LoadScene("HomeMenu");
    }

    /// <summary>
    /// Author:OrS
    /// if the player clicks not on the cards the buttons of the cards that were opened will be closed
    /// TODO: change it to every click - as long as its not onn the card itself
    /// </summary>
    public void onClick()
    {
        //close all
        
        Debug.Log("Close all");
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<HeroCard>().cardShow)
            {
                cards[i].GetComponent<HeroCard>().closeMenu();
            }
        }
        
    }

    /// <summary>
    /// Author: OrS
    /// This functin updates the bars at the top of the screen (coins and xp)
    /// </summary>
    public void updateBars()
    {
        coins = gs.GetComponent<GameStatus>().coins;
        currXp = gs.GetComponent<GameStatus>().currentXP;
        xpLevel = gs.GetComponent<GameStatus>().xpLevel;

        coinsText.GetComponent<TMPro.TextMeshProUGUI>().text = coins.ToString();
        xpText.GetComponent<TMPro.TextMeshProUGUI>().text = currXp + "/" + gs.GetComponent<GameStatus>().xpToNextLevel;
        xpLevelText.GetComponent<TMPro.TextMeshProUGUI>().text = xpLevel.ToString();
    }
}
