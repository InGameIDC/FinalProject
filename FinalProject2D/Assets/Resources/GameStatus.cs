using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public const string heroDBPath = "HerosDB";

    public float currentXP = 0f;
    public int coins = 200;
    public string lastScene = "HomeMenu";
    public string lastLevelCosen = "Test";
    public HeroContainer hc;

    // TODO: to delete after xml and images
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;

    // Start is called before the first frame update
    void Start()
    {
        hc = loadHeros(heroDBPath);

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

    public HeroContainer loadHeros(string path)
    {
        HeroContainer hc = HeroContainer.Load(path);
        Debug.Log(hc.heroesData);
        foreach(HeroData hero in hc.heroesData)
        {
            Debug.Log(hero.heroName);
        }

        return hc;
    }

}
