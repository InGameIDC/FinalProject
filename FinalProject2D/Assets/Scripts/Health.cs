using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{

    public Action<GameObject, float> OnHit = delegate { };        // handles object hit ( health > 0)
    public Action<GameObject> OnDeath = delegate { };    // handles object death (0 >= health)

    // Used for death animation.
    [SerializeField] private GameObject deathAnimation;

    private float _currentHeatlh = 4;   //For now to the health bar to work
    [SerializeField] public float _maxHealth = 4;       //For now to the health bar to work
    public float GetCurrentHealth() => _currentHeatlh;
    private void Awake()
    {

    }

    private void Start()
    {
        HeroData data = GetComponentInParent<HeroDataManage>().GetData();
        _maxHealth = data.getMaxHealth();
        _currentHeatlh = _maxHealth;
    }
   


    /// <summary>
    /// Initiate health parameters
    /// Author: OrS
    /// </summary>
    /// <param name="maxHealth"></param>
    public void InitHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHeatlh = _maxHealth;
    }

    /// <summary>
    /// Reduce HP when hit and checks if the obj is dead as a result
    /// </summary>
    /// <param name="damageValue">The health to reduce</param>
    public void TakeDamage(float damageValue)
    {
        _currentHeatlh -= damageValue;

        transform.parent.gameObject.GetComponentInChildren<SimpleHealthBar>().UpdateBar(_currentHeatlh, _maxHealth);    //updae the life Bar

        OnHit(transform.parent.gameObject, _currentHeatlh); // tells all classes that it is bieng hit and how much (for display?)
        // Play hit sound
        try
        {
            SoundManager.Instance.PlaySound(Sound.HeroHit);
        }
        catch
        {
            Debug.Log("No Sound for hit was found");
        }

        if (_currentHeatlh <= 0)      // if the XP is 0 or less the hero is dead
        {


            //Destroy(gameObject);    //For Testing

            //ResetHealth();
            playDeathAnimation();
            OnDeath(transform.parent.gameObject);        // tells all classes that it is dead
            // Play death sound
            try
            {
                SoundManager.Instance.PlaySound(Sound.HeroDie);
            }
            catch
            {
                Debug.Log("No Sound for death was found");
            }
            transform.parent.gameObject.SetActive(false);


        }

    }

    /// <summary>
    /// in case of healing aading to the health.
    /// </summary>
    /// <param name="amount">the amount to add to the health</param>
    public void RegenerateHealth(float amount)
    {
        _currentHeatlh += amount;

        if (_currentHeatlh > _maxHealth)
            _currentHeatlh = _maxHealth;
    }

    /// <summary>
    /// reset the health parameters in case of death
    /// </summary>
    public void ResetHealth()
    {
        _currentHeatlh = _maxHealth;
        transform.parent.gameObject.GetComponentInChildren<SimpleHealthBar>().UpdateBar(_currentHeatlh, _maxHealth);
    }

    public void playDeathAnimation()
    {
        if (deathAnimation != null)
        {
            Vector3 heroPosition = this.transform.parent.position;
            Instantiate(deathAnimation, heroPosition, Quaternion.identity);
        }
    }
}
