using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitOverTime : MonoBehaviour
{
    [SerializeField] float cooldown = 5f;
    [SerializeField] GameObject minions;
    //[SerializeField] GameObject target;
    private float lastSplitTime;

    // Start is called before the first frame update
    void Start()
    {
        lastSplitTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSplitTime > cooldown)
        {
            lastSplitTime = Time.time;

            GameObject[] respawns = GameObject.FindGameObjectsWithTag("RespawnEnemies");
            //Vector3 pos = Vector3.MoveTowards(transform.position, Vector3.zero, 1.5f);
            Vector3 pos = respawns[Random.Range(0, respawns.Length)].transform.position;

            GameObject minion = Instantiate(minions, pos, transform.rotation);
            GameObject[] targets = GameObject.FindGameObjectsWithTag("HeroUnit");
            foreach (GameObject target in targets)
            {
                minion.GetComponent<AI1>().addTraget(target);
            }
            //Debug.Log("Target: " + target);
        }

        
    }
}
