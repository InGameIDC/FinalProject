using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Kills : Tutorial
{
    [SerializeField] public List<Health> TargetsToKill;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Health target in TargetsToKill)
        {
            target.OnDeath += removeTarget;
        }
    }

    private void removeTarget(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        TargetsToKill.Remove(targetHealth);
        targetHealth.OnDeath -= removeTarget;

        if (TargetsToKill.Count == 0 && TutorialManager.Instance.CurrentTutorial == this)
            TutorialManager.Instance.CompletedTutorial();
    }
}
