using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;
using UnityEditor;

public class heros_Container : MonoBehaviour
{
    // Start is called before the first frame update
    public hero[] heros_Container_tmp;
    void Start()
    {
        
        List<hero> heros_Container = new List<hero>();
        hero carrot = new hero();
        carrot.heroId = 2; carrot.familyId = 1; carrot.rank = 2; carrot.hero_Name = "carrot";
        carrot.sprite_path = (Application.dataPath +"/UI/PNG/BananaProfile.jpg") ; carrot.xpCollected = 0; carrot.xpForNextUpgrade = new List<int>(){30,50,80,160};
        carrot.upgradeCost = new List<int>(){100,200,400,600};


        hero banana = new hero();
        banana.heroId = 1; banana.familyId = 1; banana.rank = 1; banana.hero_Name = "banana";
        banana.sprite_path = (Application.dataPath + "/UI/PNG/BananaProfile.jpg") ;banana.xpForNextUpgrade = new List<int>(){30,50,80,160};
        banana.upgradeCost = new List<int>(){100,200,400,600};

        heros_Container.Add(banana);
        heros_Container.Add(carrot);

        
        FileSave fileSave = new FileSave(FileFormat.Xml);
        //Writes an XML file to the path.
        fileSave.WriteToFile(Application.dataPath + "/SaveSystem/saves/heros_Container.xml", heros_Container);
        Debug.Log("heros_Container saved");
        Debug.Log(Application.dataPath);
        
        heros_Container_tmp = fileSave.ReadFromFile <hero[]> (Application.dataPath + "/SaveSystem/saves/heros_Container.xml");
        Debug.Log("Loaded data: "+heros_Container_tmp);

    } 

    
}
