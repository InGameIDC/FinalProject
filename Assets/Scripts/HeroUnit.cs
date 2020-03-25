using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour
{
    Action<HeroUnit> OnRespawn = delegate { };   //

    //******************* Life Lost Deligation *******************
    Action<HeroUnit, float> OnHit = delegate { };        // handles hero hit ( health > 0)
    Action<HeroUnit> OnDeath = delegate { };    // handles hero death (0 >= health)


    Action OnTargetInFieldOfView;                      // On scan End
    Action<HeroUnit> OnMove;                          //
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
        StartCoroutine(Test.ActiveOnIntervals(testScanForTargets, 0.5f));
        //StartCoroutine(Test.ActiveOnIntervals(heroManager, 0.05f));
        StartCoroutine(testAttackTargets());
        //StartCoroutine(Test.ActiveOnIntervals(_movement.StopMovment, 1f));
        //StartCoroutine(testPrintDirection());

    }

    private void Update() //// @@@@ TO BE REMOVED!!
    {
        heroManager();
        //testAddTargets(testTarget);
    }

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
        heroManager(); // to be implemented with delegation subscribe
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
            OnTargetInFieldOfView = heroManager;
            startTrackIfObjTargetAttackable();
            if (!TargetInRange(_targetObj))
                GoTo(_targetObj.transform.position);
                
        }
        else if(!isHeroMoving && !_movement.IsObjRotating() && isThereATargets()) // If not moving and there are targets in the targets bank
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
                OnTargetInFieldOfView = heroManager;
            }
        }
        
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
            OnTargetInFieldOfView();
            OnTargetInFieldOfView = null;
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
            if(target != null && _skill.isTargetAttackable(target))
                return target;
        }

        return null;
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
    private IEnumerator autoScan()
    {
        Debug.Log("autoScan");
        GameObject target = null;
        while(target == null && _isScanning)
        {
            target = findAnAttackableTarget();
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);

            if (!isThereATargets()) // in case that there are no more targets
                _isScanning = false;
        }

        _targetsToAttackBank.Remove(target);
        _targetsToAttackBank.Insert(0, target);

        _isScanning = false;

        if (target != null && OnTargetInFieldOfView != null)
        {
            OnTargetInFieldOfView();
            OnTargetInFieldOfView = null;
        }
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
        _movement.OnFinishMovment += heroManager;
        _movement.GoTo(desiredPos);
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
    /// <summary>
    /// Reduce XP when hit and checks if the player is dead as a result
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void TakeDamage(float damageValue)
    {
        _currentHeatlh -= damageValue;

        OnHit(this, _currentHeatlh); // tells all classes that it is bieng hit and how much (for display?)

        if(_currentHeatlh <= 0)      // if the XP is 0 or less the hero is dead
        {
            this._status = ObjStatus.dead;   // change status to dead
            this._targetsToAttackBank = null;  // reset the bank of possible enemys in range
            this._targetObj = null;         // reset the target hero
            OnDeath(this);                  // tells all classes that it is dead

            StartCoroutine(waitForRespawn());   // wait to respawn the hero
        }

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

        _currentHeatlh = _maxHealth;  // reset current health
        //TODO: add a starting position

        OnRespawn(this);            // tells all classes that it is respawning  
    }


    // ******************* Targets functions *******************
    /// <summary>
    /// adds an enemy to the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void addEnemyToBank(GameObject enemy)
    {

        if (!(_targetsToAttackBank.Contains(enemy)))   // check if the enemy is already in my bank (suppose to be always true)
        {
            _targetsToAttackBank.Add(enemy);           // if not, add the enemy to the bank
        }

        if(_targetToAttack == null)                    // if I dont have a target, make the enemy that entered the target
        {
            _targetToAttack = enemy;
            attackEnemy();                          // how to attack, when, etc.
        }
        
    }

    /// <summary>
    /// removes an enemy from the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void removeEnemyFromBank(GameObject enemy)
    {
        if (_targetsToAttackBank.Contains(enemy))      // check if the enemy is already in my bank (suppose to be always true)
        {
            _targetsToAttackBank.Remove(enemy);        // if it is, remove the enemy from the bank
        }

        if (_targetToAttack == enemy)                  // if the target was the enemy we change the target to the next item in the list
        {
            if (_targetsToAttackBank.Count != 0)
            {
                _targetToAttack = _targetsToAttackBank[0];
                attackEnemy();
            }
            else
            {
                _targetToAttack = null;
            }
        }

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
