using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Movment2DAI : MonoBehaviour
{
    public Action OnFinishMovment = delegate { };      // Functions that would be preform when the hero finish to move / rotate
    public Action OnStartMovment = delegate { };      // Functions that would be preform when the hero start to move / rotate
    private float _moveSpeed;                        // The obect movment speed
    private GameObject _targetLocationLock;         // The obj will track the location of this target
    private bool _isTargetLocationLock;
    private Vector3 _desiredPos;                  // The location desired to move to.
    private Vector3 _desiredRotationDirection;   // The direction the hero desired to rotate toward.
    private float nextWaypointDistance = 0f;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;

    private Seeker _seeker;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoTo(Vector2 desiredPos)
    {
        _seeker.StartPath(_rb.position, _targetLocationLock.transform.position, OnPathCaclComplete);
    }

    private void OnPathCaclComplete(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }
}
