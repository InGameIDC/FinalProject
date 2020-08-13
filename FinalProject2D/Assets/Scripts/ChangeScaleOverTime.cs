using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScaleOverTime : MonoBehaviour
{
    [SerializeField] float changeRate = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale + (changeRate * Vector3.one * Time.deltaTime);
    }
}
