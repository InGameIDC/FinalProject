﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    //public Action<Projectile, GameObject> onHitMechs; // function to be atctivated on the target
    public Action<Projectile> onHitDisplayers; // for feedbakcs: visual and audio displays.
    public GameObject attacker;
    public bool hitted = false;
    [SerializeField] public float SelfDestroyAfter = 5f;
    [SerializeField] private GameObject HitEffectObject; // Object that would apear on hit
    [SerializeField] protected bool isPiercing = false;

    protected virtual void Update()
    {
        SelfDestroyAfter -= Time.deltaTime;
        if (SelfDestroyAfter < 0)
            Destroy(gameObject);
    }

    //new
    public float shootDamege;

    
    protected virtual void OnTriggerEnter2D(Collider2D target)
    {
        //Checks that the projectile entered the hit point capsule defined to the player (I wrote it like this for readability reasons):
        GameObject targetObject = target.gameObject;
        
        if (target.tag.Equals("HeroDamageHitArea"))
        {
            //sets the target to be the hero\enemy (=parent) component, instead of HeroDamageHitArea.
            GameObject targetParentObject = targetObject.transform.parent.gameObject;
            targetObject = targetParentObject;

            if (TeamTool.isEnemy(attacker, targetObject) && !hitted)
            {
                if(!isPiercing)
                    hitted = true;
                targetParentObject.GetComponentInChildren<Health>().TakeDamage(shootDamege);
                createHitEffect(transform.position); // creating hit effect

                /*
                if (TeamTool.isEnemy(attacker, targetObject))
                {
                    //on hitting - the health is lowered

                    // To be implemented on the skill onhit
                    GetComponent<CircleCollider2D>().isTrigger = false;   // turn off the trigger (can't use the same bullet twice)

                    if (onHitMechs != null)
                        onHitMechs(this, targetObject);

                }
                */
                Destroy(this.transform.gameObject);
            }
        }
        
    }

    protected void createHitEffect(Vector3 pos)
    {
        pos.z -= 1;
        Instantiate(HitEffectObject, pos, transform.rotation);
    }

    /// <summary>
    /// Calculate the position of the impact
    /// </summary>
    /// <param name="target">The colider of the target</param>
    /// <returns>the target impact position, if fails return the projectile position</returns>
    protected Vector2 getHitlocation(Collider2D targetCol)
    {
        //LayerMask maskLayersToRelate = ~(maskLayersToIgnore | 1 << obj1.layer); For working with objects to ignore

        Vector2 rayDirection = SpaceCalTool.GetVectorDirectionTowardTarget(transform.position, targetCol.transform.position);
        RaycastHit2D[] hittedObject = Physics2D.RaycastAll(transform.position, rayDirection, Mathf.Infinity, LayerMask.GetMask("DamageHitArea"));

        foreach(RaycastHit2D hitted in hittedObject)
        {
            if (hitted.collider == targetCol) {
                Vector2 offset = SpaceCalTool.GetVectorDirectionTowardTarget(transform.position, targetCol.transform.position);
                return hitted.point + offset.normalized * offset.sqrMagnitude * 0.2f;
            }
        }

        return transform.position;
    }
    
}