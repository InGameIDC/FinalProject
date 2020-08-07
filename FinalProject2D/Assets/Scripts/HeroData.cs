using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Data", menuName = "Hero Data", order = 51)]
public class HeroData : ScriptableObject
{
    // Unit main stats
    [SerializeField]
    private int _heroId;
    [SerializeField]
    private Team _team; 
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _cooldown;
    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private float _projSpeed;
    [SerializeField]
    private float _projectileOffsetValue; // use mainly for melee, move around the rotator path

    // Stats gain per level up:
    [SerializeField]
    private float _levelUpGain_moveSpeed = 0;
    [SerializeField]
    private float _levelUpGain_maxHealth = 0;
    [SerializeField]
    private float _levelUpGain_damage = 0;
    [SerializeField]
    private float _levelUpGain_cooldown = 0;
    [SerializeField]
    private float _levelUpGain_projSpeed = 0;
    [SerializeField]
    private float _levelUpGain_range = 0;

    // Unit Image
    [SerializeField]
    private Sprite _heroImage = null;

    [SerializeField]
    private int[] upgradeCosts = {100,100,100,100 };
    [SerializeField]
    private int[] starsToUpgrade = { 0, 0, 0, 0 };
    [SerializeField]
    private int cardStatus = 0; //0 - locked, 1 - open, 2 - inuse, 3-readyToBuy
    [SerializeField]
    private int upgradestatus = 0; //0 - notReady, 1 - ready
    [SerializeField]
    private int starsToBuy = 0;



    public int getHeroId() => _heroId;
    public Team getTeam() => _team;
    public float getMovementSpeed() => _moveSpeed + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_moveSpeed);
    public float getMaxHealth() => _maxHealth + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_maxHealth);
    public float getDamage() => _damage + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_damage);
    public float getRange() => _range + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_range);
    public float getCooldown() => _cooldown + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_cooldown);
    public GameObject getProjectile() => _projectile;
    public float getProjSpeed() => _projSpeed + ((PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1) - 1) * _levelUpGain_projSpeed);
    public float getProjectileOffsetValue() => _projectileOffsetValue;
    public Sprite getHeroImage() => _heroImage;
    public int getUpgradeCost() => upgradeCosts[(PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1))];
    public int getStarsToUpgrade() => starsToUpgrade[(PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", 1))];
    public int getCardStatus() => cardStatus;
    public void setCardStatus(int cardStat)
    {
        cardStatus = cardStat;
    }
    public int getUpgradestatus() => upgradestatus;
    public void setUpgradestatus(int upgradeStat)
    {
        upgradestatus = upgradeStat;
    }
    public int getStarsToBuy() => starsToBuy;

    public int getHeroLevel() => PlayerPrefs.GetInt("Hero_" + _heroId + "_Level", -1);
    public void setHeroLevel(int level) => PlayerPrefs.SetInt("Hero_" + _heroId + "_Level", level);



}
