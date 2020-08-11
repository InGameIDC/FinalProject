using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Author:OrS
/// this class is attached to every card in the collection
/// </summary>
public class HeroCardTutorial : MonoBehaviour
{
    public Action<int> onCardUse = delegate { };        //deligated function for when the use button is clicked

    public enum cardStatus { locked, opened, inUse, readyToBuy};    //enum stating the stases of the card in deck

    public enum upgradeStatus { ready, notReady};       //enum stating the cards readiness for upgrade

    //-------Hero Data holders-------//
    public int heroId;
    public GameObject hdm;
    private HeroData heroData;
    public int level;
    public int familyId;
    public int upgradeCost;

    //public int partsForNextUpgrade;
    //public int partsCollected;
    private int starsForUpgrade = 0;

    public cardStatus cardStat = cardStatus.opened;             
    public upgradeStatus upgradeStat = upgradeStatus.notReady;

    private Vector3 originalScale;

    public bool cardShow = false;

    //-------Outside scripts and classes-------//
    private GameObject gs;                              //game status script - hold all the data for now
    private ChangeHero cm;                              //the change hero script
    private heroesToChoose htc;                         //the heroes to choose script

    //-------OutsideGameObjects connected in prefabs-------//
    public GameObject upgradeB;                         //upgrade button
    public GameObject buyB;                             //upgrade button
    public GameObject useB;                             //use buttton
    public GameObject infoB;                            //info button
    public GameObject infoP;                            //info pannel
    public GameObject upgradeBar;                       //upgrade bar - shows the parts to upgrade progress
    public GameObject levelDisplay;                     //level text
    public GameObject upgradeText;                      //upgrade text on the upgrade bar
    public GameObject upgradeButtonText;                //the text on the upgrade button containg the cost of the upgrade
    public GameObject buyButtonText;                    //the text on the buy button containg the cost of the upgrade
    public GameObject imageOutline;                     //image outline
    public Image profileImage;                          //the profile image
    public Image familyImage;                          
    public GameObject levelText;                      
    public GameObject damageText;                
    public GameObject healthText;



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


        updateDisplay();
    }

    public void updateDisplay()
    {
        //get information from XML
        loadHeroData(heroId);

        //updating the card texts
        //upgradeText.GetComponent<TMPro.TextMeshProUGUI>().text = partsCollected + "/" + partsForNextUpgrade;
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;

        //changing the image of locked cards to be darker
        if (cardStat == cardStatus.locked)
        {
            profileImage.GetComponent<Image>().color = new Color32(120, 120, 120, 105);
        }
        else if (cardStat == cardStatus.readyToBuy)
        {
            profileImage.GetComponent<Image>().color = new Color32(120, 120, 120, 105);
            buyB.SetActive(true);
            buyButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = heroData.getUpgradeCost().ToString();
        }

        //if already ready to upgrade changing the upgrade button
        if (upgradeStat == upgradeStatus.ready)
        {
            upgradeButtonUpdate();
        }

        originalScale = gameObject.transform.localScale;
    }

    /// <summary>
    /// Author: OrS
    /// when the user clicks on one of the hero cards this method opens/close buttons for the card according to it's status
    /// </summary>
    public void onClick()
    {
        //TODO: add a function that closes all the buttons of all the cards
        closeMenu();

        //returning buttons to their place
        infoB.transform.localPosition = new Vector3(0f, -115, 0);        //moving the upgrade button so it is shown properly
        upgradeB.transform.localPosition = new Vector3(0f, -115, 0);

        //cancel in use process if it was started and unfinished
        cm.inChangeProcess = false;
        cm.inUseOutlineOn(false);
        

        //show specific buttons
        if (!cardShow)      //need to show buttons
        {
            upgradeBar.SetActive(false);

            if (cardStat == cardStatus.locked || cardStat == cardStatus.readyToBuy)              //locked status
            {
                //infoP.SetActive(true);
                infoB.SetActive(true);
                if (cardStat == cardStatus.readyToBuy)
                {
                    buyB.SetActive(true);
                    buyButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = heroData.getUpgradeCost().ToString();
                }
            }
            else if(cardStat == cardStatus.opened)          //open status
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    useB.SetActive(true);

                    infoB.transform.localPosition = new Vector3(0f, -206, 0);        //moving the upgrade button so it is shown properly
                    upgradeB.transform.localPosition = new Vector3(0f, -298, 0);        //moving the upgrade button so it is shown properly
                    upgradeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = upgradeCost.ToString();
                    upgradeB.SetActive(true);

                }
                else
                {
                    infoB.transform.localPosition = new Vector3(0f, -206, 0);        //moving the upgrade button so it is shown properly
                    infoB.SetActive(true);
                    useB.SetActive(true);
                }
            }
            else                                            //inUse status
            {
                if(upgradeStat == upgradeStatus.ready)
                {
                    infoB.SetActive(true);
                    upgradeB.transform.localPosition = new Vector3(0f, -206, 0f);           //moving the upgrade button so it is shown properly
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
        
        //levelText.GetComponent<TMPro.TextMeshProUGUI>().text = level.ToString();

        //heroData = GetComponent<HeroDataManage>().GetData();
        //healthText.GetComponent<TMPro.TextMeshProUGUI>().text = heroData.getMaxHealth().ToString();
        //damageText.GetComponent<TMPro.TextMeshProUGUI>().text = heroData.getDamage().ToString();
        //upgradeCost = heroData.getUpgradeCost();

/*        switch (heroData.getCardStatus())
        {
            case 1:
                cardStat = cardStatus.opened;
                break;
            case 2:
                cardStat = cardStatus.inUse;
                break;
            case 3:
                cardStat = cardStatus.readyToBuy;
                break;
            default:
                cardStat = cardStatus.locked;
                break;

        }*/

/*        switch (heroData.getUpgradestatus())
        {
            case 1:
                upgradeStat = upgradeStatus.ready;
                break;

            default:
                upgradeStat = upgradeStatus.notReady;
                break;

        }*/

        /*if(cardStat == cardStatus.locked && heroData.getStarsToBuy()<= gs.GetComponent<GameStatus>().xpLevel)  //check if ready to buy
        {
            cardStat = cardStatus.readyToBuy;
            heroData.setCardStatus(3);
            buyB.SetActive(true);
            buyButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = heroData.getUpgradeCost().ToString();
        }

        if(upgradeStat == upgradeStatus.notReady && heroData.getStarsToUpgrade() <= gs.GetComponent<GameStatus>().xpLevel) //check if ready to upgrade
        {
            upgradeStat = upgradeStatus.ready;
            heroData.setUpgradestatus(1);
            upgradeButtonUpdate();
        }*/

        switch (heroId)
        {
            case 0:
                level = 1;
                //profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s1;
                healthText.GetComponent<TMPro.TextMeshProUGUI>().text = "5";
                damageText.GetComponent<TMPro.TextMeshProUGUI>().text = "4";
                upgradeCost = 200;
                familyId = 1;
                //familyImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().f1;

                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.ready;
                break;

            case 1:
                level = 1;
                //profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s2;
                healthText.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
                damageText.GetComponent<TMPro.TextMeshProUGUI>().text = "6";
                upgradeCost = 300;
                familyId = 1;
                //familyImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().f1;

                cardStat = cardStatus.inUse;
                upgradeStat = upgradeStatus.notReady;
                break;

            default:
                level = 1;
                //profileImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().s3;
                healthText.GetComponent<TMPro.TextMeshProUGUI>().text = "6";
                damageText.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
                upgradeCost = 400;
                familyId = 2;
                //familyImage.GetComponent<Image>().sprite = gs.GetComponent<GameStatus>().f2;

                cardStat = cardStatus.opened;
                upgradeStat = upgradeStatus.notReady;
                break;
        }

        /*int[] playersList = gs.GetComponent<GameStatus>().deckPlayers;
        // marking in use cards as in use
        for(int i = 0; i < 3; i++)
        {
            if (heroId == playersList[i])
            {
                cardStat = cardStatus.inUse;
                heroData.setCardStatus(2);
            }
        }*/

    }

    /// <summary>
    /// Author: OrS
    /// close all the buttons off and return the upgrade bar/ button (when ready to upgrade)
    /// </summary>
    public void closeMenu()
    {
        //gameObject.transform.localScale = originalScale;
        useB.SetActive(false);
        infoB.SetActive(false);
        infoP.SetActive(false);
        upgradeB.SetActive(false);
        //upgradeBar.SetActive(true);
        //GetComponentInChildren<SimpleHealthBar>().UpdateBar(partsCollected, partsForNextUpgrade);

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
            heroData.setCardStatus(2);
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
            heroData.setCardStatus(1);
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
        gs.GetComponent<GameStatus>().heroLevels[heroId] += 1;
        level = gs.GetComponent<GameStatus>().heroLevels[heroId];
        levelText.GetComponent<TMPro.TextMeshProUGUI>().text = level.ToString();
        //partsCollected = 0;
        
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;
        //starsForUpgrade = heroData.getStarsToUpgrade();
        //upgradeText.GetComponent<TMPro.TextMeshProUGUI>().text = partsCollected + "/" + partsForNextUpgrade;
        gs.GetComponent<GameStatus>().coins -= upgradeCost;
        gs.GetComponent<GameStatus>().xpLevel += 1;

        if (heroData.getStarsToUpgrade() <= gs.GetComponent<GameStatus>().xpLevel)
        {
            upgradeStat = upgradeStatus.ready;
            heroData.setUpgradestatus(1);
            upgradeButtonUpdate();
        }
        else
        {
            upgradeStat = upgradeStatus.notReady;
            heroData.setUpgradestatus(0);
        }

        onClick();                      //closes the menus
        htc.updateBars();               //updates the coins, xp, and xpLevel displays and bars

        //if the hero is in the deck this function updates it as well
        if (cardStat == cardStatus.inUse)
        {
            cm.cardUpgrade(heroId, level);
        }

        htc.updateCardsdisplay();

    }

    /// <summary>
    /// Author:OrS
    /// update the upgrade button according to cost, moving it so it is shown properly, and disable the upgrade bar
    /// </summary>
    private void upgradeButtonUpdate()
    {
        upgradeB.SetActive(true);
        upgradeB.transform.localPosition = new Vector3(0f, -115, 0);
        upgradeCost = heroData.getUpgradeCost();
        upgradeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = upgradeCost.ToString();
        upgradeBar.SetActive(false);
    }

    public void infoPressed()
    {
        infoP.SetActive(true);
        infoP.GetComponent<CardInfoDisply>().updateInfoPanel(level, heroData.getDamage(), heroData.getMaxHealth(), heroData.getMovementSpeed(), heroData.getStarsToUpgrade(), upgradeCost, profileImage.GetComponent<Image>().sprite); //TODO: get info from container
    }

    public void BuyPressed()
    {
        if (upgradeCost > gs.GetComponent<GameStatus>().coins)
        {
            Debug.Log("cant upgrade");
            return;
        }

        profileImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buyB.SetActive(false);

        cardStat = cardStatus.opened;
        heroData.setCardStatus(1);

        gs.GetComponent<GameStatus>().heroLevels[heroId] += 1;
        level = gs.GetComponent<GameStatus>().heroLevels[heroId];
        levelText.GetComponent<TMPro.TextMeshProUGUI>().text = level.ToString();

        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;

        gs.GetComponent<GameStatus>().coins -= upgradeCost;
        gs.GetComponent<GameStatus>().xpLevel += 1;

        if (heroData.getStarsToUpgrade() <= gs.GetComponent<GameStatus>().xpLevel)
        {
            upgradeStat = upgradeStatus.ready;
            heroData.setUpgradestatus(1);
            upgradeButtonUpdate();
        }
        else
        {
            upgradeStat = upgradeStatus.notReady;
            heroData.setUpgradestatus(0);
        }
        
        onClick();                      //closes the menus
        htc.updateBars();
        htc.updateCardsdisplay();

    }

    #endregion

}
