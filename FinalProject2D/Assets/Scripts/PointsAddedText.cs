using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsAddedText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
