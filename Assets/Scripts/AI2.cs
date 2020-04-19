using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour
{
    private HeroUnit _hero;

    // testing
    [SerializeField] List<GameObject> targetsList; // only for testing
    [SerializeField] private GameObject _hill;
    [SerializeField] private Vector3 _taegetLocation;// new Vector3(0.2f, 0.0f, 0.5f)

    // Start is called before the first frame update
    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    private void Start()
    {
        //StartCoroutine(attackTargets());
        _hero.GoTo(_taegetLocation);
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
