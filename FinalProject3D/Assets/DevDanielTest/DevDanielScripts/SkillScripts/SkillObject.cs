using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType {
    Damage,
    Heal,
    Explosion
    }

public abstract class SkillObject : ScriptableObject
{

    //Audio fields:
    public AudioClip skillCastSound;
    public AudioClip skillHitSound;
        
    public SkillType skillType;
    public Sprite skillIcon;
    public float skillCooldown;
    public float skillRadius;       //used for enemy/player detection radius from unit.

    //Fields to be derived from Player\Unit class or from Scene:
    public AudioSource skillSoundSource;
    public GameObject skillProjectile;
    public int skillId;

    [TextArea(15, 20)] public string description;

}
