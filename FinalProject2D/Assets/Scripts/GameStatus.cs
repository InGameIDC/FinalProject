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
    public int[] deckPlayers = { 1, 2, 3 };
    public int[] starsInLevels = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] levelsPlayed = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] starsToUnlock = { 0, 0, 1, 2, 3, 5, 8, 11, 15, 20 };
    public int[] heroLevels = {1, 1, 1, 0, 0, 0 };
    public int[] enemyLevels = {0, 0, 0, 0, 0, 0 };

    // TODO: to delete after xml and images
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;
    public Sprite s6;


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

        deckPlayers[0] = PlayerPrefs.GetInt("player1", 1);
        deckPlayers[1] = PlayerPrefs.GetInt("player2", 2);
        deckPlayers[2] = PlayerPrefs.GetInt("player3", 3);

        //stars ernd in level
        starsInLevels[0] = PlayerPrefs.GetInt("starslevel1", 0);
        starsInLevels[1] = PlayerPrefs.GetInt("starslevel2", 0);
        starsInLevels[2] = PlayerPrefs.GetInt("starslevel3", 0);
        starsInLevels[3] = PlayerPrefs.GetInt("starslevel4", 0);
        starsInLevels[4] = PlayerPrefs.GetInt("starslevel5", 0);
        starsInLevels[5] = PlayerPrefs.GetInt("starslevel6", 0);
        starsInLevels[6] = PlayerPrefs.GetInt("starslevel7", 0);
        starsInLevels[7] = PlayerPrefs.GetInt("starslevel8", 0);
        starsInLevels[8] = PlayerPrefs.GetInt("starslevel9", 0);
        starsInLevels[9] = PlayerPrefs.GetInt("starslevel10", 0);

        //level was played
        levelsPlayed[0] = PlayerPrefs.GetInt("playedlevel1", 0);
        levelsPlayed[1] = PlayerPrefs.GetInt("playedlevel2", 0);
        levelsPlayed[2] = PlayerPrefs.GetInt("playedlevel3", 0);
        levelsPlayed[3] = PlayerPrefs.GetInt("playedlevel4", 0);
        levelsPlayed[4] = PlayerPrefs.GetInt("playedlevel5", 0);
        levelsPlayed[5] = PlayerPrefs.GetInt("playedlevel6", 0);
        levelsPlayed[6] = PlayerPrefs.GetInt("playedlevel7", 0);
        levelsPlayed[7] = PlayerPrefs.GetInt("playedlevel8", 0);
        levelsPlayed[8] = PlayerPrefs.GetInt("playedlevel9", 0);
        levelsPlayed[9] = PlayerPrefs.GetInt("playedlevel10", 0);

        //heros level
        heroLevels[0] = PlayerPrefs.GetInt("Hero_0_Level", 1); //apple
        heroLevels[1] = PlayerPrefs.GetInt("Hero_1_Level", 1); //grapes
        heroLevels[2] = PlayerPrefs.GetInt("Hero_2_Level", 1); //mango
        heroLevels[3] = PlayerPrefs.GetInt("Hero_3_Level", 0); //banana
        heroLevels[4] = PlayerPrefs.GetInt("Hero_4_Level", 0); //watermelon
        heroLevels[5] = PlayerPrefs.GetInt("Hero_5_Level", 0); //pineapple

        //enemy level
        enemyLevels[0] = PlayerPrefs.GetInt("Hero_100_Level", 1); //avocado
        enemyLevels[1] = PlayerPrefs.GetInt("Hero_101_Level", 1); //carrot
        enemyLevels[2] = PlayerPrefs.GetInt("Hero_102_Level", 1); //pea


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
        PlayerPrefs.SetInt("player1", deckPlayers[0]);
        PlayerPrefs.SetInt("player2", deckPlayers[1]);
        PlayerPrefs.SetInt("player3", deckPlayers[2]);

        PlayerPrefs.SetInt("starslevel1", starsInLevels[0]);
        PlayerPrefs.SetInt("starslevel2", starsInLevels[1]);
        PlayerPrefs.SetInt("starslevel3", starsInLevels[2]);
        PlayerPrefs.SetInt("starslevel4", starsInLevels[3]);
        PlayerPrefs.SetInt("starslevel5", starsInLevels[4]);
        PlayerPrefs.SetInt("starslevel6", starsInLevels[5]);
        PlayerPrefs.SetInt("starslevel7", starsInLevels[6]);
        PlayerPrefs.SetInt("starslevel8", starsInLevels[7]);
        PlayerPrefs.SetInt("starslevel9", starsInLevels[8]);
        PlayerPrefs.SetInt("starslevel10", starsInLevels[9]);
        
        PlayerPrefs.SetInt("playedlevel1", levelsPlayed[0]);
        PlayerPrefs.SetInt("playedlevel2", levelsPlayed[1]);
        PlayerPrefs.SetInt("playedlevel3", levelsPlayed[2]);
        PlayerPrefs.SetInt("playedlevel4", levelsPlayed[3]);
        PlayerPrefs.SetInt("playedlevel5", levelsPlayed[4]);
        PlayerPrefs.SetInt("playedlevel6", levelsPlayed[5]);
        PlayerPrefs.SetInt("playedlevel7", levelsPlayed[6]);
        PlayerPrefs.SetInt("playedlevel8", levelsPlayed[7]);
        PlayerPrefs.SetInt("playedlevel9", levelsPlayed[8]);
        PlayerPrefs.SetInt("playedlevel10", levelsPlayed[9]);

        PlayerPrefs.SetInt("Hero_0_Level", heroLevels[0]);
        PlayerPrefs.SetInt("Hero_1_Level", heroLevels[1]);
        PlayerPrefs.SetInt("Hero_2_Level", heroLevels[2]);
        PlayerPrefs.SetInt("Hero_3_Level", heroLevels[3]);
        PlayerPrefs.SetInt("Hero_4_Level", heroLevels[4]);
        PlayerPrefs.SetInt("Hero_5_Level", heroLevels[5]);

        PlayerPrefs.SetInt("Hero_100_Level", enemyLevels[0]);
        PlayerPrefs.SetInt("Hero_101_Level", enemyLevels[1]);
        PlayerPrefs.SetInt("Hero_102_Level", enemyLevels[2]);

    }

}
