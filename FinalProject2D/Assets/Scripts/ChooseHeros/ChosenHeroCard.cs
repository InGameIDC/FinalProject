using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Author: OrS
/// thsi class is attached do every card that is in the chosen deck
/// </summary>
public class ChosenHeroCard : MonoBehaviour
{
    public int heroId;                  //the hero Id of the hero in the card
    public int level;                   //the level of the hero
    public int familyId;                //what family the hero blongs
    public int cardNumber;

    public Image profileImage;          //the profile image of the hero - connected in prefab
    public Image familyImage;          //the profile image of the hero - connected in prefab
    public GameObject levelDisplay;     //the level text on the card - connected in prefab
    public GameObject imageOutline;     //the image imitating an outline when need to choose a hero to switch - connected in prefab

    private GameStatus gs;              //game status script - hold all the data for now
    private ChangeHero cm;              //the change hero script


    void Start()
    {
        //initiating the gameobjects for future use
        gs = GameObject.FindGameObjectWithTag("GameStatus").GetComponent<GameStatus>();
        cm = GetComponentInParent<ChangeHero>();

        //loadin the default hero data
        LoadChosenHeroCard(gs.deckPlayers[cardNumber - 1]);
        updateLevelDisplay(level);

        //connecting deligated functions 
        cm.inUseOutlineOn += startChangeOutline;
        cm.cardUpgrade += cardWasUpgraded;


    }

    /// <summary>
    /// Author: OrS
    /// loads the data of the hero to the card
    /// TODO: change to xml data
    /// </summary>
    /// <param name="id">the heroId</param>
    public void LoadChosenHeroCard(int id)
    {
        // all the information is hardcoded until xml data is integrated
        heroId = id;
        level = 2;
        familyId = 1;
        switch (heroId)
        {
            case 0:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/BananaProfile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s1;
                familyImage.GetComponent<Image>().sprite = gs.f1;
                break;

            case 1:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Grapes_Profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s2;
                familyImage.GetComponent<Image>().sprite = gs.f2;
                break;

            case 2:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Lemon_profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s3;
                familyImage.GetComponent<Image>().sprite = gs.f1;
                break;

            case 3:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI\\PNG\\Watermelon_Profile") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s4;
                familyImage.GetComponent<Image>().sprite = gs.f1;
                break;
            case 4:
                profileImage.GetComponent<Image>().sprite = gs.s5;
                familyImage.GetComponent<Image>().sprite = gs.f1;
                break;

            default:
                //profileImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/UI/PNG/Brocoli_Profile") ;
                profileImage.GetComponent<Image>().sprite = gs.s6;
                familyImage.GetComponent<Image>().sprite = gs.f2;
                break;
        }
    }
    
    /// <summary>
    /// Author: OrS
    /// upon clicking on the card - controlled by the card button component
    /// </summary>
    public void onClick()
    {
        if (cm.inChangeProcess)     //if the user is in the process of changing the cards in the chosen deck
        {
            // cm deligated - message the hero that is replaced FROM the deck to open status (instead of inUse)
            cm.turnOffInUse(heroId);

            //updating data of new hero in the card
            level = cm.inChangeLevel;
            familyId = cm.inChangeFamily;
            LoadChosenHeroCard(cm.inChangeId);
            updateLevelDisplay(level);

            //after loading change the status in cm
            cm.inChangeProcess = false;

            // cm deligated - notifing that the change was done and ned to turn off outlines
            cm.finishChange(heroId);
            cm.inUseOutlineOn(false);

            gs.deckPlayers[cardNumber] = heroId;
            
        }
    }

    /// <summary>
    /// Author: OrS
    /// update the level text on the card
    /// </summary>
    /// <param name="level">the level the text should show</param>
    public void updateLevelDisplay(int level)
    {
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;
    }

    /// <summary>
    /// Author:OrS
    /// turn on and off the outlines
    /// </summary>
    /// <param name="needOutline"></param>
    private void startChangeOutline(bool needOutline)
    {
        if (needOutline)
        {
            imageOutline.SetActive(true);
        }
        else
        {
            imageOutline.SetActive(false);
        }
    }

    /// <summary>
    /// Author:OrS
    /// when a card in the deck was upgraded, this method is called and updates the level of that card 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newLevel"></param>
    private void cardWasUpgraded(int id, int newLevel)
    {
        if (heroId == id)
        {
            //ToDo - reload the card
            level = newLevel;
            updateLevelDisplay(newLevel);
        }
    }
}
