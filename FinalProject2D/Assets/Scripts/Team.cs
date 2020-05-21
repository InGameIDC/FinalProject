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
        if (target.tag.Equals(Enum.GetName(typeof(Team), Team.HeroUnit)) || target.tag.Equals(Enum.GetName(typeof(Team), Team.EnemyUnit)))
        {
            if (attacker.tag != target.tag)
                return true;
        }

        return false;

    }

    public static Team GetTeamByString(string teamName)
    {
        switch (teamName)
        {
            case "HeroUnit":
                return Team.HeroUnit;

            case "EnemyUnit":
                return Team.EnemyUnit;

            default:
                Debug.Log("Gave OTHER team type");
                return Team.Other;
        }
    }

    public static LayerMask getTeamLayer(string teamName) => LayerMask.NameToLayer(teamName);

    public static string getEnemyTeamName(Team myTeam)
    {
        return Enum.GetName(typeof(Team), (int)myTeam ^ 1);
    }

    public static int getEnemyLayer(Team myTeam)
    {
        return LayerMask.NameToLayer(getEnemyTeamName(myTeam));
    }

    public static int getEnemyLayer(string myTeam)
    {
        switch (myTeam)
        {
            case "HeroUnit":
                return LayerMask.NameToLayer("EnemyUnit");

            case "EnemyUnit":
                return LayerMask.NameToLayer("HeroUnit");
        }

        return -1;
    }
}
