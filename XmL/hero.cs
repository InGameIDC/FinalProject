using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Pathfinding;
using Path = System.IO.Path;
using System.Xml;
using System.Collections;
public class hero : MonoBehaviour
{
    
    [XmlElement("hero")]
   public string name;
    
   public string skill;
   
   public string type;
    
   public float movement_speed;
    
    float respawn_time;
    public hero(string name ,string skill, string type, float movement_speed, float respawn_time)
    {
        this.name = name;
        this.skill = skill;
        this.type = type;
        this.movement_speed = movement_speed;
        this.respawn_time = respawn_time;
    }
}

