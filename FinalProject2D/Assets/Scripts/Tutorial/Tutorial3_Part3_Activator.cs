using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3_Part3_Activator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetComponentInParent<Tutorial3>().loadPart3();
    }
}
