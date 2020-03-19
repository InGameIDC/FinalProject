using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    float id;
    float damageMultiplier;
    float damage;
    float range;
    float attackSpeed;
    float cooldown;
    bool canAttackOnMovment;
    bool needToBeAimed; // if ture, the target has to rotate toward the target
    [SerializeField] GameObject projectile;


    // Start is called before the first frame update
    void Start()
    {
        //attack();
    }

    public bool CanAttackOnMovment() => canAttackOnMovment;
    public bool IsNeedToBeAimed() => needToBeAimed;

    public bool isTargetAttackAble(Vector3 current, Vector3 target)
    {
        return Vector3.Distance(current, target) <= range;
    }
    public void attack()
    {
        Vector3 pos = this.transform.position;
        Quaternion rotation = this.transform.rotation;
        GameObject projGameObj = Instantiate(projectile, pos, rotation);
        initProj(projGameObj);
    }

    private void initProj(GameObject projGameObj)
    {
        float projSpeed = 10f;
        Rigidbody rb = projGameObj.GetComponent<Rigidbody>();
        Projectile projCtrl = projGameObj.GetComponent<Projectile>();

        rb.velocity = Vector3.forward * projSpeed;
        projCtrl.onHitMechs += hitTarget;
    }

    private void hitTarget(Projectile proj, Collider target)
    {
        Debug.Log("hitted");
        Destroy(proj.transform.gameObject);
        Destroy(target.transform.gameObject);
    }
}
