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
    private float timeReaminToBeReady;
    bool canAttackOnMovment;
    bool needToBeAimed; // if ture, the target has to rotate toward the target
    [SerializeField] GameObject projectile;

    private void Awake()
    {
        range = 5f;
        cooldown = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //attack();
    }

    public bool CanAttackOnMovment() => canAttackOnMovment;
    public bool IsNeedToBeAimed() => needToBeAimed;
    public float GetRange() => range;
    public bool IsOnCooldown() => timeReaminToBeReady > 0;

    public bool isTargetAttackable(GameObject target)
    {
        return SpaceCalTool.AreObjectsViewableAndWhithinRange(gameObject, target, range);
    }


    public void startCooldown()
    {
        if (IsOnCooldown())
            return;

        timeReaminToBeReady = cooldown; // sets the cooldown
        StartCoroutine(cooldownTimeManage());
    }

    public IEnumerator cooldownTimeManage()
    {
        while(timeReaminToBeReady > 0)
        {
            timeReaminToBeReady -= GlobalCodeSettings.FRAME_RATE;
            yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
        }
        timeReaminToBeReady = 0;
    }

    public bool isTargetInRange(Vector3 target)
    {
        return Vector3.SqrMagnitude(target -transform.position) <= (range * range);
    }
    public void attack()
    {
        if (IsOnCooldown())
            return;

        startCooldown();
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

        rb.velocity = transform.forward * projSpeed;
        projCtrl.onHitMechs += hitTarget;

    }

    private void hitTarget(Projectile proj, Collider target)
    {
        Debug.Log("hitted");
        Health targetHealth = target.gameObject.GetComponent<Health>();
        if(targetHealth != null)
        {
            // Dance tango
        }

        Destroy(proj.transform.gameObject);
        //Destroy(target.transform.gameObject); //OrS: No need, killed in the health script
    }
}
