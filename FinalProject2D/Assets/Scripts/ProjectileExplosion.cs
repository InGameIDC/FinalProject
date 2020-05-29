using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileExplosion : Projectile
{
    [SerializeField] private float _hitRadius = 2f;

    protected override void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag.Equals("HeroDamageHitArea"))
        {
            GameObject targetObject = target.gameObject;
            //sets the target to be the hero\enemy (=parent) component, instead of HeroDamageHitArea.
            GameObject targetParentObject = targetObject.transform.parent.gameObject;

            if (TeamTool.isEnemy(attacker, targetParentObject))
            {

                Collider2D[] damageHitAreasInSphere;

                damageHitAreasInSphere = Physics2D.OverlapCircleAll(transform.position, _hitRadius, LayerMask.GetMask("DamageHitArea"));
                foreach (Collider2D damageHitArea in damageHitAreasInSphere)
                {
                    GameObject unit = damageHitArea.transform.parent.gameObject;
                    //Damage the enemy
                    if (TeamTool.isEnemy(attacker, unit))
                    {
                        //on hitting - the health is lowered
                        unit.GetComponentInChildren<Health>().TakeDamage(shootDamege);

                        GetComponent<CircleCollider2D>().isTrigger = false; // turn off the trigger (can't use the same bullet twice)
                    }
                }
                Destroy(this.transform.gameObject);
            }
        }
    }
}

