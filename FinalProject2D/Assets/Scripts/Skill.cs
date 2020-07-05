﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private float _id;
    private float _damageMultiplier;
    [SerializeField]  private float _damage = 1f;
    [SerializeField] float _range = 5f;
    [SerializeField] private float _projSpeed = 5f;
    [SerializeField] private float _cooldown = 2f;
    private float _cooldownStartTime = 0f;
    [SerializeField] private bool _canAttackOnMovment = false; // for now, only for heroes that thier attack does not required to be aimed
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
        //attack();
    }

    public bool CanAttackOnMovment() => _canAttackOnMovment;
    public bool IsNeedToBeAimed() => _needToBeAimed;
    public float GetRange() => _range;
    public bool IsOnCooldown() => Time.time - _cooldownStartTime < _cooldown;

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
