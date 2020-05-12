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
        foreach (GameObject target in targetsList)
        {
            _hero.SetTargetObj(target);
            while (target != null && target.activeSelf)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

}


