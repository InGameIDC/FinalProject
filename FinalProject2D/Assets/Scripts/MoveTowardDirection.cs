using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardDirection : MonoBehaviour
{
    [SerializeField] Vector3 direction = Vector3.up;
    [SerializeField] float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos += direction * speed *Time.deltaTime;
        transform.position = newPos;
    }
    
}
