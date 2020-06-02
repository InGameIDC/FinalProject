using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHit : MonoBehaviour
{
    private GameObject attacker;
    [SerializeField] public float AttackVelocity = 50f;
    
    [SerializeField] public bool isClockDirection = false;
    private float roationDirection = 1;
    //Start is called before the first frame update
    void Start()
    {
        if (isClockDirection)
            roationDirection *= -1f;

        attacker = GetComponent<Projectile>().attacker;
        Vector2 direction = SpaceCalTool.GetVectorDirectionTowardTarget(transform.position, attacker.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = attacker.transform.position;
        Vector3 axis = new Vector3(0, 0, roationDirection);
        transform.RotateAround(point, axis, Time.deltaTime * AttackVelocity);
    }
}
