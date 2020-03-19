using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public Action<Projectile, Collider> onHitMechs; // for data updates
    public Action<Projectile> onHitDsiplayers; // for feedbakcs: visual and audio displays.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag != "Enemy")
            return;

        if (onHitMechs != null)
            onHitMechs(this, target);
    }
}
