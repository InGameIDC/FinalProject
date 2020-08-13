using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameStatus : MonoBehaviour
{
    public GameObject gas;
    private GameStatus gs;
    // Start is called before the first frame update
    void Start()
    {
        gs = gas.GetComponent<GameStatus>();
    }

    // Update is called once per frame
    public void ResetGameStatusData(bool button)
    {
        gs.currentXP = 3;
        gs.xpToNextLevel = 0;
        gs.xpLevel = 3;
        gs.coins = 200;
        gs.lastScene = "HomeMenu";
        gs.lastLevelCosen = 1;
        gs.isToLevel = 0;
        gs.deckPlayers[0] = 0;
        gs.deckPlayers[1] = 1;
        gs.deckPlayers[2] = 2;
        gs.starsInLevels[0] = 0;
        gs.starsInLevels[1] = 0;
        gs.starsInLevels[2] = 0;
        gs.starsInLevels[3] = 0;
        gs.starsInLevels[4] = 0;
        gs.starsInLevels[5] = 0;
        gs.starsInLevels[6] = 0;
        gs.starsInLevels[7] = 0;
        gs.starsInLevels[8] = 0;
        gs.starsInLevels[9] = 0;
        gs.levelsPlayed[0] = 0;
        gs.levelsPlayed[1] = 0;
        gs.levelsPlayed[2] = 0;
        gs.levelsPlayed[3] = 0;
        gs.levelsPlayed[4] = 0;
        gs.levelsPlayed[5] = 0;
        gs.levelsPlayed[6] = 0;
        gs.levelsPlayed[7] = 0;
        gs.levelsPlayed[8] = 0;
        gs.levelsPlayed[9] = 0;
        gs.heroLevels[0] = 1;
        gs.heroLevels[1] = 1;
        gs.heroLevels[2] = 1;
        gs.heroLevels[3] = 0;
        gs.heroLevels[4] = 0;
        gs.heroLevels[5] = 0;

        gs.tutorialPlayed = 0;

        if (button)
        {
            SceneManager.LoadScene("OpeningScene");
        }
        else
        {
            SceneManager.LoadScene("HomeMenu");
        }
        
    }

    public void ResetGameStatusDataOpenScene()
    {
        gs.currentXP = 3;
        gs.xpToNextLevel = 0;
        gs.xpLevel = 3;
        gs.coins = 200;
        gs.lastScene = "HomeMenu";
        gs.lastLevelCosen = 1;
        gs.isToLevel = 0;
        gs.deckPlayers[0] = 0;
        gs.deckPlayers[1] = 1;
        gs.deckPlayers[2] = 2;
        gs.starsInLevels[0] = 0;
        gs.starsInLevels[1] = 0;
        gs.starsInLevels[2] = 0;
        gs.starsInLevels[3] = 0;
        gs.starsInLevels[4] = 0;
        gs.starsInLevels[5] = 0;
        gs.starsInLevels[6] = 0;
        gs.starsInLevels[7] = 0;
        gs.starsInLevels[8] = 0;
        gs.starsInLevels[9] = 0;
        gs.levelsPlayed[0] = 0;
        gs.levelsPlayed[1] = 0;
        gs.levelsPlayed[2] = 0;
        gs.levelsPlayed[3] = 0;
        gs.levelsPlayed[4] = 0;
        gs.levelsPlayed[5] = 0;
        gs.levelsPlayed[6] = 0;
        gs.levelsPlayed[7] = 0;
        gs.levelsPlayed[8] = 0;
        gs.levelsPlayed[9] = 0;
        gs.heroLevels[0] = 1;
        gs.heroLevels[1] = 1;
        gs.heroLevels[2] = 1;
        gs.heroLevels[3] = 0;
        gs.heroLevels[4] = 0;
        gs.heroLevels[5] = 0;

        gs.tutorialPlayed = 0;

    }
}
