using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Rotator : MonoBehaviour
{
    private GameObject _parent;
	private Rigidbody2D _parentRB;
    private IAstarAI _AI;
	private AIPath _AIPath;
    private Vector3 _nextPosition;
    private Quaternion _nextRotation;
	private float lastDeltaTime;
	[UnityEngine.Serialization.FormerlySerializedAs("turningSpeed")]
	public float rotationSpeed = 360;

	private void Awake()
    {
        _parent = transform.parent.gameObject;
        _AI = _parent.GetComponent<IAstarAI>();
		_parentRB = _parent.GetComponent<Rigidbody2D>();
		_AIPath = _parent.GetComponent<AIPath>();

	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _AI.MovementUpdate(Time.deltaTime, out _nextPosition, out _nextRotation);
		lastDeltaTime = Time.deltaTime;
		transform.rotation = CalculateNextRotation();

	}

	/// <summary>
	/// TOOK THIS FUNC FROM AIbash.cs of Pathfinding
	/// </summary>
	/// <returns></returns>
	protected Quaternion CalculateNextRotation()
	{
		Quaternion nextRotation;
		float slowdown = _AI.remainingDistance < _AIPath.slowdownDistance ? Mathf.Sqrt(_AIPath.remainingDistance / _AIPath.slowdownDistance) : 1;
		if (lastDeltaTime > 0.00001f)
		{
			Vector2 desiredRotationDirection;
			desiredRotationDirection = _AIPath.velocity;

			// Rotate towards the direction we are moving in.
			// Don't rotate when we are very close to the target.
			var currentRotationSpeed = rotationSpeed * Mathf.Max(0, (slowdown - 0.3f) / 0.7f);
			nextRotation = SimulateRotationTowards(desiredRotationDirection, currentRotationSpeed * lastDeltaTime);
		}
		else
		{
			// TODO: simulatedRotation
			nextRotation = transform.rotation; // TOBE CHECKED
		}

		return nextRotation;
	}

	/// <summary>
	/// TOOK THIS FUNC FROM AIbash.cs of Pathfinding
	/// Simulates rotating the agent towards the specified direction and returns the new rotation.
	///
	/// Note that this only calculates a new rotation, it does not change the actual rotation of the agent.
	///
	/// See: <see cref="orientation"/> 
	/// See: <see cref="movementPlane"/>
	/// </summary>
	/// <param name="direction">Direction in the movement plane to rotate towards.</param>
	/// <param name="maxDegrees">Maximum number of degrees to rotate this frame.</param>
	protected Quaternion SimulateRotationTowards(Vector2 direction, float maxDegrees)
	{
		if (direction != Vector2.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(_AIPath.movementPlane.ToWorld(direction, 0), _AIPath.movementPlane.ToWorld(Vector2.zero, 1));
			// This causes the character to only rotate around the Z axis
			if (_AIPath.orientation == OrientationMode.YAxisForward) targetRotation *= Quaternion.Euler(90, 0, 0);
			return Quaternion.RotateTowards(this.transform.rotation, targetRotation, maxDegrees);
		}
		return this.transform.rotation;
	}
}
