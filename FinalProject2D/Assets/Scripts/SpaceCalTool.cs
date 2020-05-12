using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpaceCalTool
{

	/// <summary>
	/// Checks if there is no obsticales between the objects, and if thier distance less than the given range
	/// </summary>
	/// <param name="obj1">First Obj</param>
	/// <param name="obj2">Second Obj</param>
	/// <param name="range">Max Distance</param>
	/// <returns></returns>
	public static bool AreObjectsViewableAndWhithinRange(GameObject obj1, GameObject obj2, float range)
    {
        Vector2 rayDirection = GetVectorDirectionTowardTarget(obj1.transform.position, obj2.transform.position);
		RaycastHit2D[] hittedObjects = Physics2D.RaycastAll(obj1.transform.position, rayDirection);

		RaycastHit2D hittedObject = GetFirstObjectThatNotItSelfOrTerrain(hittedObjects, obj1);

		if (hittedObject.collider != null)
        {
            if (hittedObject.transform.gameObject == obj2 && hittedObject.distance < range)
                return true;
        }

        return false;
    }

	private static RaycastHit2D GetFirstObjectThatNotItSelfOrTerrain(RaycastHit2D[] hittedObjects, GameObject obj)
	{
		int i = 0;
		while ( i < hittedObjects.Length && (hittedObjects[i].transform.gameObject == obj || hittedObjects[i].transform.gameObject.tag == "Terrain"))
			++i;

		if (i >= hittedObjects.Length)
			return new RaycastHit2D();

		return hittedObjects[i];
	}


	/// <summary>
	/// Calculate the speed relative to the direction
	/// </summary>
	/// <param name="direction">The direction that the object moves toward</param>
	/// <param name="speed">The object speed, moving on X axis</param>
	/// <returns>The speed relatives to the direction</returns>
	public static float GetRelativeSpeedToDirection(Vector2 direction, float speed)
	{
		Vector2 normDirection = direction.normalized; // # TO BE Implemented in a more effiect way
		return (speed * normDirection.x) + (speed * normDirection.y * GlobalCodeSettings.Y_RELATIVE_TO_X);
	}

	#region Rotations
	/// <summary>
	/// Checks if the target is infront of the object, regarding the distanses
	/// if the height is not set to be calculate, would check only relative to the X and Z axis
	/// otherwise, would check the Y axis too.
	/// Author: Ilan
	/// </summary>
	/// /// <param name="current">The target to check</param>
	/// <param name="targetPos">The target to check</param>
	/// <returns>true if the target infront of the object</returns>
	public static bool IsLookingTowardsTheTarget(GameObject current, Vector2 targetPos)
	{
		Vector2 currentPos = current.transform.position;

		Vector2 direction = GetVectorDirectionTowardTarget(currentPos, targetPos); // Gets the direction vector
		float angelDif = CalcDiffAngle(current, direction); // Gets the angle diffrent of the object regarding the target pos
		return angelDif <= GlobalCodeSettings.DESIRED_POS_MARGIN_OF_ERROR * 0.001f; // return true if the diffrence is less than the globam margin of error
	}

	/// <summary>
	/// Checks if the distance between two vectors is less than the given distance.
	/// Author: Ilan
	/// </summary>
	/// <param name="pos1">The first vector position</param>
	/// <param name="pos2">The second vector position</param>
	/// <param name="distance">The maximum distance</param>
	/// <returns>True if the distance between the positions is less than distance</returns>
	public static bool IsDistanceBetweenTwoPosesLessThan(Vector2 pos1, Vector2 pos2, float distance)
	{
		//float dis = DistancePow2(pos1, pos2);
		//Debug.Log("Distance: " + dis);
		return DistancePow2(pos1, pos2) <= (distance * distance);
	}

	/// <summary>
	/// Calculate the diffrance between the current angel, to the require 
	/// Author: Ilan 
	/// </summary>
	/// <param name="target">The target direction vector (desiredPos - transform.position) </param>
	/// <returns>Returns the diffrances angle</returns>
	public static float CalcDiffAngle(GameObject gameObject, Vector3 targetDirection)
	{
		return CalcDiffAngle(gameObject.transform.rotation, targetDirection);
	}

	/// <summary>
	/// Calculate the diffrance between the current angel, to the require 
	/// Author: Ilan 
	/// </summary>
	/// <param name="target">The target direction vector (desiredPos - transform.position) </param>
	/// <returns>Returns the diffrances angle</returns>
	public static float CalcDiffAngle(Quaternion current, Vector2 targetDirection)
	{
		float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
		Quaternion newDirection = Quaternion.RotateTowards(current, Quaternion.Euler(0, 0, angle), 360f);
		return Quaternion.Angle(current, newDirection);
	}


	#endregion

	#region Direction And Distances
	/// <summary>
	/// Calculate the diffrence vector between the object to the given target,
	/// if the obj position would change by this vector, 
	/// (add the result to the obj pos) the object would be locate at the target pos.
	/// Author: Ilan
	/// </summary>
	/// <param name="current">The current position vector of the object</param>
	/// <param name="target">The target position vector</param>
	/// <returns>The diffrence vector (target - this.transform.position) </returns>
	public static Vector2 GetVectorDirectionTowardTarget(Vector2 current, Vector2 target)
	{
		return target - current;
	}


	/// <summary>
	/// Calculate the distance between the two vector, powed by 2
	/// Author: Ilan
	/// </summary>
	/// <param name="v1">The 1th vector</param>
	/// <param name="v2">The 2th vector</param>
	/// <returns>(distance)^2</returns>
	public static float DistancePow2(Vector2 v1, Vector2 v2)
	{
		return (v1 - v2).sqrMagnitude;
	}

	/// <summary>
	/// Calculate the closet position from current to the targetPos, that keep the mention distance from the target
	/// Author: Ilan
	/// </summary>
	/// <param name="reqDistance">The minimu distance required</param>
	/// <param name="current">The object itself Pos</param>
	/// <param name="targetPos">The tatget Pos</param>
	/// <returns></returns>
	public static Vector3 CalcClosestPosWithThisDistance(float reqDistance, Vector3 current, Vector3 targetPos)
	{
		Debug.Log("calcClosestPosWithThisDistance");
		float distance = Vector3.Distance(current, targetPos) - reqDistance;
		if (distance <= 0)
			return current;

		return current + (targetPos - current/*_desiredRotationDirection*/).normalized * distance;
	}

	// TO_ADD_DOC
	public static bool IsObjectOnMovment(GameObject obj)
	{
		if (obj == null)
			return false;

		Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
		if (rb != null && Vector2.SqrMagnitude(rb.velocity) < GlobalCodeSettings.Minumum_Movment_To_Count)
			return true;

		return false;
	}
    #endregion
}
