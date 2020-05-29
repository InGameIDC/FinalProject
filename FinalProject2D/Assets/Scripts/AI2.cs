using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : MonoBehaviour
{
    private HeroUnit _hero;
    private bool _firstSpawn = true;

    [SerializeField] Vector2 pos; // only for testing

    void Awake()
    {
        _hero = transform.GetComponent<HeroUnit>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _hero.GoTo(pos);
        _firstSpawn = false;
    }

    void OnEnable()
    {
        if(!_firstSpawn)
            _hero.GoTo(pos);
    }

}
