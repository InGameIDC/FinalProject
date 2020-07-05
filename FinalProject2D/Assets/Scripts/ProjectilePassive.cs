using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePassive : Projectile
{ // not in use
    [SerializeField] private float _damageInterval = 2f;
    private float _damageIntervalStartTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Time.time - _damageIntervalStartTime < _damageInterval)
            return;

        _damageIntervalStartTime = Time.time;

    }
}
