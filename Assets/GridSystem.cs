using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridSystem
{
	/// The size of each grid square
	public static float gridSquareSize = 6f;
	public static int levelSquaresPerSide = 36;

	/// Get the center of the grid square containing a position
	public static Vector3 GetPoint(Vector3 position)
	{
		return new Vector3(
			Mathf.Round(position.x / gridSquareSize) * gridSquareSize,
			position.y,
			Mathf.Round(position.z / gridSquareSize) * gridSquareSize
		);
	}

	/// Get the next grid square center in front of a position, given a
	/// position and a forward vector
	public static Vector3 GetForwardPoint(Vector3 position, Vector3 forward)
	{
		Vector3 currentGridSquare = GetPoint(position);

		// Return either the current grid square (if they aren't in the middle yet)
		// or the next grid square center.
		return IsBeyondTargetPosition(position, forward, currentGridSquare)
			? currentGridSquare
			: currentGridSquare + (forward * gridSquareSize);
	}

	/// Check whether a position is past a target position, given a target position,
	/// a forward vector, and a current position
	public static bool IsBeyondTargetPosition(Vector3 targetPosition, Vector3 forward, Vector3 position)
	{
		Vector3 axisPosition = Vector3.Scale(position, forward);
		Vector3 targetAxisPosition = Vector3.Scale(targetPosition, forward);
		float axisDistance = axisPosition.x + axisPosition.z;
		float targetAxisDistance = targetAxisPosition.x + targetAxisPosition.z;

		return axisDistance >= targetAxisDistance;
	}
}
