using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] public float Intervals = 0.2f;

    private void OnEnable()
    {
        StartCoroutine(BlinkSprite());
    }

    private IEnumerator BlinkSprite()
    {
        SpriteRenderer spriteRend = GetComponent<SpriteRenderer>();
        while (true)
        {
            yield return new WaitForSeconds(Intervals);
            spriteRend.enabled = !spriteRend.enabled;
        }
    }
}
