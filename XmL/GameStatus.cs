using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using SaveSystem;

public class GameStatus : MonoBehaviour
{
[XmlElement("GameStatus")]

    public float currentXP;
    public int coins;
    public string lastScene;
    public int lastLevelChosen;
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    public Sprite s5;


    public GameStatus(float currentXP, int coins, string lastScene, int lastLevelChosen)
    {
    }
    public GameStatus() { }

     void create_player_status()
    {
        GameStatus player = new GameStatus(0f,200, "HomeMenu", 1);
        FileSave fileSave = new FileSave(FileFormat.Xml);
        fileSave.WriteToFile(Application.persistentDataPath + "/player_status.xml", player);
    }

    void change_player_status (float currentXP, int coins, string lastScene, int lastLevelChosen)
    {
        GameStatus player = new GameStatus(0f, 0, "first", 1);
        FileSave fileSave = new FileSave(FileFormat.Xml);
        fileSave.WriteToFile(Application.persistentDataPath + "/player_status.xml", player);
    }
    public GameStatus load_player_stats()
    {
        FileSave fileSave = new FileSave(FileFormat.Xml);
        GameStatus player = fileSave.ReadFromFile<GameStatus>(Application.persistentDataPath + "/player_status.xml");
        Debug.Log("Loaded data: " + player);
        return player;
    }
    private void Awake()
    {
        create_player_status();
        Debug.Log("new player status created");
    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("currentXP", currentXP);
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetString("lastScene", lastScene);
        PlayerPrefs.SetInt("lastLevelCosen", lastLevelChosen);
    }
    void Update()
    {

    }
    void Start()
    {
        //Load Data from xml
        GameStatus gameStatus = load_player_stats();
        currentXP = gameStatus.currentXP;
        coins = gameStatus.coins;
        lastScene = gameStatus.lastScene;
        lastLevelChosen = gameStatus.lastLevelChosen;

    }


}






