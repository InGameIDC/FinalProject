using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{

    //******************* Life Lost Deligation *******************
    public Action<GameObject, float> OnHit = delegate { };        // handles object hit ( health > 0)
    public Action<GameObject> OnDeath = delegate { };    // handles object death (0 >= health)

    //private float _currentHeatlh;
    //private float _maxHealth;

    public float _currentHeatlh = 2;   //For now to the health bar to work
    public float _maxHealth = 2;       //For now to the health bar to work


    // ******************* Life Lost functions *******************
    /// <summary>
    /// Reduce HP when hit and checks if the obj is dead as a result
    /// Author: OrS
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void TakeDamage(float damageValue)
    {
        _currentHeatlh -= damageValue;

        GetComponentInChildren<SimpleHealthBar>().UpdateBar(_currentHeatlh, _maxHealth);    //updae the life Bar

        OnHit(gameObject, _currentHeatlh); // tells all classes that it is bieng hit and how much (for display?)

        if (_currentHeatlh <= 0)      // if the XP is 0 or less the hero is dead
        {
            OnDeath(gameObject);        // tells all classes that it is dead

            ResetHealth();
            //Destroy(gameObject);    //For Testing
        }

    }

    public void InitHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHeatlh = _maxHealth;
    }

    public void RegenerateHealth(float amount)
    {
        _currentHeatlh += amount;

        if (_currentHeatlh > _maxHealth)
            _currentHeatlh = _maxHealth;
    }

    public void ResetHealth()
    {
        _currentHeatlh = _maxHealth;
        GetComponentInChildren<SimpleHealthBar>().UpdateBar(_currentHeatlh, _maxHealth);
    }

}
