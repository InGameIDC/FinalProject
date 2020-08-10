using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI1 : MonoBehaviour
{
    public Action OnNoTarget = delegate { };

    private HeroUnit _hero;
    private bool isRunning = true;

    // testing
    [SerializeField] List<GameObject> targetsList; // only for testing
    private ControlPointsManager _controlPointsManager;

    // Start is called before the first frame update
    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
        if (targetsList == null)
            targetsList = new List<GameObject>();
        else if(targetsList.Count > 0)
            Debug.Log("We already have:" + targetsList[0]);
    }

    private void Start()
    {
        _controlPointsManager = GameObject.Find("ControlPointsManager").GetComponent<ControlPointsManager>();
        StartCoroutine(attackTargets());
    }

    public void addTraget(GameObject target)
    {
        Debug.Log("Added target: " + target + " num of targets: " + targetsList.Count);
        targetsList.Add(target);
    }
    void OnEnable()
    {
        if(!isRunning)
            StartCoroutine(attackTargets());
    }

    void OnDisable()
    {
        isRunning = false;
    }

    private IEnumerator attackTargets()
    {
        yield return new WaitForSeconds(0.3f);
        isRunning = true;
        GameObject target = null;
        while (targetsList.Count > 0)
        {
            target = getWeakest();
            if (target != null)
            {
                // if the command fails, wait enough time to have enough points
                if (!_controlPointsManager.CommandSetTargetToAttack(_hero, target, true))
                    yield return new WaitForSeconds(_hero.GetHeroCommandCost() - _controlPointsManager.GetTeamBalance((int)_hero.heroTeam));

                //_hero.SetTargetObj(target);
                while (target != null && target.activeSelf)
                {
                    yield return new WaitForSeconds(GlobalCodeSettings.AI_Refresh_Time);
                }
            }
            else
            { // Case there is no target alive
                yield return new WaitForSeconds(10 * GlobalCodeSettings.AI_Refresh_Time);
                OnNoTarget();
            }

        }
    }

   private GameObject getWeakest() //GetCurrentHealth
    {
        GameObject weakestTarget = null;
        float weakestHealth = Mathf.Infinity;
        foreach (GameObject target in targetsList)
        {
            if (target != null && target.activeSelf)
            {
                float health = target.GetComponentInChildren<Health>().GetCurrentHealth();
                if (weakestHealth > health)
                {
                    weakestTarget = target;
                    weakestHealth = health;
                }
            }
        }

        return weakestTarget;
    }

}


