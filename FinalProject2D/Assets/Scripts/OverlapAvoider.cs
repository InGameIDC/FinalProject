using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapAvoider : MonoBehaviour
{
    private CircleCollider2D overlapAvoiderCollider;
    private Movment2D parentMovementComp;
    [SerializeField] private float movementColiderSize = 0.05f;
    [SerializeField] private float IdleColiderSize = 0.5f;
    private float timeSincelastOverlapOnMovmentAvoiderFunc = 0f;
    private float overlapOnMovmentAvioderFuncCallRate = 2f;
    // Start is called before the first frame update
    void Start()
    {
        parentMovementComp = GetComponentInParent<Movment2D>();
        overlapAvoiderCollider = GetComponent<CircleCollider2D>();
        overlapAvoiderCollider.radius = IdleColiderSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (overlapAvoiderCollider.enabled && parentMovementComp.IsObjMoving())
        {
            overlapAvoiderCollider.radius = movementColiderSize;
            overlapAvoiderCollider.enabled = false;
        }
        else if(!overlapAvoiderCollider.enabled && !parentMovementComp.IsObjMoving())
        {
            overlapAvoiderCollider.enabled = true;
            overlapAvoiderCollider.radius = IdleColiderSize;
        }
        overlapOnMovementAvoider();
    }

    /// <summary>
    /// This func active create a small colder during heroes movment,
    /// for a shot duration only to avoid overlap during movment.
    /// </summary>
    private void overlapOnMovementAvoider()
    {
        // if not moving, the function is not relevant
        if (!parentMovementComp.IsObjMoving())
            return;

        if (!overlapAvoiderCollider.enabled && Time.time - timeSincelastOverlapOnMovmentAvoiderFunc > overlapOnMovmentAvioderFuncCallRate)
        {
            timeSincelastOverlapOnMovmentAvoiderFunc = Time.time;
            overlapAvoiderCollider.enabled = true;
        }
        else if(overlapAvoiderCollider.enabled && Time.time - timeSincelastOverlapOnMovmentAvoiderFunc > 0.1f)
        {
            overlapAvoiderCollider.enabled = false;
        }
    }
}
