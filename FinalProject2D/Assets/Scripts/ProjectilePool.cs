using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : Projectile
{
    [SerializeField] GameObject[] _projPool;
    [SerializeField] int[] _priorityListOfProjectileType;
    [SerializeField] float[] _speeds;

    private void Awake()
    {
        if (_speeds == null)
            _speeds = new float[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        int selectedProjIndex = getProiIndexByPriority(); // getting the proj index by priority
        GameObject proj = _projPool[selectedProjIndex]; // getting proj by priority
        Quaternion rotation = transform.rotation;
        GameObject projGameObj = Instantiate(proj, transform.position, rotation);
        initProj(projGameObj, _speeds[selectedProjIndex % _speeds.Length]);
        Destroy(gameObject);
    }

    private void initProj(GameObject projGameObj, float _projSpeed)
    {
        Rigidbody2D rb = projGameObj.GetComponent<Rigidbody2D>();
        Projectile projCtrl = projGameObj.GetComponent<Projectile>();

        projGameObj.transform.rotation = transform.rotation; // set the rotation same as the attack

        rb.velocity = (transform.up).normalized * _projSpeed;
        projCtrl.attacker = attacker;
        projGameObj.transform.position = projGameObj.transform.position + new Vector3(0, 0, -2);
    }

    private int getProiIndexByPriority()
    {
        List<int> projs = new List<int>();
        for (int i = 0; i < _projPool.Length; ++i)
        {
            if (i < _priorityListOfProjectileType.Length)
                for (int j = 0; j < _priorityListOfProjectileType[i]; ++j)
                {
                    projs.Add(i);
                }
            else
                projs.Add(i);
        }

        int rnd = Random.Range(0, projs.Count);

        return projs[rnd];
    }


}
