using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileExplosion : MonoBehaviour
{
    // public Action<Projectile, Collider2D> onHitMechs; // for data updates
    // public Action<Projectile> onHitDisplayers; // for feedbakcs: visual and audio displays.
    public GameObject attacker;

    private float _hitRadius = 3f;
    private string _enemyTag = "Enemy";

    private void OnTriggerEnter2D(Collider2D target)
    {
        Collider[] enemiesInSphere = Physics.OverlapSphere(target.gameObject.transform.position, _hitRadius);
        foreach (Collider enemy in enemiesInSphere)
        {
            if (enemy.CompareTag(_enemyTag))
            {
                //Damage the enemy
                if (TeamTool.isEnemy(attacker, target.gameObject))
                {
                    //on hitting - the health is lowered

                    GetComponent<CircleCollider2D>().isTrigger =
                        false; // turn off the trigger (can't use the same bullet twice)

                    if (onHitMechs != null)
                        onHitMechs(this, target);

                }
            }
        }
    }
}

