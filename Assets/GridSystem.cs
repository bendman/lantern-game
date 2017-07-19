using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridSystem
{
	public const float scale = 6f;

	public static Vector3 GetPoint(Vector3 position)
	{
		return new Vector3(
			Mathf.Round(position.x / scale) * scale,
			position.y,
			Mathf.Round(position.z / scale) * scale
		);
	}

	public static Vector3 GetForwardPoint(Vector3 position, Vector3 forward)
	{
		Vector3 currentGridSquare = GetPoint(position);

		// Return either the current grid square (if they aren't in the middle yet)
		// or the next grid square center.
		return IsBeyondTargetPosition(position, forward, currentGridSquare)
			? currentGridSquare
			: currentGridSquare + (forward * scale);
	}

	public static bool IsBeyondTargetPosition(Vector3 targetPosition, Vector3 forward, Vector3 position)
	{
		Vector3 axisPosition = Vector3.Scale(position, forward);
		Vector3 targetAxisPosition = Vector3.Scale(targetPosition, forward);
		float axisDistance = axisPosition.x + axisPosition.z;
		float targetAxisDistance = targetAxisPosition.x + targetAxisPosition.z;

		return axisDistance >= targetAxisDistance;
	}
}
