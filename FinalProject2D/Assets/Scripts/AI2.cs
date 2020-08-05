using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour
{
    private HeroUnit _hero;
    private bool _firstSpawn = true;
    private GameObject targetHill;

    //[SerializeField] Vector2 pos; // only for testing
    private ControlPointsManager _controlPointsManager;

    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _controlPointsManager = GameObject.Find("ControlPointsManager").GetComponent<ControlPointsManager>();
        StartCoroutine(hillsConquerManager());
        _firstSpawn = false;
    }

    void OnEnable()
    {
        if(!_firstSpawn)
            StartCoroutine(hillsConquerManager());
    }

    private GameObject getClosestHill()
    {
        GameObject closestHill = null;
        float ClosestHilldistancePow2 = Mathf.Infinity;
        float distancePow2 = Mathf.Infinity;
        foreach (GameObject hill in GameObject.FindGameObjectsWithTag("Hill"))
        {
            distancePow2 = SpaceCalTool.DistancePow2(transform.position, hill.transform.position);
            if (ClosestHilldistancePow2 > distancePow2)
            {
                closestHill = hill;
                ClosestHilldistancePow2 = distancePow2;
            }
        }

        return closestHill;
    }

    private IEnumerator hillsConquerManager()
    {
        while (true)
        {
            if (targetHill == null || targetHill.activeSelf == false)
            {
                targetHill = getClosestHill();
                if (targetHill != null)
                {
                    //_hero.GoTo(targetHill.transform.position);
                    // if the command fails, wait enough time to have enough points
                    if (!_controlPointsManager.CommandyGoTo(_hero, targetHill.transform.position, true))
                        yield return new WaitForSeconds(_hero.GetHeroCommandCost() - _controlPointsManager.GetTeamBalance((int)_hero.heroTeam));

                }
            }
            yield return new WaitForSeconds(GlobalCodeSettings.AI_Refresh_Time);
        }
    }
}
