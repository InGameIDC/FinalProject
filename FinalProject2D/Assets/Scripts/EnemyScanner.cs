using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScanner : MonoBehaviour
{
    public Action<GameObject> OnObjEnter = delegate { };
    public Action<GameObject> OnObjExit = delegate { };

    /// <summary>
    /// When an object is entering the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy entered its range
    /// Author: OrS
    /// </summary>
    /// <param name="collision">The object that enterd the collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject unit = collision.gameObject;
        //Debug.Log(gameObject.name + ": something enterd"); //For Testing OrS

        if (unit.tag == "HeroUnit")
        {
            OnObjEnter(unit);
            //Debug.Log(gameObject.name + ": Hero enterd"); //For Testing OrS
        }
    }


    /// <summary>
    /// When an object is exiting the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy exited its range 
    /// Author: OrS
    /// </summary>
    /// <param name="collision">The object that exited the collider</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject unit = collision.gameObject;
        //Debug.Log(gameObject.name + ": something exited");//For Testing OrS

        if (unit.tag == "HeroUnit")
        {
            OnObjExit(unit);
            //Debug.Log(gameObject.name + ": Hero exited"); //For Testing OrS
        }
    }
}
