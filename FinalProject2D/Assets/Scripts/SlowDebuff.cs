using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : MonoBehaviour , DeBuff
{
    [SerializeField] float slowPrecent = 1;
    [SerializeField] float duration = 1;
    [SerializeField] Color color = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        debuffActivator proj = GetComponent<Projectile>();
        
        if(proj == null)
            proj = GetComponent<ActiveDebuffOnEnter>();

        proj.addDebuff(this);
    }

    public void activeDebuff(GameObject target)
    {
        target.GetComponent<Movment2D>().SetSpeedByPrecentForDuration(slowPrecent, duration);
        target.GetComponent<HeroUnit>().setDebuffColor(color, duration);
    }
}
