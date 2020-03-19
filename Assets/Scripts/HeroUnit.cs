using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour
{
    const float FRAME_RATE = 0.0167f;
    const float DESIRED_POS_MARGIN_OF_ERROR = 0.1f;
    const float RESPAWN_TIME = 10f;

    Action <HeroUnit> OnMove;        //
    Action <HeroUnit> OnHit;        // handles hero hit ( health > 0)
    Action <HeroUnit> OnRespawn;   //
    Action <HeroUnit> OnDeath;    // handles hero death (0 >= health)
    //private Action<HeroUnit> OnReachPos; // function that would been preform when the hero reach the desired pos

    int id;
    float currentHeatlh;
    float maxHealth;
    Skill skill;
    float moveSpeed;
    List <GameObject> targetsToAttackBank;
    GameObject targetToAttack;      // this is the target to be attacked by the hero.
    Vector3 desiredPos;        // the location desired to move to.
    Vector3 desiredRotationDirection;    // the direction the hero desired to rotate toward.
    GameObject targetHero;      // the selected enemy to be attacked.
    ObjStatus status;

    public GameObject testTarget;

    private void Awake()
    {
        moveSpeed = 0.2f;
        this.status = ObjStatus.idle;
        targetToAttack = null;
        targetsToAttackBank = new List<GameObject>();
    }

    void Start()
    {
        testMovement();
        /*
        skill = this.GetComponent<Skill>();
        TargetInRange(testTarget);
        prepareToAttack();
        */
    }

    private void prepareToAttack()
    {
        if (IsObjRotating()) // TO BE CHANGED
            return;

        Vector3 targetPos = targetToAttack.transform.position;
        desiredRotationDirection = getVectorDirectionTowardTarget(targetToAttack.transform.position);
        if(!IsLookingAtTheTarget(targetPos))
            StartCoroutine(rotateTowardDirection(prepareToAttack));
        else
            attack();

    }

    private void attack()
    {
        skill.attack();
    }

    /*
    private IEnumerator autoAttack()
    {

    }
    */

    private void setHeroToAttack(GameObject targetToAttack)
    {
        this.targetToAttack = targetToAttack;
        this.status = ObjStatus.attacking;
    }

    private void addHeroesToAttackBank(GameObject targetToAttack)
    {
        targetsToAttackBank.Add(targetToAttack);

        bool isNeedToBeSetAsTarget = (this.targetToAttack == null || this.targetToAttack == targetHero);

        if (isNeedToBeSetAsTarget)
        {
            setHeroToAttack(targetToAttack);
        }
    }

    public void TargetInRange(GameObject targetToAttack)
    {
        addHeroesToAttackBank(targetToAttack);
        
        
    }

    public bool IsObjMoving()
    {
        return Vector3.Distance(transform.position, desiredPos) > DESIRED_POS_MARGIN_OF_ERROR;
    }

    public bool IsObjRotating()
    {
        float angelDif = diffAngle(desiredRotationDirection);
            return angelDif > DESIRED_POS_MARGIN_OF_ERROR * 0.001f;
    }

    private bool IsLookingAtTheTarget(Vector3 targetPos)
    {
        Vector3 direction = getVectorDirectionTowardTarget(desiredPos);
        float angelDif = diffAngle(direction);
        return angelDif <= DESIRED_POS_MARGIN_OF_ERROR * 0.001f;
    }


    /*
    public bool IsHeroAttacking()
    {
        return status == ObjStatus.attacking || status == ObjStatus.moveAndAttack || status == ObjStatus.moveAndAttackAndRotate;
    }

    public bool IsHeroMoving()
    {
        return status == ObjStatus.moving || status == ObjStatus.moveAndAttack || status == ObjStatus.moveAndRotate || status == ObjStatus.moveAndAttackAndRotate;
    }

    public bool IsHeroRotating()
    {
        return status == ObjStatus.rotating || status == ObjStatus.moveAndRotate|| status == ObjStatus.moveAndAttackAndRotate;
    }

    public bool IsHeroIdleOrSiege()
    {
        return status == ObjStatus.idle || status == ObjStatus.siege;
    }
    */

    private void testMovement()
    {
        //SetHeroDesirePos(Vector3.zero);
        //StartCoroutine(testMovmentFuncChangePosWhileMov(new Vector3(1f, 1f, 1f), 1f));
        //StartCoroutine(testRotateFuncChangePosWhileRotate(new Vector3(-10f, -10f, -10f), 0.4f));
        
        List<Vector3> testPoses = new List<Vector3>();
        testPoses.Add(new Vector3(5f, 0f, 5f));
        testPoses.Add(new Vector3(-5f, 0f, 5f));
        testPoses.Add(new Vector3(-10f, 0f, -10f));
        testPoses.Add(new Vector3(2f, 0f, -10f));
        StartCoroutine(testMovmentFuncListOfPosOrders(testPoses, 3f, true));
        
    }

    private Vector3 getVectorDirectionTowardTarget(Vector3 target)
    {
        return desiredPos - this.transform.position;
    }

    /// <summary>
    /// Move the Gameobject to the desiredPos.
    /// Author: Ilan 
    /// In addtion, rotate the Object toward the pos.
    /// </summary>
    private IEnumerator moveObject(Action onFinishMovment)
    {
        Vector3 desiredPos = this.desiredPos;
        Vector3 direction = getVectorDirectionTowardTarget(desiredPos);
        direction =  direction.normalized;

        this.desiredRotationDirection = direction;
        StartCoroutine(rotateTowardDirection(null));

        while (Vector3.Distance((this.transform.position), desiredPos) > DESIRED_POS_MARGIN_OF_ERROR) // Checks if the object reached to the desired pos, and if it's on movment
        {
            if (desiredPos != this.desiredPos) // checks if the movment is still relvant
                yield break;

            this.transform.position += direction * moveSpeed;
            yield return new WaitForSeconds(FRAME_RATE);
        }
        this.transform.position = desiredPos;

        if (onFinishMovment != null && !IsObjMoving())
            onFinishMovment();
    }

    /// <summary>
    /// Rotate the target toward the desired direction
    /// Author: Ilan
    /// </summary>
    /// TO BE EDIT, smart rotate that support target shoot lock
    private IEnumerator rotateTowardDirection(Action onFinishRotate)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = this.desiredRotationDirection;
        float singleStep = moveSpeed * 0.2f; // The step size is equal to speed times frame time.
        Quaternion rotationAmount; // The rotation amount per each iteration

        float angelDif = diffAngle(targetDirection);
        while (angelDif > DESIRED_POS_MARGIN_OF_ERROR * 0.001f) // Checks if the object finished to rotate target, and if it's on movment
        {
            if (targetDirection != this.desiredRotationDirection) // checks if the rotation is still relvant
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

        if (onFinishRotate != null && !IsObjRotating())
            onFinishRotate();
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
        StartCoroutine(moveObject(null));
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

    /// <summary>
    /// Uses to test the object desireRotateDirection change while rotating
    /// Author: Ilan
    /// </summary>
    /// <param name="Poses"></param>
    /// <returns></returns>
    private IEnumerator testRotateFuncChangePosWhileRotate(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.desiredRotationDirection = pos;
    }
}
