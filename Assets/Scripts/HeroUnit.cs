using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour
{



    Action OnTargetInFieldOfView;                         // On scan End
    Action<HeroUnit> OnMove;                             //
    Action<HeroUnit> OnHit;                             // Handles hero hit ( health > 0)
    Action<HeroUnit> OnRespawn;                        //
    Action<HeroUnit> OnDeath;                         // Handles hero death (0 >= health)
    // private Action<HeroUnit> //onFinishAction;    // Functions that would be preform when the hero finish an action;
    private Action onFinishMovment;                 // Function that would be preform when the hero finish to move / rotate
    private Action onStartMovment;                 // Function that would be preform when the hero start to move / rotate
    //private Action<HeroUnit> onFinishRotate;    // Function that would be preform when the finish to rotate

    private int _id;
    private float _currentHeatlh;
    private float _maxHealth;
    private Skill _skill;
    private List<GameObject> _targetsToAttackBank;
    private GameObject _targetToAttack;             // This is the target to be attacked by the hero.
    private GameObject _targetObj;                 // The selected enemy to be attacked.
    private ObjStatus _status;                    // The hero order status (CURRECTLY NOT IN USE, MIGHT BE REMOVED)
    private bool _isScanning;
    private Movment _movement;                  // The movment component script


    // testing
    bool stop;
    int testCounter;
    public GameObject testTarget; // only for testing

    /// Says good morning to the script
    private void Awake()
    {
        this._status = ObjStatus.idle;
        _targetToAttack = null;
        _targetsToAttackBank = new List<GameObject>();
        _skill = this.GetComponent<Skill>();
        _movement = this.GetComponent<Movment>();

        testCounter = 0;
        stop = false;

        //_movement.OnFinishMovment += heroManager;
        //_movement.OnStartMovment += prepareForNewOrder;
    }

    void Start()
    {
        //testMovement();

        //StartCoroutine(testPrintRotationEveryInterval(1f));

        //TargetInRange(testTarget);
        //SetTargetObj(testTarget);

        //Debug.Log(skill.isTargetInAvailable(testTarget));
        //_navComp.GoTo(new Vector3(1f, 0f, 2f));
        //Test.CreateASphre(new Vector3(1f, 0f, 2f));
        //stopAllAfterDelay(10f);
        //Test.DrawCircle(this.gameObject, skill.GetRange() - 0.5f, 0.05f);
        GoTo(_movement.GetXZposRelativeVector(new Vector3(6f, 0f, -8f)));
        //StartCoroutine(Test.ActiveOnIntervals(_movement.StopMovment, 1f));
        //StartCoroutine(testPrintDirection());
    }

    private void Update() //// @@@@ TO BE REMOVED!!
    {
        //heroManager();
        testAddTargets(testTarget);
    }
    /*
    private IEnumerator testPrintDirection()
    {
        while (true)
        {
            //if (desiredRotationDirection != null)
                //Debug.Log(desiredRotationDirection);

            if (testTarget != null)
                Debug.Log(testTarget.Equals(_movment.desiredRotationDirection));

            Debug.Log(desiredRotationDirection + " " + testTarget.transform.position);
            yield return new WaitForSeconds(0.75f);
        }
    }
*/
    /// <summary>
    /// Attack the target if its infront of the hero and the hero is not on movment,
    /// otherwise: if the hero is on movment, wait, if not tells the hero to lock on the target
    /// Author: Ilan
    /// </summary>
    private void prepareToAttack()
    {
        if (_movement.IsObjRotating()) // TO BE CHANGED // If moving / rotating toward the target, skip
            return;

        Vector3 targetPos = _targetToAttack.transform.position;      
        if (!_movement.IsLookingAtTheTarget(targetPos)) // if the target is not infront of the hero, tells it to rotate toward it
        {
            _movement.OnFinishMovment += heroManager;
            _movement.TargetLock(_targetToAttack, _skill.GetRange());
            //onFinishMovment += prepareToAttack; // subscribe it self, to start attack and the end of the rotation;
        }
        else  // if not rotating and the target is infornt of the hero, attack
            attack();

        heroManager();
    }

    private void attack()
    {
        _skill.attack();
    }

    /// <summary>
    /// WARNING!
    /// use it primarly for testing
    /// Stops all the commands, and delegations of the hero after the given delay
    /// Author: Ilan
    /// </summary>
    private void stopAllAfterDelay(float delay)
    {
        StopCoroutine(StopAfterDelay(delay));
    }

    /// <summary>
    /// WARNING!
    /// use it primarly for testing
    /// Stops all the commands, and delegations of the hero after the given delay.
    /// Author: Ilan
    /// </summary>
    private IEnumerator StopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stopAll();
    }

    /// <summary>
    /// WARNING!
    /// use it primary for testing.
    /// Stops all the commands, and delegations of the hero.
    /// Author: Ilan
    /// </summary>
    private void stopAll()
    {
        cancelOrders();
        OnTargetInFieldOfView = null;
        OnMove = null;
        OnHit = null;
        OnRespawn = null;
        OnDeath = null;
        onFinishMovment = null;
        onStartMovment = null;
        stop = true;
    }

    /// <summary>
    /// Cancel the hero orders
    /// Author: Ilan
    /// </summary>
    private void cancelOrders()
    {
        _targetObj = null;
        prepareForNewOrder();
    }

    /// <summary>
    /// Setup toward new order
    /// Author: Ilan
    /// </summary>
    private void prepareForNewOrder()
    {
        _targetToAttack = null;
        stopScanningIfTargetReachable();
        //desiredPos = transform.position; <<<<<================================================== the comment need to be removed
        _movement.StopMovment();
    }

    /*
private void setHeroToAttack(GameObject targetToAttack)
{
    this.targetToAttack = targetToAttack;
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
*/

    /// <summary>
    /// Sets the given target as the hero primary target to attack.
    /// Author: Ilan
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetObj(GameObject target)
    {
        this._targetObj = target;
        heroManager(); // to be implemented with delegation subscribe
    }

    private void heroManager() // stats the heroManager funcs with coroutine
    {
        StartCoroutine(activeHeroManager());
    }
    /// <summary>
    /// The hero logic
    /// Author: Ilan
    /// </summary>
    /// <returns></returns>
    private IEnumerator activeHeroManager() // WARNING - SUPER SENSETIVE, DO NOT EDIT
    {
        if (++testCounter == 10000) // wont preform any func after 10000 iterations (for testing)
            stopAll();
        else if (stop) // testing
            yield return new WaitForSeconds(100);


        yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE / 10);



        if (_targetObj != null && _movement.IsObjMoving())
            yield return null;
        /*
        if (targetsToAttackBank.Count == 0) // || targetToAttack != null)
            return;
            */

        if (_targetObj != null && _targetsToAttackBank.Contains(_targetObj) && _skill.isTargetAttackable(_targetObj)) // If the main target is not null and within the range, would start to attack
        {
            _movement.StopMovment();
            _targetToAttack = _targetObj;
            prepareToAttack();
        }
        else if(_targetObj != null && !_movement.IsObjMoving()) // If these a main target, and its not within the given range, would move toward it.
        {
            if(!TargetInRange(_targetObj))
                GoTo(_movement.CalcClosestPosWithThisDistance(_skill.GetRange(), transform.position, _targetObj.transform.position));
        }
        else if(!_movement.IsObjMoving() && !_movement.IsObjRotating() && isThereATargets()) // If not moving and there are targets in the targets bank
        { // NEED TO BE EDITED TO SUPPORT MULTI TARGETS WITHIN THE RANGE!! <==###########################
            if (_skill.isTargetAttackable(_targetsToAttackBank[0])) // If the first target is attackable, would start to attack
            {
                _targetToAttack = _targetsToAttackBank[0];
                prepareToAttack();
            }
            else // Would start scanning track the target.
            {
                startScanningIfTargetReachable(_targetsToAttackBank[0]);
                OnTargetInFieldOfView = heroManager;
            }
        }
        
    }
    /// <summary>
    /// Author: Ilan
    /// </summary>
    /// <param name="target"></param>
    private void testAddTargets(GameObject target) // A test function that track a target and add it to the hero targetsbank if it is within the skill range
    {
        if (target == null)
        {
            if (_targetsToAttackBank.Contains(target)) _targetsToAttackBank.Remove(target);
            return;
        }

        if (!_targetsToAttackBank.Contains(target))
        {
            if (_skill.isTargetInRange(target.transform.position)) {
                Test.SetTargetColor(target, Color.red);
                _targetsToAttackBank.Add(target);
                heroManager();
            }
        }
        else
        {
            if (!_skill.isTargetInRange(target.transform.position))
            {
                Test.SetTargetColor(target, Color.green);
                _targetsToAttackBank.Remove(target);
                heroManager();
            }
        }
    }

    /// <summary>
    /// Start the traking corutine to track the target
    /// Author: Ilan
    /// </summary>
    /// <param name="target">The target to be track</param>
    private void startScanningIfTargetReachable(GameObject target)
    {
        Debug.Log("startScan");
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(autoScan(target));
    }

    /// <summary>
    /// Stop the tracking corutine
    /// Author: Ilan
    /// </summary>
    private void stopScanningIfTargetReachable()
    {
        Debug.Log("stopScan");
        _isScanning = false;
        OnTargetInFieldOfView = null;
    }

    /// <summary>
    /// Start tracking after the target
    /// Author: Ilan
    /// </summary>
    /// <param name="target">Target to be track</param>
    /// <returns></returns>
    private IEnumerator autoScan(GameObject target)
    {
        Debug.Log("autoScan");
        bool isTargetAvailable = _skill.isTargetAttackable(target);
        while(!isTargetAvailable && _isScanning)
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }

        if (isTargetAvailable && OnTargetInFieldOfView != null)
            OnTargetInFieldOfView();
    }

    
    private bool isThereATargets()
    {
        return _targetsToAttackBank.Count > 0;
    }


    /// <summary>
    /// Check if the target within the skill range.
    /// If the target within the range, add the target to the hero targets bank
    /// Author: Ilan
    /// </summary>
    /// <param name="targetToAttack">Target to be checked</param>
    /// <returns>true if the target is within the skill range</returns>
    public bool TargetInRange(GameObject targetToAttack)
    {
        //addHeroesToAttackBank(targetToAttack);
        if (_skill.isTargetInRange(targetToAttack.transform.position))
        {
            _targetsToAttackBank.Add(targetToAttack);
            heroManager(); // need to be implemented with delegation
            return true;
        }

        return false;
    }


    /// <summary>
    /// this method command the hero unit to go to the desired location.
    /// Author: Dor
    /// </summary>
    public void GoTo(Vector3 desiredPos)
    {
        _movement.OnFinishMovment += heroManager;
        _movement.GoTo(desiredPos);
    }
    

    
}
