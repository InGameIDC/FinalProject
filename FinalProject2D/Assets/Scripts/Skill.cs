using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{
    public Action<GameObject, Vector3> OnAttack = delegate { }; // calls on attack

    private float _id;
    private float _damageMultiplier;
    [SerializeField]  private float _damage = 1f;
    [SerializeField] float _range = 5f;
    [SerializeField] private float _projSpeed = 5f;
    [SerializeField] private float _cooldown = 2f;
    private float _cooldownStartTime = 0f;
    private bool _canAttackOnMovment = false;
    private bool _needToBeAimed = true; // if ture, the target has to rotate toward the target
    [SerializeField] GameObject projectile;
    [SerializeField] private float _projectileOffsetValue = 0f; // use mainly for melee, move around the rotator path
    private GameObject _firePoint; // The projectile spawn location

    private void Awake()
    {
        initFirePoint();
    }

    // Start is called before the first frame update
    void Start()
    {
        HeroData data = GetComponentInParent<HeroDataManage>().GetData();
        _damage = data.getDamage();
        _range = data.getRange();
        _projSpeed = data.getProjSpeed();
        _cooldown = data.getCooldown();
        projectile = data.getProjectile();
        _projectileOffsetValue = data.getProjectileOffsetValue();
    }

    public bool CanAttackOnMovment() => _canAttackOnMovment;
    public bool IsNeedToBeAimed() => _needToBeAimed;
    public float GetRange() => _range;
    public bool IsOnCooldown() => Time.time - _cooldownStartTime < _cooldown;

    public float GetSkillCooldown() => _cooldown;

    private void initFirePoint()
    {
        _firePoint = transform.Find("Rotator").Find("FirePoint").gameObject;
        if (_firePoint == null)
            _firePoint = gameObject;
    }

    public bool isTargetAttackable(GameObject target)
    {
        return SpaceCalTool.AreObjectsViewableAndWhithinRange(gameObject, target, _range);
    }

    public bool isTargetInRange(Vector3 target)
    {
        return SpaceCalTool.IsDistanceBetweenTwoPosesLessThan(transform.position, target, _range);
    }

    public void attack()
    {
        if (Time.time - _cooldownStartTime < _cooldown)
            return;

        _cooldownStartTime = Time.time;
        Quaternion rotation = _firePoint.transform.rotation;
        GameObject projGameObj = Instantiate(projectile, _firePoint.transform.position, rotation);
        initProj(projGameObj);
        OnAttack(gameObject, SpaceCalTool.GetVectorDirectionTowardTarget(transform.position, _firePoint.transform.position));
    }

    private void initProj(GameObject projGameObj)
    {
        Rigidbody2D rb = projGameObj.GetComponent<Rigidbody2D>();
        Projectile projCtrl = projGameObj.GetComponent<Projectile>();

        projGameObj.transform.RotateAround(transform.position, new Vector3(0, 0, 1), _projectileOffsetValue);

        rb.velocity = _firePoint.transform.up * _projSpeed;
        projCtrl.attacker = gameObject;
        projCtrl.shootDamege = _damage;

    }
}
