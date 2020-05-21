using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScannerIn : MonoBehaviour
{
    public Action<GameObject> OnObjEnter = delegate { };

    /// <summary>
    /// When an object is entering the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy entered its range 
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //GameObject unit = collider.GetComponent<GameObject>();
        GameObject unit = collider.gameObject;
        if (TeamTool.isEnemy(transform.parent.gameObject, unit))    // if the heroUnit is an enemy
        {
            OnObjEnter(unit);    // tells all other classes which hero scanned a new enemy and who is the enemy
        }
    }

}
