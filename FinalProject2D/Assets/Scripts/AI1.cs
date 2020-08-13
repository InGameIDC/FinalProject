using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AI1 : MonoBehaviour
{
    public Action OnNoTarget = delegate { };

    private HeroUnit _hero;
    private bool isRunning = true;

    // testing
    private List<GameObject> targetsList; // only for testing
    private ControlPointsManager _controlPointsManager;

    // Start is called before the first frame update
    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    private void Start()
    {
        _controlPointsManager = GameObject.Find("ControlPointsManager").GetComponent<ControlPointsManager>();
        getTargets();
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

    private void getTargets()
    {

        GameObject[] heroes = GameObject.FindGameObjectsWithTag("HeroUnit");
        if (heroes != null && heroes.Length > 0)
            targetsList = GameObject.FindGameObjectsWithTag("HeroUnit").ToList();
        else
            targetsList = new List<GameObject>();
        /*
        else if (targetsList.Count > 0)
            Debug.Log("We already have:" + targetsList[0]);
            */

    }

    private IEnumerator attackTargets()
    {
        yield return new WaitForSeconds(0.3f);
        isRunning = true;
        GameObject target = null;
        while (true)
        {
            target = getWeakest();
            Debug.Log("GOT TARGET: " + target + " Amount: " + targetsList.Count);
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
            else if (targetsList == null || targetsList.Count == 0)
            {
                getTargets();
            }
            else
            { // Case there is no target alive
                yield return new WaitForSeconds(10 * GlobalCodeSettings.AI_Refresh_Time);
                OnNoTarget();
            }
            yield return new WaitForSeconds(1f);
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
                Health healthObj = target.GetComponentInChildren<Health>();
                if (healthObj == null)
                    break;

                float health = healthObj.GetCurrentHealth();
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


