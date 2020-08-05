using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScannerOut : MonoBehaviour
{
    public Action<GameObject> OnObjExit = delegate { };

    private void Start()
    {
        HeroData data = GetComponentInParent<HeroData>();
        GetComponent<CircleCollider2D>().radius = data.getRange();
    }
    /// <summary>
    /// When an object is exiting the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy exited its range 
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void OnTriggerExit2D(Collider2D collider)
    {
        //GameObject unit = collider.GetComponent<GameObject>();
        GameObject unit = collider.gameObject;

        if (TeamTool.isEnemy(gameObject, unit))    // if the heroUnit is an enemy
        {
            OnObjExit(unit);    // tells all other classes which hero scanned an enemy that exited its range and who is the enemy
        }
    }
}
