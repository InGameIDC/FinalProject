using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private float _id;
    private float _damageMultiplier;
    [SerializeField] private float _damage;
    [SerializeField] float _range;
    private float _attackSpeed;
    private float _cooldown;
    private bool _isOnCooldown;
    private bool _canAttackOnMovment;
    private bool _needToBeAimed; // if ture, the target has to rotate toward the target
    [SerializeField] GameObject projectile;
    private GameObject _firePoint; // The projectile spawn location

    private void Awake()
    {
        _cooldown = 2f;
        _damage = 1f;
        _range = 5;
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
    public bool IsOnCooldown() => _isOnCooldown;

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


    public void startCooldown()
    {
        if (_isOnCooldown)
            return;

        _isOnCooldown = true;
        StartCoroutine(cooldownTimeManage());
    }

    public IEnumerator cooldownTimeManage()
    {
        yield return new WaitForSeconds(_cooldown);

        _isOnCooldown = false;
    }

    public bool isTargetInRange(Vector3 target)
    {
        return SpaceCalTool.IsDistanceBetweenTwoPosesLessThan(transform.position, target, _range);
    }

    public void attack()
    {
        if (_isOnCooldown)
            return;

        startCooldown();
        Quaternion rotation = _firePoint.transform.rotation;
        GameObject projGameObj = Instantiate(projectile, _firePoint.transform.position, rotation);
        initProj(projGameObj);
    }

    private void initProj(GameObject projGameObj)
    {
        float projSpeed = 10f;
        Rigidbody2D rb = projGameObj.GetComponent<Rigidbody2D>();
        Projectile projCtrl = projGameObj.GetComponent<Projectile>();

        rb.velocity = _firePoint.transform.up * projSpeed;
        projCtrl.onHitMechs += hitTarget;
        projCtrl.attacker = gameObject;
    }

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
