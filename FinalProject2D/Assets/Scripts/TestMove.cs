using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestMove : MonoBehaviour
{
    [SerializeField] GameObject target;
    private Movment2D _mov2D;
    private HeroUnit _hero;

    // Start is called before the first frame update
    void Start()
    {
        _mov2D = GetComponent<Movment2D>();
        _hero = GetComponent<HeroUnit>();
        //_mov2D.GoTo(new Vector2(3, 5));
        _mov2D.TargetLock(target, 20f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
