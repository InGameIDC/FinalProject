using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectAtRandom : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] float cooldown = 1f;
    [SerializeField] Vector3 offset;
    private float _cooldownStartTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if not in cooldown spawn obj
        if(Time.time - _cooldownStartTime > cooldown)
        {
            _cooldownStartTime = Time.time;
            Vector3 Pos = getRandomPos();
            Instantiate(objToSpawn, Pos, transform.rotation);
        }
    }

    private Vector3 getRandomPos()
    {
        GameObject[] ObjectSpawnPoses = GameObject.FindGameObjectsWithTag("ObjectSpawnPos");

        return ObjectSpawnPoses[Random.Range(0, ObjectSpawnPoses.Length)].transform.position + offset;
    }

}
