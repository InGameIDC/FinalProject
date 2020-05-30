using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disapear : MonoBehaviour
{
    public void selfDestroy()
    {
        Destroy(gameObject);
    }
}
