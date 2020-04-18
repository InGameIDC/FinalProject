using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Team
{
    HeroUnit,
    EnemyUnit,
    Other
}

public static class TeamTool
{
    public static bool isEnemy(GameObject attacker, GameObject target)
    {
        Debug.Log(Team.HeroUnit); 
        if (target.tag.Equals(Enum.GetName(typeof(Team), Team.HeroUnit)) || target.tag.Equals(Enum.GetName(typeof(Team), Team.EnemyUnit)))
        {
            if (attacker.tag != target.tag)
                return true;
        }

        return false;

    }

    public static Team GetTeamByString(string teamName)
    {
        switch(teamName)
        {
            case "HeroUnit":
                return Team.HeroUnit;

            case "EnemyUnit":
                return Team.EnemyUnit;

            default:
                return Team.Other;
        }
    }
}


