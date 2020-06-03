using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Author:OrS
/// this class is attached to every card in the collection
/// </summary>
public class HeroCard : MonoBehaviour
{
    public Action<int> onCardUse = delegate { };        //deligated function for when the use button is clicked

    public enum cardStatus { locked, opened, inUse};    //enum stating the stases of the card in deck

    public enum upgradeStatus { ready, notReady};       //enum stating the cards readiness for upgrade

    //-------Hero Data holders-------//
    public int heroId;                                  
    public int level;
    public int familyId;
    public int upgradeCost;

    public int partsForNextUpgrade;
    public int partsCollected;

    public cardStatus cardStat = cardStatus.opened;             
    public upgradeStatus upgradeStat = upgradeStatus.notReady;

    public bool cardShow = false;

    //-------Outside scripts and classes-------//
    private GameObject gs;                              //game status script - hold all the data for now
    private ChangeHero cm;                              //the change hero script
    private heroesToChoose htc;                         //the heroes to choose script

    //-------OutsideGameObjects connected in prefabs-------//
    public GameObject upgradeB;                         //upgrade button
    public GameObject useB;                             //use buttton
    public GameObject infoB;                            //info button
    public GameObject infoP;                            //info pannel
    public GameObject upgradeBar;                       //upgrade bar - shows the parts to upgrade progress
    public GameObject levelDisplay;                     //level text
    public GameObject upgradeText;                      //upgrade text on the upgrade bar
    public GameObject upgradeButtonText;                //the text on the upgrade button containg the cost of the upgrade
    public GameObject imageOutline;                     //image outline
    public Image profileImage;                          //the profile image



    private void Start()
    {
        //initiating the gameobjects for future use
        gs = GameObject.FindGameObjectWithTag("GameStatus");
        cm = GameObject.FindGameObjectWithTag("ChosenHeroPanel").GetComponent<ChangeHero>();
        htc = GameObject.FindGameObjectWithTag("HerosToChooseScript").GetComponent<heroesToChoose>();
        //infoP = GameObject.FindGameObjectWithTag("infoP");

        //connecting deligated functions
        cm.turnOffInUse += turnOffInUse;
        cm.finishChange += completeInUse;
        cm.inUseOutlineOn += turnOfOutline;

        //get information from XML
        loadHeroData(heroId);

        //updating the card texts
        upgradeText.GetComponent<TMPro.TextMeshProUGUI>().text = partsCollected + "/" + partsForNextUpgrade;
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;

        //changing the image of locked cards to be darker
        if (cardStat == cardStatus.locked)
        {
            profileImage.GetComponent<Image>().color = new Color32(120, 120, 120, 105); 
        }

        //if already ready to upgrade changing the upgrade button
        if(upgradeStat == upgradeStatus.ready)
        {
            upgradeButtonUpdate();
        }
    }

    /// <summary>
    /// Author: OrS
    /// when the user clicks on one of the hero cards this method opens/close buttons for the card according to it's status
    /// </summary>
    public void onClick()
    {
        //TODO: add a function that closes all the buttons of all the cards
        closeMenu();

        //cancel in use process if it was started and unfinished
        cm.inChangeProcess = false;
        cm.inUseOutlineOn(false);
        

        //show specific buttons
        if (!cardShow)      //need to show buttons
        {
            upgradeBar.SetActive(false);

            if (cardStat == cardStatus.locked)              //locked status
            {
                infoP.SetActive(true);
            }
            else if(cardStat == cardStatus.opened)          //open status
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                    
                    upgradeB.transform.localPosition = new Vector3(0f, -187.55f, 0);        //moving the upgrade button so it is shown properly
                    upgradeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = upgradeCost.ToString();
                    upgradeB.SetActive(true);

                }
                else
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);
                }
            }
            else                                            //inUse status
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, -137, 0f);           //moving the upgrade button so it is shown properly
                    upgradeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = upgradeCost.ToString();
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
        //TODO: will load all the data of the hero from the XMLs
        //need to change according to data
        level = 2;
        familyId = 1;

        switch (heroId)
        {
            case 1:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/BananaProfile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s1;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.ready;
                partsForNextUpgrade = 4;
                partsCollected = 4;
                upgradeCost = 200;
                break;

            case 2:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Grapes_Profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s2;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.ready;
                partsForNextUpgrade = 4;
                partsCollected = 4;
                upgradeCost = 400;
                break;

            case 3:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Lemon_profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s3;
                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 4;
                partsCollected = 2;
                upgradeCost = 400;
                break;

            case 4:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI\\PNG\\Watermelon_Profile") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s4;
                cardStat = cardStatus.opened;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 4;
                partsCollected = 3;
                upgradeCost = 200;
                break;

            default:
                //profileImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/UI/PNG/Brocoli_Profile") ;
                profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s5;
                cardStat = cardStatus.locked;
                upgradeStat = upgradeStatus.notReady;
                partsForNextUpgrade = 0;
                partsCollected = 0;
                upgradeCost = 500;
                break;
        }

        GetComponentInChildren<SimpleHealthBar>().UpdateBar(partsCollected, partsForNextUpgrade);
    }

    /// <summary>
    /// Author: OrS
    /// close all the buttons off and return the upgrade bar/ button (when ready to upgrade)
    /// </summary>
    public void closeMenu()
    {
        useB.SetActive(false);
        infoB.SetActive(false);
        infoP.SetActive(false);
        upgradeB.SetActive(false);
        upgradeBar.SetActive(true);
        GetComponentInChildren<SimpleHealthBar>().UpdateBar(partsCollected, partsForNextUpgrade);

        if (upgradeStat == upgradeStatus.ready)
        {
            upgradeButtonUpdate();
        }
    }

    #region Buttons related functions
    /// <summary>
    /// Author: OrS
    /// if the user clicks the use button this method will run
    /// the user will need to click on one other cards that are in use (at the top panel) to complete the transformation.
    /// </summary>
    public void onUse()
    {
        onClick();
        
        cm.inChangeId = heroId;
        cm.inChangeLevel = level;
        cm.inChangeFamily = familyId;
        cm.inChangeProcess = true;
        cm.inUseOutlineOn(true);
        imageOutline.SetActive(true);
    }

    /// <summary>
    /// Author: OrS
    /// upon the cm notifing that the replacement was done, this function complete the replacement on the card side
    /// </summary>
    /// <param name="id">the id of the hero that the is now copleted the replacement</param>
    private void completeInUse(int id)
    {
        if (heroId == id)
        {
            cardStat = cardStatus.inUse;
        }

        imageOutline.SetActive(false);
    }

    /// <summary>
    /// Author: OrS
    /// upon the cm notifing that a card was chosen to be replaced from the deck, this function changes that hero card status to open (instead of inUse)
    /// </summary>
    /// <param name="id"></param>
    private void turnOffInUse(int id)
    {
        if (id == heroId)
        {
            cardStat = cardStatus.opened;
        }
    }

    /// <summary>
    /// Author:OrS
    /// turn the outline of an hero card off
    /// </summary>
    /// <param name="turn"></param>
    private void turnOfOutline(bool turn)
    {
        if (!turn)
        {
            imageOutline.SetActive(false);
        }
    }

    /// <summary>
    /// Author: OrS
    /// if the user clicks the upgrade button this method will run
    /// </summary>
    public void onUpgrade()
    {
        //check if the upgrade is possible
        //TODO:change button appearence if it is'nt possible
        if (upgradeCost > gs.GetComponent<GameStatus>().coins)
        {
            Debug.Log("cant upgrade");
            return;
        }

        //update the data in the card and in the game status
        //TODO: update data in xml and reload the card
        level += 1;
        partsCollected = 0;
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;
        upgradeText.GetComponent<TMPro.TextMeshProUGUI>().text = partsCollected + "/" + partsForNextUpgrade;
        gs.GetComponent<GameStatus>().coins -= upgradeCost;

        upgradeStat = upgradeStatus.notReady;
        onClick();                      //closes the menus
        htc.updateBars();               //updates the coins, xp, and xpLevel displays and bars

        //if the hero is in the deck this function updates it as well
        if (cardStat == cardStatus.inUse)
        {
            cm.cardUpgrade(heroId, level);
        }

    }

    /// <summary>
    /// Author:OrS
    /// update the upgrade button according to cost, moving it so it is shown properly, and disable the upgrade bar
    /// </summary>
    private void upgradeButtonUpdate()
    {
        upgradeB.SetActive(true);
        upgradeB.transform.localPosition = new Vector3(0f, -82.4f, 0);
        upgradeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = upgradeCost.ToString();
        upgradeBar.SetActive(false);
    }

    public void infoPressed()
    {
        infoP.SetActive(true);
        infoP.GetComponent<CardInfoDisply>().updateInfoPanel(level, 1, 4, 3, partsForNextUpgrade, upgradeCost, profileImage.GetComponent<Image>().sprite); //TODO: get info from container
    }
    #endregion

}
