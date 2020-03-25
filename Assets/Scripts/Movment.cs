using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Movment : MonoBehaviour
{
	public  Action OnFinishMovment = delegate { };                   // Functions that would be preform when the hero finish to move / rotate
	public Action OnStartMovment = delegate { };                    // Functions that would be preform when the hero start to move / rotate
	private float _moveSpeed;                       // The obect movment speed
	private Vector3 _desiredPos;                   // The location desired to move to.
	private Vector3 _desiredRotationDirection;    // The direction the hero desired to rotate toward.
	private bool _isHeightCalculated;            // If toggle to true, the movments functions would respect the given poses heigh, otherwise, it would ignore the Y axis
	private bool _isRotationHeightCalculated;   // If toggle to true, the rotation functions would respect the given poses heigh, otherwise, it would ignore the Y axis
	private GameObject _targetRotationLock;    // The target that the obj would track (with rotation)
	private bool _isRotationLock;             // True if the object is lock on the target
	NavMeshAgent _navMeshAgent;              // Keep NavMeshAgent component
	private bool _isMovmentFinishTracking;
	private GameObject _targetLocationLock;    // The obj will track the location of this target
	private bool _isTargetLocationLock;




	[SerializeField]
	private Transform testDestination;  // Use for testing

	/// Says good morning to the script
	private void Awake()
	{
		_desiredRotationDirection = transform.position;
		_desiredPos = transform.position;
		_moveSpeed = 0.2f;
		_isRotationLock = false;

		bindNavMeshAgent();
		_navMeshAgent.isStopped = true;
	}

	/// <summary>
	/// Binds the NavMeshAgent componenet to the _navMeshAgent property
	/// If these is no NavMeshAgent component on the object,
	/// creates a one, and notify on the debugger
	/// Author: Ilan
	/// </summary>
	private void bindNavMeshAgent()
	{
		_navMeshAgent = this.GetComponent<NavMeshAgent>();
		if (_navMeshAgent == null)
		{
			Debug.Log("Could not find the navMeshAgen");
			_navMeshAgent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
		}
	}

	/// <summary>
	/// Terminates all the movments funcss
	/// Author: Ilan
	/// </summary>
	public void StopMovment()
	{
		OnFinishMovment = null;
		OnStartMovment = null;
		_isRotationLock = false;
		_targetRotationLock = null;
		_desiredPos = transform.position;
		_desiredRotationDirection = transform.position; // = transform.forward;
		stopMovmentFinishTrack();
		stopTargetLocationTrack();
		StopNav();
	}

	/// <summary>
	/// this method stops the navigation of the heroUnit.
	/// Author: Dor
	/// </summary>
	public void StopNav()
	{
		_navMeshAgent.isStopped = true;
		_navMeshAgent.velocity = Vector3.zero;
	}

	/// <summary>
	/// this method tell command the obj unit to go to the desired location.
	/// active the OnStartMovment delegation
	/// Author: Dor, Ilan
	/// </summary>
	public void GoTo(Vector3 pos)
	{
		//Debug.Log("SetObjDesirePos");
		if (OnStartMovment != null)
			OnStartMovment();
		setDesiredPos(pos);
		//StartCoroutine(moveObject());
		_navMeshAgent.isStopped = false;
		_navMeshAgent.SetDestination(pos);

		if (OnFinishMovment != null)
			startMovmentFinishTrack();
	}

	public void GoAfterTarget(GameObject target)
	{
		//Debug.Log("SetObjDesirePos");
		if (OnStartMovment != null)
			OnStartMovment();
		_desiredPos = target.transform.position;
		//StartCoroutine(moveObject());
		_navMeshAgent.isStopped = false;
		_navMeshAgent.SetDestination(target.transform.position);
		startTargetLocationTrack();

		if (OnFinishMovment != null)
			startMovmentFinishTrack();
	}

	private void startMovmentFinishTrack()
	{
		//Debug.Log("startMovFinishTrack");
		if (_isMovmentFinishTracking)
			return;

		_isMovmentFinishTracking = true;
		StartCoroutine(autoMovmentFinishTrack());
	}

	private void stopMovmentFinishTrack()
	{
		//Debug.Log("stopMovFinishTrack");
		_isMovmentFinishTracking = false;
		OnFinishMovment = delegate { };
	}

	private IEnumerator autoMovmentFinishTrack()
	{
		while (IsObjMoving() && _isMovmentFinishTracking)
		{
			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
		}

		_isMovmentFinishTracking = false;
		OnFinishMovment();
	}

	private void startTargetLocationTrack()
	{
		//Debug.Log("startMovFinishTrack");
		if (_isTargetLocationLock)
			return;

		_isTargetLocationLock = true;
		StartCoroutine(autoTargetLocationTrack());
	}

	private void stopTargetLocationTrack()
	{
		//Debug.Log("stopMovFinishTrack");
		_isTargetLocationLock = false;
		OnFinishMovment = delegate { };
	}

	private IEnumerator autoTargetLocationTrack()
	{
		while (_isTargetLocationLock && _targetLocationLock != null)
		{
			if (_desiredPos != _targetLocationLock.transform.position)
				GoTo(_targetLocationLock.transform.position);

			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
		}

		_isMovmentFinishTracking = false;
	}

	/// <summary>
	/// Makes the object to rotate toward a GameObject, as long as its within the given range
	/// Author: Ilan
	/// </summary>
	/// <param name="target">The target that the game object would rotate toward</param>
	/// <param name="range">The maximum distance of the target, that the object would keep the lock on the target</param>
	public void TargetLock(GameObject target, float range)
	{
		this._targetRotationLock = target;
		if (!_isRotationLock)
		{
			_isRotationLock = true;
			StartCoroutine(keepRotateTowardTarget(range));
		}
	}

	/// <summary>
	/// Calculate the closet position from current to the targetPos, that keep the mention distance from the target
	/// Author: Ilan
	/// </summary>
	/// <param name="reqDistance">The minimu distance required</param>
	/// <param name="current">The object itself Pos</param>
	/// <param name="targetPos">The tatget Pos</param>
	/// <returns></returns>
	public Vector3 CalcClosestPosWithThisDistance(float reqDistance, Vector3 current, Vector3 targetPos)
	{
		Debug.Log("calcClosestPosWithThisDistance");
		float distance = Vector3.Distance(current, targetPos) - reqDistance;
		if (distance <= 0)
			return current;

		return current + (targetPos - _desiredRotationDirection).normalized * distance;
	}

	/// <summary>
	/// Checks if the target is infront of the object, regarding the distanses
	/// if the height is not set to be calculate, would check only relative to the X and Z axis
	/// otherwise, would check the Y axis too.
	/// Author: Ilan
	/// </summary>
	/// <param name="targetPos">The target to check</param>
	/// <returns>true if the target infront of the object</returns>
	public bool IsLookingAtTheTarget(Vector3 targetPos)
	{
	 if (!_isRotationHeightCalculated)
			targetPos.y = transform.position.y;

		Vector3 direction = getVectorDirectionTowardTarget(targetPos); // Gets the direction vector
		float angelDif = diffAngle(direction); // Gets the angle diffrent of the object regarding the target pos
		return angelDif <= GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR * 0.001f; // return true if the diffrence is less than the globam margin of error
	}

	/*
	public bool IsLookingAtTheTargetXZ(Vector3 targetPos)
	{
		return IsLookingAtTheTarget(GetXZposRelativeVector(targetPos));
	}
	*/
	/// <summary>
	/// Calculate the diffrence vector between the object to the given target,
	/// if the obj position would change by this vector, 
	/// (add the result to the obj pos) the object would be locate at the target pos.
	/// Author: Ilan
	/// </summary>
	/// <param name="target">The target position vector</param>
	/// <returns>The diffrence vector (target - this.transform.position) </returns>
	private Vector3 getVectorDirectionTowardTarget(Vector3 target)
	{
		return target - this.transform.position;
	}

	/// <summary>
	/// Calculate the diffrence vector between the object to the given target,
	/// but ignores the y axis
	/// if the obj position would change by this vector, 
	/// (add the result to the obj pos) the object would be locate at the target pos.
	/// Author: Ilan
	/// </summary>
	/// <param name="target">The target position vector</param>
	/// <returns>The diffrence vector (X,Z), the Y remains to the object pos Y value </returns>
	public Vector3 GetXZposRelativeVector(Vector3 pos)
	{
		return new Vector3(pos.x, transform.position.y, pos.z);
	}

	/// <summary>
	/// Move the Gameobject to the desiredPos.
	/// Author: Ilan 
	/// In addtion, rotate the Object toward the pos.
	/// </summary>
	private IEnumerator moveObject() // to implement with navMesh
	{
		Vector3 desiredPos = this._desiredPos;
		Vector3 heighIgoneDesiredPos = GetXZposRelativeVector(desiredPos); // set the position to be at the same heigh as the obj

		this._desiredRotationDirection = this._desiredPos;
		StartCoroutine(rotateTowardDirection());

		Vector3 direction = getVectorDirectionTowardTarget(heighIgoneDesiredPos);
		direction =  direction.normalized;

		while (heighIgoneDesiredPos != transform.position && IsObjMoving()) // Checks if the object reached to the desired pos, and if it's on movment
		{
			if (desiredPos != this._desiredPos) // checks if the movment is still relvant
				yield break;

			this.transform.position += direction * _moveSpeed;

			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
		}

		if (OnFinishMovment != null && !IsObjOnMovment())
			OnFinishMovment();
	}

	/// <summary>
	/// Rotate the obj toward the desired rotation direction
	/// Author: Ilan
	/// </summary>
	private IEnumerator rotateTowardDirection()
	{
		Vector3 targetDirection = this._desiredRotationDirection; // Determine which direction to rotate towards

		Vector3 direction = getVectorDirectionTowardTarget(targetDirection).normalized;

		float angelDif = diffAngle(direction);
		while (angelDif > GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR * 0.001f) // Checks if the object finished to rotate target, and if it's on movment
		{
			if (targetDirection != this._desiredRotationDirection) // checks if the rotation is still relvant
				yield break;

			rotateToAgivenDirection(direction, _moveSpeed * 0.2f);// The amount size is equal to speed times frame time.

			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
		}
		rotateToAgivenDirection(direction, _moveSpeed * 0.2f);

		if (OnFinishMovment != null && !IsObjOnMovment())
			OnFinishMovment();

	}


	/// <summary>
	/// Rotate the object toward the target, as long as its in the given range
	/// Author: Ilan
	/// </summary>
	/// <param name="rangeKeep">If the target is further than this value, the track rotation would stop</param>
	/// <returns></returns>
	private IEnumerator keepRotateTowardTarget(float rangeKeep)
	{
		Vector3 direction;

		setDesiredRotationDirection(_targetRotationLock.transform.position);
		Debug.Log("Pre Rotation");
		while (_targetRotationLock != null && isTargetInRange(rangeKeep) && _isRotationLock) // && !IsLookingAtTheTarget(_desiredRotationDirection)) // Checks if the object finished to rotate target, and if it's on movment
		{
			setDesiredRotationDirection(_targetRotationLock.transform.position);

			if (!IsLookingAtTheTarget(_desiredRotationDirection))
			{
				//this._desiredRotationDirection = GetXZposRelativeVector(this._targetRotationLock.transform.position);

				direction = getVectorDirectionTowardTarget(this._desiredRotationDirection).normalized; // calcs the normalized direction vector

				rotateToAgivenDirection(direction, _moveSpeed * 0.2f);// The amount size is equal to speed times frame time.
			}
			else // if already looking at the target
			{
				if (OnFinishMovment != null)
					OnFinishMovment();
				yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE); // For Efficiency - wait twice aslong than usual before starting the routine agian
			}

			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
			Debug.Log("After Rotation Tick");
		}
		Debug.Log("End Rotation");

		_desiredRotationDirection = this.transform.position;

		if (OnFinishMovment != null )//&& !IsObjOnMovment())
			OnFinishMovment();

		StopMovment();
	}

	/// <summary>
	/// Rotate the object party toward the given direction,
	/// the amount of the rotation is determinate by the amount parameter
	/// Author: Ilan
	/// </summary>
	/// <param name="direction">The direction that the object would rotate toward</param>
	/// <param name="amount">The part of the rotation that the object would rotate</param>
	private void rotateToAgivenDirection(Vector3 direction, float amount)
	{
		Quaternion rotationAmount; // The rotation amount per each iteration
		// Rotate the forward vector towards the target direction by one step
		Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, direction, amount, 0.0f);

		// Draw a ray pointing at our target in
		Debug.DrawRay(transform.position, newDirection, Color.red);
		// Calculate a rotation a step closer to the target and applies rotation to this object
		rotationAmount = Quaternion.LookRotation(newDirection);

		this.transform.rotation = rotationAmount; // rotate the object
	}

	/// <summary>
	/// Checks if the target is closer than the given number
	/// Author: Ilan
	/// </summary>
	/// <param name="range">the maximum distance to check</param>
	/// <returns>True if the target distance is less than the range value</returns>
	private bool isTargetInRange(float range)
	{
		return isDistanceBetweenTwoPosesLessThan(transform.position, _targetRotationLock.transform.position, range);
	}

	/// <summary>
	/// Checks if the distance between two vectors is less than the given distance.
	/// Author: Ilan
	/// </summary>
	/// <param name="pos1">The first vector position</param>
	/// <param name="pos2">The second vector position</param>
	/// <param name="distance">The maximum distance</param>
	/// <returns>True if the distance between the positions is less than distance</returns>
	private bool isDistanceBetweenTwoPosesLessThan(Vector3 pos1, Vector3 pos2, float distance)
	{
		//float dis = DistancePow2(pos1, pos2);
		//Debug.Log("Distance: " + dis);
		return DistancePow2(pos1, pos2) <= (distance * distance);
	}

	/// <summary>
	/// Sets the desired rotation direction,
	/// with respect to the height calculation mod.
	/// Author: Ilan
	/// </summary>
	/// <param name="direction"></param>
	private void setDesiredRotationDirection(Vector3 direction)
	{
		_desiredRotationDirection.x = direction.x;
		_desiredRotationDirection.z = direction.z;

		if (_isRotationHeightCalculated)
			_desiredRotationDirection.y = direction.y;
	}

	/// <summary>
	/// Sets the desired location that the object would move to,
	/// with respect to the height calculation mod.
	/// Author: Ilan
	/// </summary>
	/// <param name="direction"></param>
	private void setDesiredPos(Vector3 direction)
	{
		_desiredPos.x = direction.x;
		_desiredPos.z = direction.z;

		if (_isHeightCalculated)
			_desiredPos.y = direction.y;
	}

	/// <summary>
	/// Calculate the diffrance between the current angel, to the require 
	/// Author: Ilan 
	/// </summary>
	/// <param name="target">The target direction vector (desiredPos - transform.position) </param>
	/// <returns>Returns the diffrances angle</returns>
	private float diffAngle(Vector3 targetDirection)
	{
		Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, 360f, 0.0f); // cacls the vector rotation diffrence
		Quaternion rotationLeft = Quaternion.LookRotation(newDirection);
		return Quaternion.Angle(transform.rotation, rotationLeft); // calcs the angle diffrence between the current rotation the rotationLeft vector
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

	public bool IsNavigating2()
	{
		return _navMeshAgent.pathPending || !_navMeshAgent.isStopped;
	}

	/// <summary>
	/// Tells if the object is on movment or rotation
	/// Author: Ilan
	/// </summary>
	/// <returns>True if the object is on movment or rotation</returns>
	public bool IsObjOnMovment()
	{
		return IsObjMoving() || IsObjRotating();
	}

	/// <summary>
	/// Tells if the object is preforming a movment
	/// </summary>
	/// <returns>True if the object is moving</returns>
	public bool IsObjMoving()
	{
		return /*!isDistanceBetweenTwoPosesLessThan(transform.position, _desiredPos, GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR); //|| */IsNavigating2();
	}

	/// <summary>
	/// Checks if the object is preforming a rotation
	/// Author: Ilan
	/// </summary>
	/// <returns></returns>
	public bool IsObjRotating()
	{
		if (isDistanceBetweenTwoPosesLessThan(transform.position, _desiredRotationDirection, GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR))
			return false;

		float angelDif = diffAngle(getVectorDirectionTowardTarget(_desiredRotationDirection));
		return angelDif > GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR * 0.001f; // if the object is not looking at the desired rotation direction, it probably prefoms a rotation
	}

	/// <summary>
	/// Calculate the distance between the two vector, powed by 2
	/// Author: Ilan
	/// </summary>
	/// <param name="v1">The 1th vector</param>
	/// <param name="v2">The 2th vector</param>
	/// <returns>(distance)^2</returns>
	public float DistancePow2(Vector3 v1, Vector3 v2)
	{
		return (v1 - v2).sqrMagnitude;
	}

	private void testMovement()
	{
		//SetHeroDesirePos(Vector3.zero);
		//StartCoroutine(testMovmentFuncChangePosWhileMov(new Vector3(1f, 1f, 1f), 1f));
		//StartCoroutine(testRotateFuncChangePosWhileRotate(new Vector3(-10f, -10f, -10f), 0.4f));

		List<Vector3> testPoses = new List<Vector3>();
		testPoses.Add(new Vector3(5f, 0.125f, 5f));
		testPoses.Add(new Vector3(-5f, 0.125f, 5f));
		testPoses.Add(new Vector3(-10f, 0.125f, -10f));
		testPoses.Add(new Vector3(2f, 0.125f, -10f));
		//StartCoroutine(testMovmentFuncListOfPosOrders(testPoses, GlobalCodeSettings.CaclTimeRelativeToFramRate(3f), false));

	}

	/// <summary>
	/// Uses to test the object movment
	/// Author: Ilan
	/// </summary>
	/// <param name="Poses"></param>
	/// <returns></returns>
	/*
	private IEnumerator testMovmentFuncListOfPosOrders(List<Vector3> poses, float delay, bool patrolLoop)
	{
		do
		{
			foreach (Vector3 pos in poses)
			{
				GoTo(ref pos);
				yield return new WaitForSeconds(delay);
			}
		} while (patrolLoop);
	}
	*/
	/// <summary>
	/// Uses to test the object reaction to pos change while moving
	/// Author: Ilan
	/// </summary>
	/// <param name="Poses"></param>
	/// <returns></returns>
	private IEnumerator testMovmentFuncChangePosWhileMov(Vector3 pos, float delay)
	{
		yield return new WaitForSeconds(delay);
		setDesiredPos(pos);
	}

	/// <summary>
	/// Uses to test the object desireRotateDirection change while rotating
	/// Author: Ilan
	/// </summary>
	/// <param name="Poses"></param>
	/// <returns></returns>
	private IEnumerator testRotateFuncChangePosWhileRotate(Vector3 pos, float delay)
	{
		yield return new WaitForSeconds(delay);
		this._desiredRotationDirection = pos;
	}

	private IEnumerator testPrintRotationEveryInterval(float delay)
	{
		while (true)
		{
			Debug.Log(this.transform.rotation);
			yield return new WaitForSeconds(delay);
		}
	}

}
