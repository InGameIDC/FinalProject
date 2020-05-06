using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeroCard : MonoBehaviour
{
    public Action<int> onCardUse = delegate { };
    public enum cardStatus { locked, opened, inUse};
    public enum upgradeStatus { ready, notReady};

    public int heroId;

    private GameStatus gs;
    private ChangeHero cm;
    private heroesToChoose htc;

    public cardStatus cardStat = cardStatus.opened;
    public upgradeStatus upgradeStat = upgradeStatus.notReady;
    public bool cardShow = false;
    public GameObject upgradeB;
    public GameObject useB;
    public GameObject infoB;
    public GameObject infoP;
    public GameObject upgradeBar;
    public GameObject levelDisplay;
    public Image profileImage;

    public int partsForNextUpgrade;
    public int partsCollected;

    public int level;
    public int familyId;


    private void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus").GetComponent<GameStatus>();
        cm = GameObject.FindGameObjectWithTag("ChosenHeroPanel").GetComponent<ChangeHero>();
        htc = GameObject.FindGameObjectWithTag("HerosToChooseScript").GetComponent<heroesToChoose>(); 

        cm.turnOffInUse += turnOffInUse;
        cm.finishChange += completeInUse;
        //get information from XML
        loadHeroData(heroId);
        

        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;

        if (cardStat == cardStatus.locked)
        {
            profileImage.GetComponent<Image>().color = new Color32(120, 120, 120, 105); 
        }
        
    }

    /// <summary>
    /// Author: OrS
    /// when the user clicks on one of the hero cards this method opens/close buttons for the card according to it's status
    /// </summary>
    public void onClick()
    {
        //first we close all cards menus
        //htc.onClick();
        Debug.Log(name);

        //cancel in use process if it was started and unfinished
        cm.inChangeProcess = false;

        //show specific buttons
        if (!cardShow)      //need to show buttons
        {
            upgradeBar.SetActive(false);

            if (cardStat == cardStatus.locked)
            {
                infoP.SetActive(true);
            }
            else if(cardStat == cardStatus.opened)
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, -177.2f, 0);
                    upgradeB.SetActive(true);

                }
                else
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                }
            }
            else        //inUse
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, useB.transform.localPosition.y, 0f);
                    upgradeB.SetActive(true);
                }
                else
                {
                    infoB.SetActive(true);
                }
            }

            cardShow = true;
        }
        else                //need to close buttons
        {
            closeMenu();
            cardShow = false;
        }
    }

    /// <summary>
    /// Author:OrS
    /// function will load the data of the card from the xml
    /// </summary>
    /// <param name="heroId">the ID of the hero</param>
    public void loadHeroData(int heroId)
    {
        //will load all the data of the hero from the XMLs
        //need to change according to data
        level = 2;
        familyId = 1;

        switch (heroId)
        {
            case 1:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/BananaProfile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s1;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.ready;
                partsForNextUpgrade = 4;
                partsCollected = 4;
                break;

            case 2:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Grapes_Profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s2;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.ready;
                partsForNextUpgrade = 4;
                partsCollected = 4;
                break;

            case 3:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Lemon_profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s3;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 4;
                partsCollected = 2;
                break;

            case 4:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI\\PNG\\Watermelon_Profile") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s4;
                cardStat = cardStatus.opened;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 4;
                partsCollected = 3;
                break;

            default:
                //profileImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/UI/PNG/Brocoli_Profile") ;
                profileImage.GetComponent<Image>().sprite = gs.s5;
                cardStat = cardStatus.locked;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 0;
                partsCollected = 0;
                break;
        }
        GetComponentInChildren<SimpleHealthBar>().UpdateBar(partsCollected, partsForNextUpgrade);

    }

    /// <summary>
    /// Author: OrS
    /// if the user clicks the use button this method will run
    /// the user will need to click on one other cards that are in use (at the top panel) to complete the transformation.
    /// </summary>
    public void onUse()
    {
        //cardStat = cardStatus.inUse;
        onClick();
        cm.inChangeId = heroId;
        cm.inChangeLevel = level;
        cm.inChangeFamily = familyId;
        cm.inChangeProcess = true;
    }

    public void closeMenu()
    {
        useB.SetActive(false);
        infoB.SetActive(false);
        infoP.SetActive(false);
        upgradeB.SetActive(false);
        upgradeBar.SetActive(true);
        GetComponentInChildren<SimpleHealthBar>().UpdateBar(partsCollected, partsForNextUpgrade);
    }

    private void completeInUse(int id)
    {
        if(heroId == id)
        {
            cardStat = cardStatus.inUse;
        }
    }

    public void onUpgrade()
    {
        //ToDo - update data in xml and reload the card
        level += 1;
        partsCollected = 0;
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;

        upgradeStat = upgradeStatus.notReady;
        onClick();

        if(cardStat == cardStatus.inUse)
        {
            cm.cardUpgrade(heroId, level);
        }

    }

    private void turnOffInUse(int id)
    {
        if(id == heroId)
        {
            cardStat = cardStatus.opened;
        }
    }
}
