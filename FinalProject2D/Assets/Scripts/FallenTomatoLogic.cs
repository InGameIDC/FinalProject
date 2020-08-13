using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTomatoLogic : MonoBehaviour
{
    [SerializeField] GameObject objectThatTouchWouldDestory;
    [SerializeField] float distanceToDestory;
    [SerializeField] GameObject explotionEffect;
    [SerializeField] GameObject objectToSpawn;

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - objectThatTouchWouldDestory.transform.position).sqrMagnitude < distanceToDestory)
        {
            ProjectileExplosion proj = GetComponent<ProjectileExplosion>();
            Collider2D[] damageHitAreasInSphere;
            damageHitAreasInSphere = Physics2D.OverlapCircleAll(transform.position, proj.getRadius(), LayerMask.GetMask("DamageHitArea"));
            if (damageHitAreasInSphere.Length > 0)
                proj.ManualActiveExplotion(damageHitAreasInSphere[0]);

            Instantiate(explotionEffect, transform.position /*+ new Vector3(0, distanceToDestory * 2, 0)*/, transform.rotation);

            Instantiate(objectToSpawn, objectThatTouchWouldDestory.transform.position, transform.rotation);

            Destroy(objectThatTouchWouldDestory);
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}
