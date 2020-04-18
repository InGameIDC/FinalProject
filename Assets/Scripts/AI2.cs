using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour
{
    private HeroUnit _hero;

    // testing
    [SerializeField] List<GameObject> targetsList; // only for testing
    [SerializeField] private GameObject _hill;

    // Start is called before the first frame update
    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    private void Start()
    {
        //StartCoroutine(attackTargets());
        _hero.GoTo(_hill.transform.position);
    }

    private IEnumerator attackTargets()
    {
        foreach (GameObject target in targetsList)
        {
            _hero.SetTargetObj(target);
            while (target != null)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
