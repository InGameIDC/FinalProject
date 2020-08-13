using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;

    private void OnDestroy()
    {
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
