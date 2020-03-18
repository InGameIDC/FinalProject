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
    Vector3 disiredPos;        // the location desired to move to.
    HeroUnit targetHero;      // the selected enemy to be attacked.
    enum status {dead,siege,moving,idle,attacking,moveAndAttack};

    private void Awake()
    {
        disiredPos = Vector3.zero;
        moveSpeed = 2f;
    }

    void Start()

    {
       
    }

   
    void Update()
    {
        movmentLite();
    }

    private void movmentLite  ()   // 
    {
       Vector3 direction = this.transform.position - this.disiredPos;
       direction =  direction.normalized;     
       if (Vector3.Distance((this.transform.position), this.disiredPos) > DESIRED_POS_MARGIN_OF_ERROR)
        {
            this.transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
