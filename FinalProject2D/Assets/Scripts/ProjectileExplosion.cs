﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileExplosion : Projectile
{
    [SerializeField] private float _hitRadius = 2f;

    protected override void OnTriggerEnter2D(Collider2D target)
    {
        if (!TeamTool.isEnemy(attacker, target.gameObject))
            return;

        Collider2D[] enemiesInSphere;
        
        enemiesInSphere = Physics2D.OverlapCircleAll(transform.position, _hitRadius, 1 << TeamTool.getEnemyLayer(attacker.tag));
        foreach (Collider2D enemy in enemiesInSphere)
        {
            //Damage the enemy

            //on hitting - the health is lowered
            GetComponent<CircleCollider2D>().isTrigger =
                false; // turn off the trigger (can't use the same bullet twice)

            if (onHitMechs != null)
                onHitMechs(this, enemy.gameObject);

        }
        Destroy(this);
    }
}

