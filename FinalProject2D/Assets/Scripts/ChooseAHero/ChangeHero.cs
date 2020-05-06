using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeHero : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;

    public int inChangeId;
    public int inChangeLevel;
    public int inChangeFamily;
    public bool inChangeProcess = false;

    public Action<int> turnOffInUse = delegate { };
    public Action<int> finishChange = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        LoadTeam();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTeam()
    {
        //load the team from the xml
        card1 = GameObject.FindGameObjectWithTag("HeroCard1");
        card2 = GameObject.FindGameObjectWithTag("HeroCard2");
        card3 = GameObject.FindGameObjectWithTag("HeroCard3");
    }

    public void inChange(int id, int level, int family)
    {
        inChangeProcess = true;
        inChangeLevel = level;
        inChangeFamily = family;
        inChangeId = id;
    }
    public void finishChosenChange(int id)
    {
        finishChange(id);
    }

    public void notInUse(int id)
    {
        turnOffInUse(id);
    }

    public void cardUpgrade(int id, int newLevel)
    {
        if(card1.GetComponent<ChosenHeroCard>().heroId == id)
        {
            //ToDo - reload the card
            card1.GetComponent<ChosenHeroCard>().level = newLevel;
            card1.GetComponent<ChosenHeroCard>().updateLevelDisplay(newLevel);
        }

        if (card2.GetComponent<ChosenHeroCard>().heroId == id)
        {
            //ToDo - reload the card
            card2.GetComponent<ChosenHeroCard>().level = newLevel;
            card2.GetComponent<ChosenHeroCard>().updateLevelDisplay(newLevel);
        }

        if (card3.GetComponent<ChosenHeroCard>().heroId == id)
        {
            //ToDo - reload the card
            card3.GetComponent<ChosenHeroCard>().level = newLevel;
            card3.GetComponent<ChosenHeroCard>().updateLevelDisplay(newLevel);
        }

    }
}
