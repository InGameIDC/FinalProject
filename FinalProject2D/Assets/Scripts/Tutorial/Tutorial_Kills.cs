using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("working");
        Health targetHealth = target.GetComponentInChildren<Health>();
        TargetsToKill.Remove(targetHealth);
        targetHealth.OnDeath -= removeTarget;

        if (TargetsToKill.Count == 0 /*&& TutorialManager.Instance.CurrentTutorial == this*/)
        {
            StartCoroutine(nextLevelCoroutine());
            TutorialManager.Instance.CompletedTutorial();
        }

    }

    IEnumerator nextLevelCoroutine()
    {
        Debug.Log("YAY");
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("HomeMenu");

    }
}
