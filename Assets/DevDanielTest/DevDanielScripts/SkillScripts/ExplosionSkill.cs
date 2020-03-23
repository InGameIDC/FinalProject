using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explosion Skill", menuName = "Skill System/Skills/Explosion")]
public class ExplosionSkill : SkillObject
{
    
    public float damageValue;
    public float explosionDamageValue;
    public float explosionRadius;
    private void Awake()
    {
        skillType = SkillType.Damage;
    }
}
