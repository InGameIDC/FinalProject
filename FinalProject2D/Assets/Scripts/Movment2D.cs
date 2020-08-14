using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Movment2D : MonoBehaviour
{
	public Action OnFinishRotation = delegate { };
    public Action OnFinishMovment = delegate { };     // Functions that would be preform when the hero finish to move / rotate
    public Action OnStartMovment = delegate { };     // Functions that would be preform when the hero start to move / rotate
	public Action OnFinishMovmentAnimation = delegate { };     // Functions that would be preform when the hero finish to move / rotate
	public Action OnStartMovmentAnimation = delegate { };     // Functions that would be preform when the hero start to move / rotate
	[SerializeField]private float _moveSpeed;       // The obect movment speed
	private float _rotateSpeed;
//    private Vector3 _desiredPos;                 // The location desired to move to.
    private bool _isMovmentFinishTracking;
    private GameObject _targetLocationLock;      // The obj will track the location of this target 
	private Vector2 _desiredRotationDirection;  // The direction the hero desired to rotate toward.####@@@####
	private GameObject _rotator;			   // Show the direction that the object is facing

	private AIDestinationSetter _desSetter;
    IAstarAI _AI;
	Rigidbody2D _rb;
	AIPath2D _AIPath;
	bool _isMoving = false;
	bool _isRotationLock = false;
	bool _isRotating = false;

	private Path _path;
	private int _currentWaypoint = 0;
	private bool _reachedEndOfPath = false;

	private Seeker _seeker;

	private List<float> _currentSpeedsChangesPrecents = new List<float>(); // used to mange multiplie buffs/debuffs of speed
	private float minSpeed = Mathf.Infinity;
	// Start is called before the first frame update
	private void Awake()
    {
        _desSetter = GetComponent<AIDestinationSetter>();
        _AI = GetComponent<IAstarAI>();
		_rb = GetComponent<Rigidbody2D>();
		_AI.isStopped = true;
		_seeker = GetComponent<Seeker>();
		_AIPath = GetComponent<AIPath2D>();
		_rotateSpeed = 360f;
		_rotator = transform.Find("Rotator").gameObject;
	}

	private void Start()
	{
		HeroData data = GetComponentInParent<HeroDataManage>().GetData();
		_moveSpeed = data.getMovementSpeed();
		_AI.maxSpeed = _moveSpeed;
	}

	private void OnEnable()
	{
		if (_AI != null)
			_AI.maxSpeed = _moveSpeed;
	}

	public Dictionary<GameObject, Vector3> heroesDirections = new Dictionary<GameObject, Vector3>();
	public float GetSpeed() => _AI.maxSpeed;
	public Vector3 GetHeroRotation() => _rotator != null ? _rotator.transform.rotation.eulerAngles : transform.rotation.eulerAngles;
	public Vector3 GetHeroDirection() => _rotator != null ? _rotator.transform.forward : transform.forward;

	/// <summary>
	/// Change the movement speed by the given precent
	/// </summary>
	/// <param name="precentOfCurrentSpeed">The relative change in the speed</param>
	/// <param name="duration">At the end of this duration, the speed would set to be normal, or to the next change that current present</param>
	public void SetSpeedByPrecentForDuration(float precentOfCurrentSpeed, float duration) {
		_currentSpeedsChangesPrecents.Add(precentOfCurrentSpeed);
		_currentSpeedsChangesPrecents.Sort();
		StartCoroutine(setSpeedForDurationCur(precentOfCurrentSpeed, duration));
	}

	private IEnumerator setSpeedForDurationCur(float precentOfCurrentSpeed, float duration)
	{
		//Debug.Log("Got speed: " + _moveSpeed * ((_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] > _moveSpeed) ? (_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] + _currentSpeedsChangesPrecents[0]) / 2f : _currentSpeedsChangesPrecents[0]));
		_AI.maxSpeed = _moveSpeed * (_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] > _moveSpeed ? (_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] + _currentSpeedsChangesPrecents[0]) / 2f : _currentSpeedsChangesPrecents[0]);
		yield return new WaitForSeconds(duration);
		if (_currentSpeedsChangesPrecents.Exists(precentOfSpeed => precentOfSpeed == precentOfCurrentSpeed)) // if the speed is in the speeds list
		{
			_currentSpeedsChangesPrecents.Remove(precentOfCurrentSpeed);
		}

		if(_currentSpeedsChangesPrecents.Count > 0)
			// If there is also a buff, then the speed would set to be avg of the buff and the duebuff, othersize it would be set by the debuff
			_AI.maxSpeed = _moveSpeed * (_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] > _moveSpeed ? (_currentSpeedsChangesPrecents[_currentSpeedsChangesPrecents.Count - 1] + _currentSpeedsChangesPrecents[0]) / 2f : _currentSpeedsChangesPrecents[0]);
		else
			_AI.maxSpeed = _moveSpeed;

	}


	#region CtrlMovment
	/// <summary>
	/// Terminates all the movments funcss
	/// Author: Ilan
	/// </summary>
	public void StopMovment()
	{
		_AI.destination = transform.position;
		_desSetter.target = null;
		_AI.isStopped = true;
		_isMoving = false;
		_AIPath.isRotateOnly = false;
		_rb.velocity = Vector2.zero;
		_isRotationLock = false;
		_isRotating = false;
		_targetLocationLock = null;
		OnFinishMovment = delegate { };
		OnStartMovment = delegate { };
		OnFinishRotation = delegate { };
	}

	/// <summary>
	/// this method tell command the obj unit to go to the desired location.
	/// active the OnStartMovment delegation
	/// Author: Ilan
	/// </summary>
	public void GoTo(Vector2 pos)
	{
		OnStartMovment();
		OnStartMovmentAnimation();
		_AIPath.isRotateOnly = false;
		_AI.destination = pos;
		_isMoving = true;
		_AI.isStopped = false;
		startMovmentFinishTrack();
	}

	public void GoAfterTarget(GameObject target)
	{
		OnStartMovment();
		OnStartMovmentAnimation();
		_desSetter.target = target.transform;

		if (SpaceCalTool.IsObjectOnMovment(target)) // sets acceleraction
			_AIPath.maxAcceleration = GlobalCodeSettings.ACCELARATION_TOWARD_MOVING_ENEMY;
		else
			_AIPath.maxAcceleration = GlobalCodeSettings.ACCELARATION;

		_isMoving = true;
		_AI.isStopped = false;
		startMovmentFinishTrack(); // TO BE CHECKED
	}

    #region Rotate functions that use the AI funcs
    /*
	public void RotateTowards(Vector3 pos)
	{
		_AIPath.isRotateOnly = true;
		_AI.destination = pos;
		_AI.isStopped = false;
	}

	public void RotateLock(GameObject target)
	{
		_AIPath.isRotateOnly = true;
		_desSetter.target = target.transform;
		_AI.isStopped = false;
	}
	*/
    #endregion

    private void RotateTowards(Vector3 pos)
	{
		
	}

	/// <summary>
	/// Set the gameobject to rotate after the target, as long as its within the given range
	/// </summary>
	/// <param name="target">The target to be rotate toward</param>
	/// <param name="range">Range to keep</param>
	public void TargetLock(GameObject target, float range)
	{
		_AI.isStopped = true;
		this._targetLocationLock = target;
		if (!_isRotationLock)
		{
			_isRotationLock = true;
			StartCoroutine(keepRotateTowardTarget(range));
		}
	}
	#endregion

	#region Trackers 
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
		while (!_AI.reachedDestination && _isMovmentFinishTracking)
		{
			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
		}

		_isMovmentFinishTracking = false;
		_AI.isStopped = true;
		_isMoving = false;
		OnFinishMovment();
		OnFinishMovmentAnimation();

		OnFinishMovment = delegate { };
		OnStartMovment = delegate { };
	}
	#endregion

	#region Rotation

	/// <summary>
	/// Rotate the object toward the target, as long as its in the given range
	/// Author: Ilan
	/// </summary>
	/// <param name="rangeKeep">If the target is further than this value, the track rotation would stop</param>
	/// <returns></returns>
	private IEnumerator keepRotateTowardTarget(float rangeKeep)
	{
		Vector3 direction;
		GameObject objToRotate = _rotator;

		_desiredRotationDirection = _targetLocationLock.transform.position;
		//Debug.Log("Pre Rotation");
		while (_targetLocationLock != null && SpaceCalTool.IsDistanceBetweenTwoPosesLessThan(objToRotate.transform.position, _targetLocationLock.transform.position, rangeKeep) && _isRotationLock && _targetLocationLock.activeSelf) // && !IsLookingAtTheTarget(_desiredRotationDirection)) // Checks if the object finished to rotate target, and if it's on movment
		{
			_desiredRotationDirection = _targetLocationLock.transform.position;

			if (!SpaceCalTool.IsLookingTowardsTheTarget(objToRotate.transform.gameObject, _desiredRotationDirection))
			{
				//Debug.Log("Not Looking");
				_isRotating = true;
				//this._desiredRotationDirection = GetXZposRelativeVector(this._targetRotationLock.transform.position);

				direction = SpaceCalTool.GetVectorDirectionTowardTarget(objToRotate.transform.position, this._desiredRotationDirection); //.normalized; // calcs the normalized direction vector

				objToRotate.transform.rotation = SpaceCalTool.CalcRotationToAgivenDirection(objToRotate.transform.rotation, direction, _rotateSpeed * Time.deltaTime * Mathf.Max(0, (1f - 0.3f) / 0.7f));// The amount size is equal to speed times frame time.
			}
			else // if already looking at the target
			{
				_isRotating = false;
				if (OnFinishRotation != null)
					OnFinishRotation();
				yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE); // For Efficiency - wait twice aslong than usual before starting the routine agian
			}

			yield return new WaitForSeconds(GlobalCodeSettings.FRAME_RATE);
			//Debug.Log("After Rotation Tick");
		}
		//Debug.Log("End Rotation");

		_isRotating = false;
		_desiredRotationDirection = this.transform.position;

		if (OnFinishMovment != null)
			OnFinishMovment();

		// StopMovment(); // TO BE CHECKED
	}

	#endregion

	#region Movment Status Check
	/// <summary>
	/// Tells if the object is on movment or rotation
	/// Author: Ilan
	/// </summary>
	/// <returns>True if the object is on movment or rotation</returns>
	public bool IsObjOnMovment()
	{
		return IsObjMoving() || IsObjRotatingOnly();
	}

	/// <summary>
	/// Tells if the object is preforming a movment
	/// </summary>
	/// <returns>True if the object is moving</returns>
	public bool IsObjMoving()
	{
		return _isMoving;//!_AI.isStopped && Vector2.SqrMagnitude(_rb.velocity) > GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR;
	}

	/// <summary>
	/// Checks if the object is preforming a rotation only (not changing position)
	/// Author: Ilan
	/// </summary>
	/// <returns></returns>
	public bool IsObjRotatingOnly() // #TO BE Changed!
	{
		/*
		if (SpaceCalTool.IsDistanceBetweenTwoPosesLessThan(transform.position, _desiredRotationDirection, GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR))
			return false;
		*/
		//float angelDif = SpaceCalTool.CalcDiffAngle(gameObject, GetVectorDirectionTowardTarget());
		//return angelDif > GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR * 0.001f; // if the object is not looking at the desired rotation direction, it probably prefoms a rotation
		return _isRotating;//_AIPath.isRotateOnly;
	}
	#endregion

	#region Test
	/*
	public void Start()
	{
		//InvokeRepeating("testMov", 0, 0.1f);
	}
	*/
	void testMov()
	{
		Debug.Log("Is moving: " + IsObjMoving());
	}

	#endregion

}
