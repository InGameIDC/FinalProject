using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("mask:" + mask.value);
        //Debug.Log("Temp: " +  SpaceCalTool.AreObjectsViewableAndWhithinRange(gameObject, target, 3.6f));

        Team a = Team.HeroUnit;
        Debug.Log(TeamTool.getEnemyLayer(a));
    }

}
