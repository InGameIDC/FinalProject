using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOr : MonoBehaviour
{

    public Action<Skill> OnHit = delegate { };
    public Action<Skill> OnHitSound = delegate { };
    public Action<Skill> OnHitEffect = delegate { };

    private Skill _projectileSkill;
    private float _projectileSpeed;
    private float _projectileDamage;

    private void Start()
    {
      //  _projectileSkill = FindObjectOfType
    }

    private void Update()
    {
        
    }
}
