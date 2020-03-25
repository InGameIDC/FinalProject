using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// checks for enemeys entering into the heros range of attack
public class Scanner : MonoBehaviour
{
    public Action <HeroUnit, HeroUnit> onHeroEnter = delegate { };
    public Action <HeroUnit, HeroUnit> onHeroExit = delegate { };

    /// <summary>
    /// When an object is entering the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy entered its range 
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void OnTriggerEnter(Collider collider)
    {
        HeroUnit unit = collider.GetComponent<HeroUnit>();

        if(unit != null)    // if the object that entered is a HeroUnit
        {
            if (unit.tag == "Enemy")    // if the heroUnit is an enemy
            {
                HeroUnit hero = transform.parent.GetComponent<HeroUnit>();  //get the parent of the scanner (get the heroUnit the scanner belongs to)
                onHeroEnter(hero, unit);    // tells all other classes which hero scanned a new enemy and who is the enemy
            }
        }
    }

    /// <summary>
    /// When an object is exiting the range, the function check if it is an enemy, and if so it tells all other classes that an 
    /// enemy exited its range 
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void OnTriggerExit(Collider collider)
    {
        HeroUnit unit = collider.GetComponent<HeroUnit>();

        if (unit != null)    // if the object that exited is a HeroUnit
        {
            if (unit.tag == "Enemy")    // if the heroUnit is an enemy
            {
                HeroUnit hero = transform.parent.GetComponent<HeroUnit>();  //get the parent of the scanner (get the heroUnit the scanner belongs to)
                onHeroExit(hero, unit);    // tells all other classes which hero scanned an enemy that exited its range and who is the enemy
            }
        }
    }
}
