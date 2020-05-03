﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float speed;

    public Transform otherCherry;

    public float secondsToMaxDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Lerp(minSpeed, maxSpeed, GetDifficutlyPercent());
        transform.position = Vector2.MoveTowards(transform.position, otherCherry.position, speed * Time.deltaTime);
    }

    float GetDifficutlyPercent()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}
