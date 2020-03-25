using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour
{
    Action<GameObject> OnRespawn = delegate { };                       // Notify that the hero has respawn
    Action<GameObject> OnTargetInFieldOfView = delegate { };                      // On scan End
    Action<HeroUnit> OnMove = delegate { };                          //
    // private Action<HeroUnit> //onFinishAction;    // Functions that would be preform when the hero finish an action;
    private Action onFinishMovment = delegate { };                 // Function that would be preform when the hero finish to move / rotate
    private Action onStartMovment = delegate { };                 // Function that would be preform when the hero start to move / rotate
    //private Action<HeroUnit> onFinishRotate;    // Function that would be preform when the finish to rotate

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

    /// Says good morning to the script
    private void Awake()
    {
        this._status = ObjStatus.idle;
        _targetToAttack = null;
        _targetsToAttackBank = new List<GameObject>();
        _skill = this.GetComponent<Skill>();
        _movement = this.GetComponent<Movment>();

        _scanner = GetComponentInChildren<Scanner>(); // To Be Removed!!

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
        Test.DrawCircle(this.gameObject, _skill.GetRange() - 0.5f, 0.05f);
        //GoTo(_movement.GetXZposRelativeVector(new Vector3(6f, 0f, -8f)));
        //StartCoroutine(Test.ActiveOnIntervals(testScanForTargets, 0.5f));
        //StartCoroutine(Test.ActiveOnIntervals(heroManager, 0.05f));
        StartCoroutine(testAttackTargets());
        //StartCoroutine(Test.ActiveOnIntervals(_movement.StopMovment, 1f));
        //StartCoroutine(testPrintDirection());
        StartCoroutine(testSelfDestroyAfterDelay(60f));
        StartCoroutine(Test.ActiveOnIntervals(manageHero, 0.05f));
    }

    private IEnumerator testSelfDestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Update() //// @@@@ TO BE REMOVED!!
    {
        //heroManager();
        //testAddTargets(testTarget);
    }

    public GameObject GetHeroTargetObj() => _targetObj;

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
            _movement.OnFinishMovment += prepareToAttack;
            _movement.TargetLock(_targetToAttack, _skill.GetRange());
            //onFinishMovment += prepareToAttack; // subscribe it self, to start attack and the end of the rotation;
        }
        else  // if not rotating and the target is infornt of the hero, attack
            attack();

        //heroManager();
        manageHero();
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
        OnTargetInFieldOfView = delegate { };
        OnMove = delegate { };
        OnRespawn = delegate { };
        onFinishMovment = delegate { };
        onStartMovment = delegate { };
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
        stopScannings();
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
        GoAfter(target); // FOR TESTING
        //heroManager(); // to be implemented with delegation subscribe
    }

    private void heroManager() // stats the heroManager funcs with coroutine
    {
        activeHeroManager();
        //StartCoroutine(activeHeroManager());
    }
    /// <summary>
    /// The hero logic
    /// Author: Ilan
    /// </summary>
    /// <returns></returns>
    private void activeHeroManager() // WARNING - SUPER SENSETIVE, DO NOT EDIT
    {
        if (++testCounter == 500000)
        { // wont preform any func after 10000 iterations (for testing)
            stopAll();
            Destroy(this);
        }
        else if (stop) // testing
            return; //yield return new WaitForSeconds(100);


        //yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE / 10);

        bool isHeroMoving = _movement.IsObjMoving();
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
        else if (_targetObj != null && isHeroMoving)
        {
            return;//yield return null;
        }
        else if(_targetObj != null && !isHeroMoving) // If these a main target, and its not within the given range, would move toward it.
        {
            /*
            if(!TargetInRange(_targetObj))
                GoTo(_movement.CalcClosestPosWithThisDistance(_skill.GetRange(), transform.position, _targetObj.transform.position));
            */
            prepareForNewOrder();
            //OnTargetInFieldOfView = heroManager; <===== TO BE FIXED (WONT WORK WITH OUT THIS)
            startTrackIfObjTargetAttackable();
            if (!TargetInRange(_targetObj))
                GoTo(_targetObj.transform.position);
                
        }
        else if(!isHeroMoving && !_movement.IsObjRotating() && isThereATarget()) // If not moving and there are targets in the targets bank
        { // NEED TO BE EDITED TO SUPPORT MULTI TARGETS WITHIN THE RANGE!! <==###########################
            GameObject target = findAnAttackableTarget();
            if (target != null) // If the first target is attackable, would start to attack
            {
                _targetToAttack = target;
                prepareToAttack();
            }
            else // Would start scanning track the target.
            {
                startScanningForAnAttackableTarget();
                //OnTargetInFieldOfView = heroManager; <===== TO BE FIXED(WONT WORK WITH OUT THIS)
            }
        }
        
    }

    private void manageHero()
    {
        if (_movement.IsObjOnMovment())
        {
            return;
        }

        manageHeroIdle();
    }

    private void manageHeroIdle()
    {
        if (_targetToAttack != null) // If hero is attacking, do not disturb
            return;

        if(_targetObj != null)
        {
            prepareForNewOrder();
            GoTo(_targetObj.transform.position);
        }

        GameObject newTarget = findAnAttackableTarget();

        if (newTarget != null) // if the target escaped we change the target to the next item in the list
        {
            prepareForNewOrder();
            _targetToAttack = _targetsToAttackBank[0];
            prepareToAttack();       // how to attack, when, etc.
            return;
        }
        else
        {
            _targetToAttack = null;
            if (isThereATarget())
            {
                OnTargetInFieldOfView += manageTargetAddDuringIdle;
                startScanningForAnAttackableTarget();
            }
        }
    }

    private void manageTargetAddDuringMovment(GameObject target)
    {
        if (target == _targetObj)
        {
            if (_skill.isTargetAttackable(target))
            {
                    prepareForNewOrder();
                    _targetToAttack = target;
                    prepareToAttack();
            }
            else
            {
                stopScannings();
                OnTargetInFieldOfView += manageTargetAddDuringMovment;
                startTrackIfObjTargetAttackable();
            }
        }
        //else if() // TO BE ADDED: case the hero can attack and move
    }

    private void manageTargetAddDuringIdle(GameObject target)
    {
        if ((_targetObj != null && _targetToAttack == _targetObj) || (_targetObj == null && _targetToAttack != null)) // If already attacking a target, keep doing it.
            return;

        if (_skill.isTargetAttackable(target))
        {
            if (target == _targetObj)
            {
                prepareForNewOrder();
                _targetToAttack = target;
                prepareToAttack();
            }
            else if (_targetToAttack == null)                    // if I dont have a target, make the enemy that entered the target
            {
                _targetToAttack = target;
                prepareToAttack();                         // how to attack, when, etc.
            }
        }
        else
        {
            if (!_isScanning) // if not scanning, Start scanning for an attackable Targets
            {
                OnTargetInFieldOfView += manageTargetAddDuringIdle; 
                startScanningForAnAttackableTarget();
            }
        }
        
    }

    private void manageTargetRemoveDuringMovment(GameObject target)
    {
        if(target == _targetObj) // If target escape
            stopScannings();

        if (_targetObj == null)
        {
            prepareForNewOrder();
            manageTargetRemoveDuringIdle(target);
        }
    }

    private void manageTargetRemoveDuringIdle(GameObject target)
    {
        if(_targetObj != null && target == _targetObj) // If target escape
        {
            prepareForNewOrder();
            GoAfter(target);
            return;
        }
        else if (target != _targetToAttack) // Some other target escaped, 
            return;

        // IMPORTANT we assume that if we attack, and there is a targetObj, then we attack the targetObj, therefore the prevoius condtion would be trigger.

        manageHeroIdle();

    }



    /// <summary>
    /// Start the traking of a the obj target spesific target if its attackable
    /// Author: Ilan
    /// </summary>
    private void startTrackIfObjTargetAttackable()
    {
        if (_isScanning)
            return;

        _isScanning = true;
        StartCoroutine(trackIfObjTargetAttackable());
    }

    public IEnumerator trackIfObjTargetAttackable()
    {
        while (_targetObj != null && !_skill.isTargetAttackable(_targetObj))
        {
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }
        //_targetsTo######AttackBank.Add()
        _isScanning = false;

        if (OnTargetInFieldOfView != null)
        {
            OnTargetInFieldOfView(_targetObj);
            OnTargetInFieldOfView = null;
        }
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
        GameObject target = null;
        while(target == null && _isScanning)
        {
            target = findAnAttackableTarget();
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);

            if (!isThereATarget()) // in case that there are no more targets
                _isScanning = false;
        }

        _targetsToAttackBank.Remove(target);
        _targetsToAttackBank.Insert(0, target);

        _isScanning = false;

        if (target != null && OnTargetInFieldOfView != null)
        {
            OnTargetInFieldOfView(target);
            OnTargetInFieldOfView = delegate { };
        }
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

    private bool isThereATarget()
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
            //_targetsToAttackBank.Add(targetToAttack);
            //heroManager(); // need to be implemented with delegation
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
        //_movement.OnFinishMovment += heroManager;
        _movement.GoTo(desiredPos);
    }

    public void GoAfter(GameObject target)
    {
        //_movement.OnFinishMovment += heroManager;
        _movement.GoAfterTarget(target);
    }

    /// <summary>
    /// Author: Ilan
    /// </summary>
    /// <param name="target"></param>
    private void testAddTarget(GameObject target) // A test function that track a target and add it to the hero targetsbank if it is within the skill range
    {
        if (target == null)
        {
            if (_targetsToAttackBank.Contains(target)) _targetsToAttackBank.Remove(target);
            return;
        }

        if (!_targetsToAttackBank.Contains(target))
        {
            if (_skill.isTargetInRange(target.transform.position))
            {
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
    /// Run the testAddTargets on the testTargets list
    /// </summary>
    public void testScanForTargets()
    {
        foreach (GameObject target in testGameTargets)
        {
            testAddTarget(target);
        }
    }

    public IEnumerator testAttackTargets()
    {
        foreach(GameObject target in testTargets)
        {
            SetTargetObj(target);
            while(target != null)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
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


    // ******************* Attack functions *******************
    /// <summary>
    /// removes an enemy from the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void attackEnemy()
    {

    }

}
