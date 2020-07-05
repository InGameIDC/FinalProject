using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

[System.Serializable]
public class hero
{
    public string hero_Name;
    public string sprite_path;
    
    public int heroId;
    public int familyId;
    public int rank;
    public int cardStat;

    public int upgradeStat;

 
    public int xpCollected;

    public List <int> xpForNextUpgrade = new List <int> ();
    public List <int> upgradeCost = new List<int>();

    #region CONSTRUCTOR
    public hero()
    {

    }

    public hero(string hero_Name,string sprite_path ,int id,int familyId, int rank , int upgradeStat,int xpCollected,
    List<int> xpForNextUpgrade, List<int> upgradeCost)
    {
        this.hero_Name = hero_Name;
        this.sprite_path = sprite_path;
        this.heroId = id;
        this.familyId = familyId;
        this.rank = rank;
        this.upgradeStat = upgradeStat;
        this.xpCollected = xpCollected;
        this.xpForNextUpgrade = xpForNextUpgrade;
        this.upgradeCost = upgradeCost;

    }
    
    
    #endregion
   
}
    