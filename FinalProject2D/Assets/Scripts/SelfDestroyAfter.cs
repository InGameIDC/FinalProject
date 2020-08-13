using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAfter : MonoBehaviour
{
    [SerializeField] float destroyAfterThisDuration = 1f;

    private void Start()
    {
        StartCoroutine(SelfDestroyAfterDuration(destroyAfterThisDuration));
    }

    private IEnumerator SelfDestroyAfterDuration(float duration)
    {
        yield return new WaitForSeconds(destroyAfterThisDuration);
        Destroy(gameObject);
    }
}
