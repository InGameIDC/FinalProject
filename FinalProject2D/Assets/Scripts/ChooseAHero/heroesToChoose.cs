using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class heroesToChoose : MonoBehaviour
{

    GameObject gs;
    public string levelToPlay;
    GameObject[] cards;


    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        cards = GameObject.FindGameObjectsWithTag("GeneralHeroCard");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(gs.GetComponent<GameStatus>().lastLevelCosen);
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
}
