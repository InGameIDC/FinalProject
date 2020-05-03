using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public float currentXP = 0f;
    public int coins = 200;
    public string lastScene = "HomeMenu";
    public string lastLevelCosen = "Test";

    // Start is called before the first frame update
    void Start()
    {
        //Load Data from playerprefs
        currentXP = PlayerPrefs.GetFloat("currentXP", 0);
        coins = PlayerPrefs.GetInt("coins", 0);
        lastScene = PlayerPrefs.GetString("lastScene", "HomeMenu");
        lastLevelCosen = PlayerPrefs.GetString("lastLevelCosen", "Test");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("currentXP", currentXP);
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetString("lastScene", lastScene);
        PlayerPrefs.SetString("lastLevelCosen", lastLevelCosen);
    }

}
