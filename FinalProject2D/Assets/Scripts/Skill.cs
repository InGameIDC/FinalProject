using System.Collections;
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
    private bool _canAttackOnMovment = false;
    private bool _needToBeAimed = true; // if ture, the target has to rotate toward the target
    [SerializeField] GameObject projectile;
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

        rb.velocity = _firePoint.transform.up * _projSpeed;
        projCtrl.onHitMechs += hitTarget;
        projCtrl.attacker = gameObject;

    }

    // TO BE FIXED SHOOT AND HERO DIE
    private void hitTarget(Projectile proj, Collider2D target)
    {
        //Debug.Log("hitted");
        Health targetHealth = target.gameObject.GetComponent<Health>();
        if(targetHealth != null)
        {
            target.GetComponent<Health>().TakeDamage(_damage);
        }
        
        Destroy(proj.transform.gameObject);
        //Destroy(target.transform.gameObject); //OrS: No need, killed in the health script
    }
}
