using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Skill", menuName = "Skill System/Skills/Heal")]
public class HealSkill : SkillObject
{
    public float healValue;
    private void Awake()
    {
        skillType = SkillType.Heal;
    }
}
