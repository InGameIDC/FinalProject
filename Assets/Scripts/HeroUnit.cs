using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack };


public class HeroUnit : MonoBehaviour
{
    const float FRAME_RATE = 0.0167f;
    const float DESIRED_POS_MARGIN_OF_ERROR = 0.1f;  // needs to finel.
    const float RESPAWN_TIME = 10f;                // needs to final.

    Action <HeroUnit> OnMove;        //
    Action <HeroUnit> OnHit;        // handles hero hit ( health > 0)
    Action <HeroUnit> OnRespawn;   //
    Action <HeroUnit> OnDeath;    // handles hero death (0 >= health)

    int id;
    float currentHeatlh;
    float maxHealth; 
    // skill SKILL;
    float moveSpeed;
    List <HeroUnit> herosToAttackBank; 
    HeroUnit heroToAttack;      // this is the target to be attacked by the hero.
    Vector3 desiredPos;        // the location desired to move to.
    HeroUnit targetHero;      // the selected enemy to be attacked.
    ObjStatus status;

    private void Awake()
    {
        moveSpeed = 0.2f;
        this.status = ObjStatus.idle;
    }

    void Start()
    {
        //SetHeroDesirePos(Vector3.zero);
        //StartCoroutine(testMovmentFuncChangePosWhileMov(new Vector3(1f, 1f, 1f), 1f));
        /*
        List<Vector3> testPoses = new List<Vector3>();
        testPoses.Add(new Vector3(5f, 0f, 5f));
        testPoses.Add(new Vector3(-5f, 0f, 5f));
        testPoses.Add(new Vector3(-10f, 0f, -10f));
        testPoses.Add(new Vector3(2f, 0f, -10f));
        StartCoroutine(testMovmentFuncListOfPosOrders(testPoses, 3f, true));
        */
    }

   
    void Update()
    {

    }

    /// <summary>
    /// Move the Gameobject to the desiredPos.
    /// Author: Ilan 
    /// In addtion, rotate the Object toward the pos.
    /// </summary>
    private IEnumerator moveObject()
    {
        Vector3 desiredPos = this.desiredPos;
        Vector3 direction = desiredPos - this.transform.position;
        direction =  direction.normalized;
        StartCoroutine(rotateTowardDirection());

        while (Vector3.Distance((this.transform.position), desiredPos) > DESIRED_POS_MARGIN_OF_ERROR && (status == ObjStatus.moving || status == ObjStatus.moveAndAttack)) // Checks if the object reached to the desired pos, and if it's on movment
        {
            if (desiredPos != this.desiredPos) // checks if the movment is still relvant
                yield break;

            this.transform.position += direction * moveSpeed;
            yield return new WaitForSeconds(FRAME_RATE);
        }
        this.transform.position = desiredPos;
    }

    /// <summary>
    /// Rotate the target toward the desired direction
    /// Author: Ilan
    /// </summary>
    /// TO BE EDIT, smart rotate that support target shoot lock
    private IEnumerator rotateTowardDirection()
    {
        Vector3 desiredPos = this.desiredPos;
        float singleStep = moveSpeed * 0.2f; // The step size is equal to speed times frame time.
        Quaternion rotationAmount; // The rotation amount per each iteration
        // Determine which direction to rotate towards
        Vector3 targetDirection = desiredPos - transform.position;

        float angelDif = diffAngle(targetDirection);
        while (angelDif > DESIRED_POS_MARGIN_OF_ERROR * 0.001f && (status == ObjStatus.moving || status == ObjStatus.moveAndAttack)) // Checks if the object finished to rotate target, and if it's on movment
        {
            if (desiredPos != this.desiredPos) // checks if the rotation is still relvant
                yield break;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);
            // Calculate a rotation a step closer to the target and applies rotation to this object
            rotationAmount = Quaternion.LookRotation(newDirection);
        
            this.transform.rotation = rotationAmount; // rotate the object
            angelDif = diffAngle(targetDirection); // calculate the diffrance between the current angle, to the require

            yield return new WaitForSeconds(FRAME_RATE);
        }
        this.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(this.transform.forward, targetDirection, singleStep, 0.0f));

    }

    /// <summary>
    /// Calculate the diffrance between the current angel, to the require 
    /// Author: Ilan 
    /// </summary>
    /// <param name="target">The target direction vector (desiredPos - transform.position) </param>
    /// <returns>Returns the diffrances angle</returns>
    private float diffAngle(Vector3 targetDirection)
    {
        Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, 360f, 0.0f);
        Quaternion rotationLeft = Quaternion.LookRotation(newDirection);
        return Quaternion.Angle(transform.rotation, rotationLeft);
    }

    /// <summary>
    /// Change the hero DesirePos, and start the movment func, to move the hero toward the new pos.
    /// Author: Ilan 
    /// </summary>
    /// <param name="pos">The new pos, that the hero will move to</param>
    public void SetHeroDesirePos(Vector3 pos)
    {
        this.desiredPos = pos;
        this.status = ObjStatus.moving;
        StartCoroutine(moveObject());
    }

    /// <summary>
    /// Uses to test the object movment
    /// Author: Ilan
    /// </summary>
    /// <param name="Poses"></param>
    /// <returns></returns>
    private IEnumerator testMovmentFuncListOfPosOrders(List<Vector3> poses, float delay, bool patrolLoop)
    {
        while (patrolLoop)
        {
            foreach (Vector3 pos in poses)
            {
                SetHeroDesirePos(pos);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    /// <summary>
    /// Uses to test the object reaction to pos change while moving
    /// Author: Ilan
    /// </summary>
    /// <param name="Poses"></param>
    /// <returns></returns>
    private IEnumerator testMovmentFuncChangePosWhileMov(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.desiredPos = pos;
    }
}
