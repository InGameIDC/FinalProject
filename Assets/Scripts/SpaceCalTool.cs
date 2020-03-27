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
        Vector3 rayDirection = obj2.transform.position - obj1.transform.position;
        RaycastHit hittedObject;
        if (Physics.Raycast(obj1.transform.position, rayDirection, out hittedObject))
        {
            if (hittedObject.transform.gameObject == obj2 && hittedObject.distance < range)
                return true;
        }

        return false;
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
	public static bool IsLookingTowardsTheTarget(GameObject current, Vector3 targetPos, bool isHeightCalculated)
	{
		Vector3 currentPos = current.transform.position;

		if (!isHeightCalculated)
			targetPos.y = currentPos.y;

		Vector3 direction = GetVectorDirectionTowardTarget(currentPos, targetPos); // Gets the direction vector
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
	public static bool IsDistanceBetweenTwoPosesLessThan(Vector3 pos1, Vector3 pos2, float distance)
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
		return CalcDiffAngle(gameObject.transform.rotation, gameObject.transform.forward, targetDirection);
	}

	/// <summary>
	/// Calculate the diffrance between the current angel, to the require 
	/// Author: Ilan 
	/// </summary>
	/// <param name="target">The target direction vector (desiredPos - transform.position) </param>
	/// <returns>Returns the diffrances angle</returns>
	public static float CalcDiffAngle(Quaternion current, Vector3 forward, Vector3 targetDirection)
	{
		Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, 360f, 0.0f); // cacls the vector rotation diffrence
		Quaternion rotationLeft = Quaternion.LookRotation(newDirection);
		return Quaternion.Angle(current, rotationLeft); // calcs the angle diffrence between the current rotation the rotationLeft vector
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
    public static Vector3 GetVectorDirectionTowardTarget(Vector3 current, Vector3 target)
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
	public static float DistancePow2(Vector3 v1, Vector3 v2)
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

    #endregion
}
