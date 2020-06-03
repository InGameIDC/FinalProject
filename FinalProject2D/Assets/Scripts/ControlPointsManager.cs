using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointsManager : MonoBehaviour
{
    [SerializeField] public float TeamControlPointsPool = 10f;
    private float[] _teamsCurrentControlPoints;

    public float GetTeamBalance(int teamNum) {
        float balance = 0;
        if(teamNum == 0 || teamNum == 1)
            balance =  _teamsCurrentControlPoints[teamNum];

        return balance;
    }

    private void Awake()
    {
        _teamsCurrentControlPoints = new float[2];
        for (int i = 0; i < _teamsCurrentControlPoints.Length; i++)
        {
            _teamsCurrentControlPoints[i] = TeamControlPointsPool;
        }
    }


    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < _teamsCurrentControlPoints.Length; i++)
        {
            if (_teamsCurrentControlPoints[i] < TeamControlPointsPool)
                _teamsCurrentControlPoints[i] = Mathf.Min(_teamsCurrentControlPoints[i] + Time.deltaTime, TeamControlPointsPool);

            //Debug.Log("Team " + i + " Points: " + _teamsCurrentControlPoints[i]);
        }
        
    }

    public bool CommandyGoTo(HeroUnit hero, Vector2 pos, bool isAICommand)
    {
        if (_teamsCurrentControlPoints[(int)hero.heroTeam] - hero.GetHeroCommandCost() >= 0) {
            _teamsCurrentControlPoints[(int)hero.heroTeam] -= hero.GetHeroCommandCost();
            hero.GoTo(pos);
            Debug.Log("CommandyGoTo, Team: " + (int)hero.heroTeam + ", Remaining: " + _teamsCurrentControlPoints[(int)hero.heroTeam] + ", cost: " + hero.GetHeroCommandCost());
            return true;
        }
        else if(!isAICommand)
        {
            Debug.Log("Team " + (int)hero.heroTeam + " not enough Points: " + _teamsCurrentControlPoints[(int)hero.heroTeam] + ", cost: " + hero.GetHeroCommandCost());
            // show message, UI interaction
        }
            
        return false;
    }

    public bool CommandSetTargetToAttack(HeroUnit hero, GameObject target, bool isAICommand)
    {

        if (_teamsCurrentControlPoints[(int)hero.heroTeam] - hero.GetHeroCommandCost() >= 0)
        {
            _teamsCurrentControlPoints[(int)hero.heroTeam] -= hero.GetHeroCommandCost();
            hero.SetTargetObj(target);
            return true;
        }
        else if(!isAICommand)
        {
            Debug.Log("Team " + (int)hero.heroTeam + " not enough Points: " + _teamsCurrentControlPoints[(int)hero.heroTeam] + ", cost: " + hero.GetHeroCommandCost());
            // show message, UI interaction
        }

        return false;
    }

}
