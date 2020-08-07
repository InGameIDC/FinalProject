using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisableAfterDelay : MonoBehaviour
{
    [SerializeField] float disableAfter = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > disableAfter)
            gameObject.SetActive(false);
    }
}
