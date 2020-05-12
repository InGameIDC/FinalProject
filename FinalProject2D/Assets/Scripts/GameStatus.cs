using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStatus : MonoBehaviour
{
    public Action<bool> GameStatusUpdate = delegate { };

    public float currentXP;
    public float xpToNextLevel;
    public int xpLevel;
    public int coins;
    public string lastScene;
    public int lastLevelCosen;
    public int isToLevel;

    // TODO: to delete after xml and images
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;

    public Sprite f1;
    public Sprite f2;
    public Sprite f3;

    // Start is called before the first frame update
    void Start()
    {
        //Load Data from playerprefs
        currentXP = PlayerPrefs.GetFloat("currentXP", 0);
        xpToNextLevel = PlayerPrefs.GetFloat("xpToNextLevel", 1000);
        xpLevel = PlayerPrefs.GetInt("xpLevel", 1);
        coins = PlayerPrefs.GetInt("coins", 0);
        lastScene = PlayerPrefs.GetString("lastScene", "HomeMenu");
        lastLevelCosen = PlayerPrefs.GetInt("lastLevelCosen", 3);
        isToLevel = PlayerPrefs.GetInt("isToLevel", 0);

        xpToNextLevel = xpLevel * 1000;

        //Debug.Log("upon waking: currentXP - " + currentXP + " xpToNextLevel - " + xpToNextLevel + " xpLevel - " + xpLevel + " coins - " + coins);

        GameStatusUpdate(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FromLevel(bool isGoingToLevel)
    {
        if (isGoingToLevel)
        {
            isToLevel = 1;
        }
        else
        {
            isToLevel = 0;
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("upon Destroy: currentXP - " + currentXP + " xpToNextLevel - " + xpToNextLevel + " xpLevel - " + xpLevel + " coins - " + coins);
        PlayerPrefs.SetFloat("currentXP", currentXP);
        PlayerPrefs.SetFloat("xpToNextLevel", xpToNextLevel);
        PlayerPrefs.SetInt("xpLevel", xpLevel);
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetString("lastScene", lastScene);
        PlayerPrefs.SetInt("lastLevelCosen", lastLevelCosen);
        PlayerPrefs.SetInt("isToLevel", isToLevel);
        
    }

}
