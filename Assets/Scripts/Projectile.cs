using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public Action<Projectile, Collider> onHitMechs; // for data updates
    public Action<Projectile> onHitDsiplayers; // for feedbakcs: visual and audio displays.

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag != "EnemyUnit")
            return;

        if (onHitMechs != null)
            onHitMechs(this, target);
    }
}
