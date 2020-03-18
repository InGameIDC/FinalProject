using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeroUnit : MonoBehaviour
{

    float DESIRED_POS_MARGIN_OF_ERROR = 0.1f;  // needs to finel.
    float RESPAWN_TIME;                // needs to final.

    Action <HeroUnit> onMove;        //
    Action <HeroUnit> onHit;        // handles hero hit ( health > 0)
    Action <HeroUnit> onRespawn;   //
    Action <HeroUnit> onDeath;    // handles hero death (0 >= health)

    int id;
    float currentHeatlh;
    float maxHealth; 
    // skill SKILL;
    float moveSpeed;
    List <HeroUnit> herosToAttackBank; 
    HeroUnit heroToAttack;      // this is the target to be attacked by the hero.
    Vector3 desiredPos;        // the location desired to move to.
    HeroUnit targetHero;      // the selected enemy to be attacked.
    enum status {dead,siege,moving,idle,attacking,moveAndAttack};

    private void Awake()
    {
        desiredPos = Vector3.zero;
        moveSpeed = 2f;
    }

    void Start()
    {
       
    }

   
    void Update()
    {
        movmentLite();
    }

    /// <summary>
    /// 
    /// </summary>
    private void movmentLite()   // 
    {
        Vector3 direction = this.desiredPos - this.transform.position;
        direction =  direction.normalized;
        rotateTowardDirection(this.desiredPos);

        if (Vector3.Distance((this.transform.position), this.desiredPos) > DESIRED_POS_MARGIN_OF_ERROR)
        {
            this.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Rotate the target toward the desired direction
    /// ###### NEED TO BE CHANGE, TO CHECK IF THE ROTATION REQUIRE ######
    /// </summary>
    private void rotateTowardDirection(Vector3 target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = moveSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
