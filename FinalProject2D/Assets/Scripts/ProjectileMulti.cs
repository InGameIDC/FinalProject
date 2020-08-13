using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMulti : Projectile
{
    [SerializeField] GameObject projToMult;
    [SerializeField] float amount;
    [SerializeField] List<float> posOffsets; // init position relative to the unit current pos
    [SerializeField] List<Vector3> relativeDirections; // new direction changes relative to the forward direction
    [SerializeField] float delayBetweenProjSpawn = 0f;
    [SerializeField] float _projSpeed;
    [SerializeField] bool lockRotation = true;
    private void Awake()
    {
        if (posOffsets == null)
            posOffsets = new List<float>();
        if (posOffsets.Count == 0)
            posOffsets.Add(0);

        if (relativeDirections == null)
            relativeDirections = new List<Vector3>();
        if (relativeDirections.Count == 0)
            relativeDirections.Add(Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(createProjectiles());
    }

    private IEnumerator createProjectiles()
    {
        for (int i = 0; i < amount; i++)
        {
            Quaternion rotation = transform.rotation;
            GameObject projGameObj = Instantiate(projToMult, transform.position, rotation);
            Debug.Log("AA: " + posOffsets[i % posOffsets.Count]);
            Debug.Log("BB: " + relativeDirections[i % relativeDirections.Count]);
            initProj(projGameObj, posOffsets[i % posOffsets.Count], relativeDirections[i % relativeDirections.Count]);
            yield return new WaitForSeconds(delayBetweenProjSpawn);
        }
        Destroy(gameObject);
    }

    private void initProj(GameObject projGameObj, float _projectileOffsetValue, Vector3 relativeDirection)
    {
        Rigidbody2D rb = projGameObj.GetComponent<Rigidbody2D>();
        Projectile projCtrl = projGameObj.GetComponent<Projectile>();

        projGameObj.transform.RotateAround(attacker.transform.position, new Vector3(0, 0, 1), _projectileOffsetValue);
        if(lockRotation)
            projGameObj.transform.rotation = transform.rotation; // set the rotation same as the attack

        rb.velocity = (transform.up + relativeDirection).normalized * _projSpeed;
        projCtrl.attacker = attacker;
        projCtrl.shootDamege = shootDamege;

    }
}
