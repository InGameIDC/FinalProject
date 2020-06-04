﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsAddedText : MonoBehaviour
{
    [SerializeField] public float Duration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
}
