using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public Action<Projectile, Collider2D> onHitMechs; // for data updates
    public Action<Projectile> onHitDsiplayers; // for feedbakcs: visual and audio displays.
    public GameObject attacker;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (TeamTool.isEnemy(attacker, target.gameObject))
        {
            //on hitting - the health is lowered

            GetComponent<CircleCollider2D>().isTrigger = false;   // turn off the trigger (can't use the same bullet twice)

            if (onHitMechs != null)
                onHitMechs(this, target);

        }
    }
}