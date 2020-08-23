using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InervalMove : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] float intervalSize = 2f;
    [SerializeField] float speed = 1f;

    private int directionSign;
    private float deltaTime;
    void Awake()
    {
        directionSign = 1;
        deltaTime = intervalSize/2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += directionSign * speed * direction * Time.deltaTime;
        deltaTime += Time.deltaTime;
        if(deltaTime >= intervalSize)
        {
            directionSign *= -1;
            deltaTime = 0;
        }
    }
}
