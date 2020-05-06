using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class TargetsBank : MonoBehaviour
{
    public Action<GameObject> OnAddTargetToBank = delegate { };
    public Action<GameObject> OnRemoveTargetFromBank = delegate { };

    public List<GameObject> _targetsToAttackBank;
    private bool _isScanning;

    private void Awake()
    {
        _targetsToAttackBank = new List<GameObject>();
    }

    public bool isThereATarget()
    {
        return _targetsToAttackBank.Count > 0;
    }

    // ******************* Targets functions *******************
    /// <summary>
    /// adds an enemy to the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void AddTargetToBank(GameObject target)
    {
        Debug.Log("add to bank " + target.GetType());
        if (_targetsToAttackBank.Contains(target))   // check if the enemy is already in my bank (suppose to be always true)
            return;
        _targetsToAttackBank.Add(target);           // if not, add the enemy to the bank
        removeEnemyOnEnemyDeath(target);
        /*
        if (enemy == null)
            Debug.Log("Bug, null target");
        Debug.Log("manageTargetAdd: " + enemy.name);
        */

        if (OnAddTargetToBank != null)
            OnAddTargetToBank(target);

    }

    /// <summary>
    /// removes an enemy from the enemy bank
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void RemoveTargetFromBank(GameObject target)
    {
        if (!_targetsToAttackBank.Contains(target))      // check if the enemy is already in my bank (suppose to be always true)
            return;
        _targetsToAttackBank.Remove(target);        // if it is, remove the enemy from the bank


        if (target == null)
            Debug.Log("Bug, null target");
        Debug.Log("manageTargetRemove: " + target.name);


        if (OnRemoveTargetFromBank != null)
            OnRemoveTargetFromBank(target);
    }

    public ReadOnlyCollection<GameObject> GetTargetsList()
    {
        return new ReadOnlyCollection<GameObject>(_targetsToAttackBank);
    }
    
    private void removeEnemyOnEnemyDeath(GameObject enemy)
    {
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
            enemyHealth.OnDeath += RemoveTargetFromBank;
    }

    public bool isTargetInBank(GameObject target)
    {
        return _targetsToAttackBank.Contains(target);
    }
}
