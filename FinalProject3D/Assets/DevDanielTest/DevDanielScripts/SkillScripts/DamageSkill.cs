using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Damage Skill", menuName ="Skill System/Skills/Damage")]
public class DamageSkill : SkillObject
{
    public float damageValue;
    private void Awake()
    {
        skillType = SkillType.Damage;
    }
}
