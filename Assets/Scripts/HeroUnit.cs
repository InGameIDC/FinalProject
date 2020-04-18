using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ObjStatus { dead, siege, moving, idle, attacking, moveAndAttack, rotating, moveAndRotate, moveAndAttackAndRotate, rotateAndAttack};


public class HeroUnit : MonoBehaviour 
{
    public Action<GameObject> OnRespawn = delegate { };                       // Notify that the hero has respawn

    private int _id;
    private Skill _skill;
    private GameObject _targetToAttack;             // This is the target to be attacked by the hero.
    private GameObject _targetObj;                 // The selected enemy to be attacked.
    private ObjStatus _status;                    // The hero order status (CURRECTLY NOT IN USE, MIGHT BE REMOVED)
    private bool _isScanning;
    private Movment _movement;                  // The movment component script
    private Scanner _scanner;
    private Health _health;
    private TargetsBank _targetsBank;
    private TargetFinder _targetFinder;
    private float _respawnTime;
    [SerializeField] public Team heroTeam;

    public GameObject GetHeroTargetObj() => _targetObj; // Returns the hero target object

    /// Says good morning to the script
    private void Awake()
    {
        gameObject.tag = Enum.GetName(typeof(Team), heroTeam);
        this._status = ObjStatus.idle;
        _targetToAttack = null;
        _skill = this.GetComponent<Skill>();
        _movement = this.GetComponent<Movment>();
        initTargetsBank();
        _targetsBank = this.GetComponent<TargetsBank>();
        _targetFinder = this.GetComponent<TargetFinder>();
        _targetFinder.OnTargetDeath += manageTargetObjDeath;
        initScanner();
        initHeroHealth();
    }

    void Start()
    {
        StartCoroutine(Test.ActiveOnIntervals(manageHero, 0.05f));
        Test.DrawCircle(this.gameObject, _skill.GetRange() - 0.5f, 0.05f);
        //StartCoroutine(testSelfDestroyAfterDelay(60f));
    }    

    #region Inits
    private void initTargetsBank()
    {
        _targetsBank = this.GetComponent<TargetsBank>();
        _targetsBank.OnAddTargetToBank += manageTargetAddToBank;
        _targetsBank.OnRemoveTargetFromBank += manageTargetRemoveFromBank;
    }
    private void initScanner()
    {
        _scanner = GetComponentInChildren<Scanner>();
        _scanner.OnObjEnter += _targetsBank.AddTargetToBank;
        _scanner.OnObjExit += _targetsBank.RemoveTargetFromBank;
    }

    private void initHeroHealth()
    {
        _health = GetComponent<Health>();
        //_health.InitHealth(100f);
        _health.OnDeath += Die;
        // TO BE CHANGED, TO FIND BATTLEMANAGER WITHOUT FIND!!
        _health.OnDeath += GameObject.Find("BattleManager").GetComponent<BattleManager>().DieAndRespawn;
    }
    #endregion

    #region Hero Ctrl

    // ******************** Orders Cancel Functions *****************
    /// <summary>
    /// Cancel the hero orders
    /// Author: Ilan
    /// </summary>
    public void CancelOrders()
    {
        _targetObj = null;
        _targetFinder.stopTackIfTargetAlive();
        prepareForNewOrder();
    }

    /// <summary>
    /// Setup toward new order
    /// Author: Ilan
    /// </summary>
    private void prepareForNewOrder()
    {
        _targetToAttack = null;
        _targetFinder.stopScannings();
        _movement.StopMovment();
    }

    // ******************** START - Movment Manage Functions *****************

    /// <summary>
    /// this method command the hero unit to go to the desired location.
    /// Author: Dor
    /// </summary>
    /// <param name="desiredPos">Pos to go to</param>
    public void GoTo(Vector3 desiredPos)
    {
        CancelOrders();
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
    // ******************** END - Movment Manage Functions *****************

    /// <summary>
    /// Sets the given target as the hero primary target to attack.
    /// Author: Ilan
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetObj(GameObject target)
    {
        CancelOrders(); // CHECK NEED TO BE CHANGED
        this._targetObj = target;
        _targetFinder.StartTrackIfTargetAlive(target);
        GoAfter(target); // CHECK NEED TO BE CHANGED
    }
    #endregion

    #region Hero Logic

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

        GameObject newTarget = _targetFinder.findAnAttackableTarget(_targetsBank.GetTargetsList(), _skill); // Checks if there an attackable target

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
            if (_targetsBank.isThereATarget()) // If there a target in the attack range
            {
                _targetFinder.OnTargetInFieldOfView += manageTargetAddDuringIdle; // Sets the manage hero at target add function, to be call if the a target become attackable
                _targetFinder.startScanningForAnAttackableTarget(_targetsBank.GetTargetsList(), _skill); // Start scanning for an attackable target
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
                _targetFinder.stopScannings();
                _targetFinder.OnTargetInFieldOfView += manageTargetAddDuringMovment;
                _targetFinder.startTrackIfATargetAttackable(_targetObj, _skill);
            }
        }
        //else if() // TO BE ADDED: case the hero can attack and move
    }


    private void manageTargetAddToBank(GameObject target)
    {
        if (_movement.IsObjMoving())
            manageTargetAddDuringMovment(target);
        else
            manageTargetAddDuringIdle(target);
    }

    private void manageTargetRemoveFromBank(GameObject target)
    {
        if (_movement.IsObjMoving())
            manageTargetRemoveDuringMovment(target);
        else
            manageTargetRemoveDuringIdle(target);
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
                _targetFinder.OnTargetInFieldOfView += manageTargetAddDuringIdle;  // If finds a target during scanning, calls the function again
                _targetFinder.startScanningForAnAttackableTarget(_targetsBank.GetTargetsList(), _skill);
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
            _targetFinder.stopScannings(); // stop the target tracking, and the hero is probably keep moving after the target
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

    private void manageTargetObjDeath()
    {
        CancelOrders();
        manageHeroIdle();
    }

    //************ Hero Logic - END ****************
    #endregion

    #region Health Related
    // ******************* Life Lost functions *******************
    public void Die(GameObject hero)
    {
        //this._status = ObjStatus.dead;   // change status to dead
        this._targetObj = null;         // reset the target hero
    }

    /// <summary>
    /// returning the hero to the game after it was killed
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    //private void respawnHero() { //OnRespawn(gameObject); }            // tells all classes that it is respawning  
    #endregion

    #region Attack Manage
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
            //_movement.OnFinishMovment += prepareToAttack;
            _movement.OnFinishMovment += attack;
            _movement.TargetLock(_targetToAttack, _skill.GetRange());
            //onFinishMovment += prepareToAttack; // subscribe it self, to start attack and the end of the rotation;
        }
        else  // if not rotating and the target is infornt of the hero, attack
            attack();

        manageHero();
    }
    // TO BE ADDED: AUTO ATTACK FROM ARCHIVE
    private void attack(){ _skill.attack(); }
    #endregion

    #region Testing
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
        CancelOrders();
        OnRespawn = delegate { };
        _movement.StopMovment();
        _isScanning = false;
    }
    private IEnumerator testSelfDestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    #endregion
}
