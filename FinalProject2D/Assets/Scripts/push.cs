using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 direction;
    [SerializeField] float speed = 1f;
    List<GameObject> targetOnBoard = new List<GameObject>();


    private void OnTriggerStay2D(Collider2D other)
    {
        PushTarget(other.gameObject);
    }

    private void PushTarget(GameObject target)
    {
        Transform rb = target.GetComponent<Transform>();
        Vector3 currentPos = rb.position;
        Vector3 newPos = currentPos + direction * Time.deltaTime;
        rb.position = newPos;
    }
}