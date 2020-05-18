using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class playerStats : MonoBehaviour
{
  
[XmlElement("stats")]

    public string username;
   public int level;
    
   public int coins;
   
    public playerStats(string name, int level, int coins)
    {
        this.name = name;
        this.level = level;
        this.coins = coins;
    }
}


  

