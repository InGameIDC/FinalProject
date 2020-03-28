using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetBank : MonoBehaviour
{
    Action<GameObject> OnAddTargetToBank = delegate { };
    Action<GameObject> OnRemoveTargetFromBank = delegate { };

    private List<GameObject> _targetsToAttackBank;
    private bool _isScanning;

    private void Awake()
    {
        _targetsToAttackBank = new List<GameObject>();
    }

    private bool isThereATarget()
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

        if (OnAddTargetToBank != null)
            OnAddTargetToBank(enemy);

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


        if (OnRemoveTargetFromBank != null)
            OnRemoveTargetFromBank(enemy);
    }


}
