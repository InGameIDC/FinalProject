using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] public float RotateVelocity = 200f;
    [SerializeField] public Vector3 RotateDirection = new Vector3(0, 1, 0);
    // Update is called once per frame
    void Update()
    {
        Vector3 point = transform.position;
        Vector3 axis = RotateDirection;
        transform.RotateAround(point, axis, Time.deltaTime * RotateVelocity);
    }
}
