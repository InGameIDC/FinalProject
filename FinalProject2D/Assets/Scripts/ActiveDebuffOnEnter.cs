using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDebuffOnEnter : MonoBehaviour, debuffActivator
{
    [SerializeField] List<DeBuff> debuffsToActive;

    private void Awake()
    {
        if(debuffsToActive == null)
            debuffsToActive = new List<DeBuff>();
    }

    private void OnTriggerEnter2D(Collider2D unit)
    {
        foreach (DeBuff debuff in debuffsToActive)
        {
            debuff.activeDebuff(unit.transform.parent.gameObject);
        }
    }
    public void addDebuff(DeBuff debuff)
    {
        debuffsToActive.Add(debuff);
    }

}
