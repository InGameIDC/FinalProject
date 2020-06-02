using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI1 : MonoBehaviour
{
    private HeroUnit _hero;
    private bool isRunning = true;

    // testing
    [SerializeField] List<GameObject> targetsList; // only for testing

    // Start is called before the first frame update
    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    private void Start()
    {
        StartCoroutine(attackTargets());
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
        Debug.Log("Activated");
        GameObject target = null;
        while (targetsList.Count > 0)
        {
            target = getWeakest();
            _hero.SetTargetObj(target);
            while (target != null && target.activeSelf)
            {
                yield return new WaitForSeconds(GlobalCodeSettings.AI_Refresh_Time);
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


