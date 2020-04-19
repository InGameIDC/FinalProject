using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Move the Gameobject to the desiredPos.
/// Author: Dor 
/// In addtion, return when the player changes course
/// </summary>
public class NavMesh : MonoBehaviour
{
    //Action onFinishMovment;
    //Action onStartMovment;
    private Vector3 desiredPos;
    [SerializeField]
    private Transform _destination;
    NavMeshAgent _navMeshAgent;

    
    void Start()
    {
        bindNavMeshAgent();
        //setDestination(Vector3.zero);    // dor testing
    }

    void Update()
    {
        if (!IsNavigating()) Destroy(this.gameObject);
    }
    
    private void bindNavMeshAgent()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            Debug.Log("Could not find the navMeshAgen");
            _navMeshAgent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }
    }

    /// <summary>
    /// this method checks if  the heroUnit is still navigating.
    /// Author: Dor
    /// </summary>
    public bool IsNavigating()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            return false;
        }
        return true;
    }
       
    /// <summary>
    /// start navigtation to the target vector.
    /// Author: Dor
    /// </summary>
    private void setDestination(Vector3 targetVector)
    {
        _navMeshAgent.SetDestination(targetVector);
    }

    /// <summary>
    /// this method stops the navigation of the heroUnit.
    /// Author: Dor
    /// </summary>
    public void StopNav()
    {
        this.GetComponent<NavMeshAgent>().isStopped = true;
    }

    /// <summary>
    /// this method tell command the hero unit to go to the desired location.
    /// Author: Dor
    /// </summary>
    public void GoTo(Vector3 desiredPos)
    {
        setDestination(desiredPos);
    }

}
