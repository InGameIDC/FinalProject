using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnObjetTouch : MonoBehaviour
{
    [SerializeField] GameObject objectThatTouchWouldDestory;
    [SerializeField] float distanceToDestory;

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - objectThatTouchWouldDestory.transform.position).sqrMagnitude < distanceToDestory)
        {

            Destroy(objectThatTouchWouldDestory);
            Destroy(gameObject);
        }
    }
}
