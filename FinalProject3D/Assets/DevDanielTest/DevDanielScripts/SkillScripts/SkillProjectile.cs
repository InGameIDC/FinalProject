using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillProjectile : MonoBehaviour
{
    public Action<SkillProjectile, Collider> onHitMechs; // for data updates
    public Action<SkillProjectile> onHitDisplayers; // for feedbacks: visual and audio displays.

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag != "Enemy")
            return;

        if (onHitMechs != null)
            onHitMechs(this, target);
    }
}
