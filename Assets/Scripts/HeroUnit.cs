using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour
{
    Action<GameObject> OnRespawn = delegate { };                       // Notify that the hero has respawn
    Action<GameObject> OnTargetInFieldOfView = delegate { };                      // On scan End


    private int _id;
    private Skill _skill;
    private List<GameObject> _targetsToAttackBank;
    private GameObject _targetToAttack;             // This is the target to be attacked by the hero.
    private GameObject _targetObj;                 // The selected enemy to be attacked.
    private ObjStatus _status;                    // The hero order status (CURRECTLY NOT IN USE, MIGHT BE REMOVED)
    private bool _isScanning;
    private Movment _movement;                  // The movment component script
    private Scanner _scanner;
    private Health _health;


    // testing
    bool stop;
    int testCounter;
    [SerializeField] public static GameObject testTarget;
    [SerializeField] List<GameObject> testTargets; // only for testing
    [SerializeField] List<GameObject> testGameTargets; // only for testing

    public GameObject GetHeroTargetObj() => _targetObj; // Returns the hero target object

    /// Says good morning to the script
    private void Awake()
    {
        this._status = ObjStatus.idle;
        _targetToAttack = null;
        _targetsToAttackBank = new List<GameObject>();
        _skill = this.GetComponent<Skill>();
        _movement = this.GetComponent<Movment>();
        initScanner();


        testCounter = 0;
        stop = false;

    }

    void Start()
    {
        Test.DrawCircle(this.gameObject, _skill.GetRange() - 0.5f, 0.05f);
        
        StartCoroutine(testAttackTargets());

        StartCoroutine(testSelfDestroyAfterDelay(60f));
        
        StartCoroutine(Test.ActiveOnIntervals(manageHero, 0.05f));
    }

    private void initScanner()
    {
        _scanner = GetComponentInChildren<Scanner>();
        _scanner.OnObjEnter += AddEnemyToBank;
        _scanner.OnObjExit += RemoveEnemyFromBank;
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
        stopScannings();
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
        prepareForNewOrder(); // CHECK NEED TO BE CHANGED
        GoAfter(target); // CHECK NEED TO BE CHANGED
    }

    //************ Hero Logic - Start ****************

    /// <summary>
    /// A function that tells the hero what to do according to the his state
    /// </summary>
    private void manageHero()
    {
        if (_movement.IsObjOnMovment()) // If he hero is moving, its already preforming an action
        {
            return;
        }

        manageHeroIdle();
    }

    /// <summary>
    /// A function that tells to an idle hero what to do according to the his state
    /// </summary>
    private void manageHeroIdle()
    {
        if (_targetToAttack != null && _skill.isTargetAttackable(_targetToAttack)) // If hero has a target to attack, and it is able to attack it, do not distrub
            return;

        if(_targetObj != null) // If the hero has a targetObj, and its not attacking it, its probably not in the range.
        {
            prepareForNewOrder();
            GoAfter(_targetObj);
        }

        GameObject newTarget = findAnAttackableTarget(); // Checks if there an attackable target

        if (newTarget != null) // If the target escaped we change the target to the next item in the list
        {
            prepareForNewOrder();
            _targetToAttack = newTarget;
            prepareToAttack();       // How to attack, when, etc.
            return;
        }
        else
        {
            _targetToAttack = null; // There is no an available target, sets the _targetToAttack to null
            if (isThereATarget()) // If there a target in the attack range
            {
                OnTargetInFieldOfView += manageTargetAddDuringIdle; // Sets the manage hero at target add function, to be call if the a target become attackable
                startScanningForAnAttackableTarget(); // Start scanning for an attackable target
            }
        }
    }

    /// <summary>
    /// A function that tells to a hero, that preform a movment, what to do when a new target enter to attack range
    /// </summary>
    /// <param name="target">The new target that has been added to the hero bank</param>
    private void manageTargetAddDuringMovment(GameObject target)
    {
        if (target == _targetObj) // If the target is the _targetObj
        {
            if (_skill.isTargetAttackable(target)) // If the hero can attack the target - tells the hero to attack it
            {
                    prepareForNewOrder();
                    _targetToAttack = target;
                    prepareToAttack();
            }
            else // If the target is not attackable, tells the hero to start track the target, and if it become attackable, it would recall the function.
            {
                stopScannings();
                OnTargetInFieldOfView += manageTargetAddDuringMovment;
                startTrackIfATargetAttackable(_targetObj);
            }
        }
        //else if() // TO BE ADDED: case the hero can attack and move
    }

    /// <summary>
    /// A function that tells to an idle hero what to do when a new target enter to attack range
    /// </summary>
    /// <param name="target">The new target that has been added to the hero bank</param>
    private void manageTargetAddDuringIdle(GameObject target)
    {
        if ((_targetObj != null && _targetToAttack == _targetObj) || (_targetObj == null && _targetToAttack != null)) // If already attacking a target, keep doing it.
            return;

        if (_skill.isTargetAttackable(target)) // If the target is attackable, the hero would start to attack
        {
            if (target == _targetObj || _targetObj == null) // if I dont have a target, or its my _targetObj, make the enemy that entered the target
            {
                prepareForNewOrder();
                _targetToAttack = target;
                prepareToAttack(); // how to attack, when, etc.
            }
            /*
            else if (_targetToAttack == null)                    // if I dont have a target, make the enemy that entered the target
            {
                _targetToAttack = target;
                prepareToAttack();                         // how to attack, when, etc.
            }
            */
        }
        else
        {
            if (!_isScanning) // if not scanning, Start scanning for an attackable Targets
            {
                OnTargetInFieldOfView += manageTargetAddDuringIdle;  // If finds a target during scanning, calls the function again
                startScanningForAnAttackableTarget();
            }
        }
        
    }

    /// <summary>
    /// Tells the hero, that during movment, what to do if a target is no longer in its attack range
    /// </summary>
    /// <param name="target">The target that got out of range</param>
    private void manageTargetRemoveDuringMovment(GameObject target)
    {
        if (target == _targetObj) // If target escape
        {
            stopScannings(); // stop the target tracking, and the hero is probably keep moving after the target
            if(_targetObj == null) // If the target is dead
            {
                prepareForNewOrder();
                manageTargetRemoveDuringIdle(target);
            }
        }

    }

    /// <summary>
    /// Tells the idle hero what to do if a target is no longer in its attack range
    /// </summary>
    /// <param name="target">The target that got out of range</param>
    private void manageTargetRemoveDuringIdle(GameObject target)
    {
        if(_targetObj != null && target == _targetObj) // If target escape
        {
            prepareForNewOrder();
            GoAfter(target);
            return;
        }
        else if (target != _targetToAttack) // Some other target escaped, does not matter
            return;

        // IMPORTANT we assume that if we attack, and there is a targetObj, then we attack the targetObj, therefore the prevoius condtion would be trigger.

        manageHeroIdle();

    }

    //************ Hero Logic - END ****************

    //************ Hero Target Scanner ************* 
    
    /// <summary>
    /// Start the traking of a the obj target spesific target if its attackable
    /// Author: Ilan
    /// </summary>
    private void startTrackIfATargetAttackable(GameObject target)
    {
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(trackIfATargetAttackable(target));
    }

    /// <summary>
    /// A tracking function, that lock and target and keep checking if the target is attackable
    /// </summary>
    /// <returns></returns>
    public IEnumerator trackIfATargetAttackable(GameObject target)
    {
        while (_targetObj != null && !_skill.isTargetAttackable(target) && _isScanning)
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }
        //_targetsTo######AttackBank.Add()
        

        if (_isScanning && OnTargetInFieldOfView != null)
        {
            _isScanning = false;
            OnTargetInFieldOfView(target);
            OnTargetInFieldOfView = null;
        }
        _isScanning = false;
    }


    /// <summary>
    /// Start the traking corutine to track the targetes
    /// Author: Ilan
    /// </summary>
    /// <param name="target">The target to be track</param>
    private void startScanningForAnAttackableTarget()
    {
        Debug.Log("startScan");
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(autoScan());
    }

    /// <summary>
    /// Stop the tracking corutine
    /// Author: Ilan
    /// </summary>
    private void stopScannings()
    {
        //Debug.Log("stopScan");
        _isScanning = false;
        OnTargetInFieldOfView = delegate { };
    }

    /// <summary>
    /// Start tracking after the target
    /// Author: Ilan
    /// </summary>
    /// <param name="target">Target to be track</param>
    /// <returns></returns>
    private IEnumerator autoScan()
    {
        Debug.Log("autoScan");
        GameObject target = findAnAttackableTarget();

        while(target == null && _isScanning) // if scanning and target not found
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
            target = findAnAttackableTarget();

            if (!isThereATarget()) // in case that there are no more targets
                _isScanning = false;
        }

        _targetsToAttackBank.Remove(target);
        _targetsToAttackBank.Insert(0, target);


        if (_isScanning && target != null && OnTargetInFieldOfView != null)
        {
            _isScanning = false;
            OnTargetInFieldOfView(target);
            OnTargetInFieldOfView = delegate { };
        }
        _isScanning = false;
    }

    /// <summary>
    /// Search if there an attackable target within the targetbank
    /// </summary>
    /// <returns></returns>
    public GameObject findAnAttackableTarget()
    {
        foreach (GameObject target in _targetsToAttackBank)
        {
            if (target != null && _skill.isTargetAttackable(target))
                return target;
        }

        return null;
    }

    // ******************* Targets functions *******************
    /// <summary>
    /// adds an enemy to the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void AddEnemyToBank(GameObject enemy)
    {
        if (_targetsToAttackBank.Contains(enemy))   // check if the enemy is already in my bank (suppose to be always true)
            return;
        _targetsToAttackBank.Add(enemy);           // if not, add the enemy to the bank

        /*
        if (enemy == null)
            Debug.Log("Bug, null target");
        Debug.Log("manageTargetAdd: " + enemy.name);
        */

        if (_movement.IsObjMoving())
            manageTargetAddDuringMovment(enemy);
        else
            manageTargetAddDuringIdle(enemy);

    }

    /// <summary>
    /// removes an enemy from the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void RemoveEnemyFromBank(GameObject enemy)
    {
        if (!_targetsToAttackBank.Contains(enemy))      // check if the enemy is already in my bank (suppose to be always true)
            return;
        _targetsToAttackBank.Remove(enemy);        // if it is, remove the enemy from the bank


        if (enemy == null)
            Debug.Log("Bug, null target");
        Debug.Log("manageTargetAdd: " + enemy.name);


        if (_movement.IsObjMoving())
            manageTargetRemoveDuringMovment(enemy);
        else
            manageTargetRemoveDuringIdle(enemy);

    }


    private bool isThereATarget()
    {
        return _targetsToAttackBank.Count > 0;
    }

    // ******************** Movment Manage Functions *****************

    /// <summary>
    /// this method command the hero unit to go to the desired location.
    /// Author: Dor
    /// </summary>
    /// <param name="desiredPos">Pos to go to</param>
    public void GoTo(Vector3 desiredPos)
    {
        _movement.OnFinishMovment += manageHeroIdle;
        _movement.GoTo(desiredPos);
    }

    /// <summary>
    /// This function command the hero to follow a GameObject
    /// </summary>
    /// <param name="target">Enemy to follow</param>
    public void GoAfter(GameObject target)
    {
        //_movement.OnFinishMovment += heroManager;
        _movement.GoAfterTarget(target);
    }


    // ******************* Life Lost functions *******************
    private void initHeroHealth()
    {
        _health = GetComponent<Health>();
        _health.InitHealth(100f);
        _health.OnDeath += Die;
    }

    public void Die(GameObject hero)
    {
        this._status = ObjStatus.dead;   // change status to dead
        this._targetsToAttackBank = null;  // reset the bank of possible enemys in range
        this._targetObj = null;         // reset the target hero

        StartCoroutine(waitForRespawn());
    }

    /// <summary>
    /// wait for RESPAWN_TIME seconds before appearing again in the game
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private IEnumerator waitForRespawn()
    {
        yield return new WaitForSeconds(GlobalCodeSettings.RESPAWN_TIME);
        respawnHero();
    }

    /// <summary>
    /// returning the hero to the game after it was killed
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void respawnHero()
    {
        // repeating "Awake" function
        this._status = ObjStatus.idle;
        //_moveSpeed = 0.2f; // <============= To Check
        _targetsToAttackBank.Clear();

        _health.ResetHealth();
        //TODO: add a starting position

        OnRespawn(gameObject);            // tells all classes that it is respawning  
    }


    // ******************* Attack functions *******************
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
        if (!SpaceCalTool.IsLookingTowardsTheTarget(gameObject, targetPos, _movement._isHeightCalculated)) // if the target is not infront of the hero, tells it to rotate toward it
        {
            _movement.OnFinishMovment += prepareToAttack;
            _movement.TargetLock(_targetToAttack, _skill.GetRange());
            //onFinishMovment += prepareToAttack; // subscribe it self, to start attack and the end of the rotation;
        }
        else  // if not rotating and the target is infornt of the hero, attack
            attack();

        //heroManager();
        manageHero();
    }
    /*
    private void AutoAttack()
    {
        while (_skill.isTargetAttackable(_targetToAttack))
        {
            prepareToAttack();
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);

            if()
        }
        _targetToAttack = null;
    }
    */
    private void attack()
    {
        _skill.attack();
    }

    //********************* TESTING **************************
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
        OnTargetInFieldOfView = delegate { };
        OnRespawn = delegate { };
        stop = true;
        _movement.StopMovment();
        _isScanning = false;
    }

    public IEnumerator testAttackTargets()
    {
        foreach (GameObject target in testTargets)
        {
            SetTargetObj(target);
            while (target != null)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private IEnumerator testSelfDestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }



}
