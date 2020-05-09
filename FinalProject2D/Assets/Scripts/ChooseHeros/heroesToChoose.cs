using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class heroesToChoose : MonoBehaviour
{

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

    


    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        cards = GameObject.FindGameObjectsWithTag("GeneralHeroCard");

        if(gs.GetComponent<GameStatus>().isToLevel == 0)
        {
            goButton.SetActive(false);
        }

        updateBars();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(gs.GetComponent<GameStatus>().lastLevelCosen);
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("HomeMenu");
    }

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
