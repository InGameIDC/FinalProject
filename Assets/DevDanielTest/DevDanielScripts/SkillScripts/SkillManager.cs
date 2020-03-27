using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager 
{

    private static Dictionary<int, Skill> _skills;

    public static Skill GetSkill(int id)
    {
        if (_skills.ContainsKey(id)) 
        {
            return _skills[id];
        }
       
        return null;
    }

    public static void LoadSkills()
    {

    }
}
