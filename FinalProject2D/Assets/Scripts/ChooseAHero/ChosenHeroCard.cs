using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosenHeroCard : MonoBehaviour
{
    public int heroId;
    public int level;
    public int familyId;

    public Image profileImage;
    public GameObject levelDisplay;

    private GameStatus gs;
    private ChangeHero cm;


    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameStatus").GetComponent<GameStatus>();
        cm = GetComponentInParent<ChangeHero>();
        LoadChosenHeroCard(heroId);
        updateLevelDisplay(level);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadChosenHeroCard(int id)
    {
        heroId = id;
        level = 2;
        familyId = 1;
        switch (heroId)
        {
            case 1:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/BananaProfile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s1;
                break;

            case 2:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Grapes_Profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s2;
                break;

            case 3:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI/PNG/Lemon_profile.jpg") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s3;
                break;

            case 4:
                //profileImage.GetComponent<Image>().sprite = Resources.Load("UI\\PNG\\Watermelon_Profile") as Sprite;
                profileImage.GetComponent<Image>().sprite = gs.s4;
                break;

            default:
                //profileImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/UI/PNG/Brocoli_Profile") ;
                profileImage.GetComponent<Image>().sprite = gs.s5;
                break;
        }
    }
    
    public void onClick()
    {
        if (cm.inChangeProcess)
        {
            cm.turnOffInUse(heroId);

            level = cm.inChangeLevel;
            familyId = cm.inChangeFamily;
            LoadChosenHeroCard(cm.inChangeId);
            cm.inChangeProcess = false;
            updateLevelDisplay(level);
            cm.finishChange(heroId);
        }
    }

    public void updateLevelDisplay(int level)
    {
        levelDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + level;
    }
}
