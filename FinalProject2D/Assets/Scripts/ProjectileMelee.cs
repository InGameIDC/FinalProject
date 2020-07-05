using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMelee : Projectile
{
    [SerializeField] bool canHitMultipleEnemies = false;
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
                if(!canHitMultipleEnemies)
                    hitted = true;

                targetParentObject.GetComponentInChildren<Health>().TakeDamage(shootDamege);
                createHitEffect(getHitlocation(target)); // creating hit effect

            }
        }

    }
}
