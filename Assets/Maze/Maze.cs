using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	[SerializeField]
	private int gridSquaresPerAxis;

	private Renderer myRenderer;

	private void Start()
	{
		// Base scaling on renderer bounds
		myRenderer = GetComponentInChildren<Renderer>();

		// Ensure the maze is centered
		Vector3 center = myRenderer.bounds.center * 2;
		transform.position = new Vector3(-center.x, 0, -center.z);

		// Update GridSystem with new scales
		Vector3 extents = myRenderer.bounds.extents;
		GridSystem.gridSquareSize = extents.x * 2 / gridSquaresPerAxis;
		GridSystem.levelSquaresPerSide = gridSquaresPerAxis;

		// For debugging the level/grid placement
		// GameManager.PlaceTorch(new Vector3(extents.x, 2f, extents.z));
		// GameManager.PlaceTorch(new Vector3(-extents.x, 2f, extents.z));
		// GameManager.PlaceTorch(new Vector3(-extents.x, 2f, -extents.z));
		// GameManager.PlaceTorch(new Vector3(extents.x, 2f, -extents.z));
	}
	
	private void Update()
	{
		
	}
}
