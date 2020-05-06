using UnityEngine;
using System.Collections.Generic;



public class hero
{
    string name;
    string skill;
    string type;
    float movement_speed;
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

public class skill
{
    string skill_name;
    string skill_type;
    float skill_speed;
    float skill_dammege;
    public skill(string skill_name, string skill_type, float skill_speed, float skill_dammege)
    {
        this.skill_name = skill_name;
        this.skill_type = skill_type;
        this.skill_dammege = skill_dammege;
        this.skill_speed = skill_speed;
    }

}
