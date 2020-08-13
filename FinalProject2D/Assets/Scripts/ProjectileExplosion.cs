using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileExplosion : Projectile
{
    [SerializeField] private float _hitRadius = 2f;

    public void ManualActiveExplotion(Collider2D target)
    {
        OnTriggerEnter2D(target);
    }

    public float getRadius() => _hitRadius;
    protected override void OnTriggerEnter2D(Collider2D target)
    {
        Debug.Log("OO");
        if (target.tag.Equals("HeroDamageHitArea"))
        {
            GameObject targetObject = target.gameObject;
            //sets the target to be the hero\enemy (=parent) component, instead of HeroDamageHitArea.
            GameObject targetParentObject = targetObject.transform.parent.gameObject;

            Debug.Log("AA");
            if (TeamTool.isEnemy(attacker, targetParentObject) && !hitted)
            {
                Debug.Log("BB");
                createHitEffect(transform.position); // creating hit effect
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

                        //GetComponent<Collider2D>().isTrigger = false; // turn off the trigger (can't use the same bullet twice)
                    }
                }
                if (!isPiercing)
                    hitted = true;
                //Destroy(gameObject);
            }
        }
        else if (target.tag.Equals("Obstacle")){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}

