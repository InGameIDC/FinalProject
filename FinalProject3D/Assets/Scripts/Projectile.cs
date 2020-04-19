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
        else
        {
            target.GetComponent<Health>().TakeDamage(1);        //on hitting - the health is lowered

            GetComponent<SphereCollider>().isTrigger = false;   // turn off the trigger (can't use the same bullet twice)
        }


        if (onHitMechs != null)          
            onHitMechs(this, target);     
    }
}
