using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    //public Action<Projectile, GameObject> onHitMechs; // function to be atctivated on the target
    public Action<Projectile> onHitDisplayers; // for feedbakcs: visual and audio displays.
    public GameObject attacker;
    public bool hitted = false;

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
                hitted = true;
                targetParentObject.GetComponentInChildren<Health>().TakeDamage(shootDamege);

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
    
}