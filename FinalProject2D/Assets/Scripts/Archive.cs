using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive
{
    /*
    /// <summary>
    /// Change the obj DesirePos, and start navigtation to the target vector.
    /// active the OnStartMovment delegation
    /// Author: Ilan, Dor
    /// </summary>
    /// <param name="pos">The new pos, that the hero will move to</param>
    private void SetObjDesirePos(Vector3 pos)
    {
        Debug.Log("SetObjDesirePos");
        if (OnStartMovment != null)
            OnStartMovment();
        setDesiredPos(pos);
        StartCoroutine(moveObject());
        //_navMeshAgent.SetDestination(_desiredPos);
    }
    */


    //private void heroManager() // stats the heroManager funcs with coroutine
    //{
    //    activeHeroManager();
    //    //StartCoroutine(activeHeroManager());
    //}
    ///// <summary>
    ///// The hero logic
    ///// Author: Ilan
    ///// </summary>
    ///// <returns></returns>
    //private void activeHeroManager() // WARNING - SUPER SENSETIVE, DO NOT EDIT
    //{
    //    if (++testCounter == 500000)
    //    { // wont preform any func after 10000 iterations (for testing)
    //        stopAll();
    //        Destroy(this);
    //    }
    //    else if (stop) // testing
    //        return; //yield return new WaitForSeconds(100);


    //    //yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE / 10);

    //    bool isHeroMoving = _movement.IsObjMoving();
    //    /*
    //    if (targetsToAttackBank.Count == 0) // || targetToAttack != null)
    //        return;
    //        */

    //    if (_targetObj != null && _targetsToAttackBank.Contains(_targetObj) && _skill.isTargetAttackable(_targetObj)) // If the main target is not null and within the range, would start to attack
    //    {
    //        _movement.StopMovment();
    //        _targetToAttack = _targetObj;
    //        prepareToAttack();
    //    }
    //    else if (_targetObj != null && isHeroMoving)
    //    {
    //        return;//yield return null;
    //    }
    //    else if (_targetObj != null && !isHeroMoving) // If these a main target, and its not within the given range, would move toward it.
    //    {
    //        /*
    //        if(!TargetInRange(_targetObj))
    //            GoTo(_movement.CalcClosestPosWithThisDistance(_skill.GetRange(), transform.position, _targetObj.transform.position));
    //        */
    //        prepareForNewOrder();
    //        //OnTargetInFieldOfView = heroManager; <===== TO BE FIXED (WONT WORK WITH OUT THIS)
    //        startTrackIfObjTargetAttackable();
    //        if (!TargetInRange(_targetObj))
    //            GoTo(_targetObj.transform.position);

    //    }
    //    else if (!isHeroMoving && !_movement.IsObjRotating() && isThereATarget()) // If not moving and there are targets in the targets bank
    //    { // NEED TO BE EDITED TO SUPPORT MULTI TARGETS WITHIN THE RANGE!! <==###########################
    //        GameObject target = findAnAttackableTarget();
    //        if (target != null) // If the first target is attackable, would start to attack
    //        {
    //            _targetToAttack = target;
    //            prepareToAttack();
    //        }
    //        else // Would start scanning track the target.
    //        {
    //            startScanningForAnAttackableTarget();
    //            //OnTargetInFieldOfView = heroManager; <===== TO BE FIXED(WONT WORK WITH OUT THIS)
    //        }
    //    }

    //}


    ///// <summary>
    ///// Author: Ilan
    ///// </summary>
    ///// <param name="target"></param>
    //private void testAddTarget(GameObject target) // A test function that track a target and add it to the hero targetsbank if it is within the skill range
    //{
    //    if (target == null)
    //    {
    //        if (_targetsToAttackBank.Contains(target)) _targetsToAttackBank.Remove(target);
    //        return;
    //    }

    //    if (!_targetsToAttackBank.Contains(target))
    //    {
    //        if (_skill.isTargetInRange(target.transform.position))
    //        {
    //            Test.SetTargetColor(target, Color.red);
    //            _targetsToAttackBank.Add(target);
    //            heroManager();
    //        }
    //    }
    //    else
    //    {
    //        if (!_skill.isTargetInRange(target.transform.position))
    //        {
    //            Test.SetTargetColor(target, Color.green);
    //            _targetsToAttackBank.Remove(target);
    //            heroManager();
    //        }
    //    }
    //}


    ///// <summary>
    ///// Run the testAddTargets on the testTargets list
    ///// </summary>
    //public void testScanForTargets()
    //{
    //    foreach (GameObject target in testGameTargets)
    //    {
    //        testAddTarget(target);
    //    }
    //}




    ///// <summary>
    ///// Check if the target within the skill range.
    ///// If the target within the range, add the target to the hero targets bank
    ///// Author: Ilan
    ///// </summary>
    ///// <param name="targetToAttack">Target to be checked</param>
    ///// <returns>true if the target is within the skill range</returns>
    //public bool TargetInRange(GameObject targetToAttack)
    //{
    //    //addHeroesToAttackBank(targetToAttack);
    //    if (_skill.isTargetInRange(targetToAttack.transform.position))
    //    {
    //        //_targetsToAttackBank.Add(targetToAttack);
    //        //heroManager(); // need to be implemented with delegation
    //        return true;
    //    }

    //    return false;
    //}


    //private void AutoAttack()
    //{
    //    while (_skill.isTargetAttackable(_targetToAttack))
    //    {
    //        prepareToAttack();
    //        yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);

    //        if()
    //    }
    //    _targetToAttack = null;
    //}
    #region Cooldown
    /*
    public void startCooldown()
    {
        if (_isOnCooldown)
            return;

        _isOnCooldown = true;
        StartCoroutine(cooldownTimeManage());
    }

    public IEnumerator cooldownTimeManage()
    {
        yield return new WaitForSeconds(_cooldown);

        _isOnCooldown = false;
    }
    */
    #endregion
}
