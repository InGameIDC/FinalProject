using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempActive : MonoBehaviour
{
    [SerializeField] public float Duration = 1f;
    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(Duration);
        gameObject.SetActive(false);
    }
}
