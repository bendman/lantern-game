using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private float maxSpeed = 3f;

	private bool userMoving = true;

	private void Awake()
	{
		SwipeManager.OnSwipe += OnSwipe;
	}

	private void Update()
	{
		if (userMoving)
		{
			transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime);
		}
	}

	private void OnSwipe(SwipeManager.SwipeDirection direction, Vector2 delta)
	{
		// Ensure they swipe far enough, not just tap
		if (Mathf.Abs(delta.x) <= 50f) { return; }
		else if (direction == SwipeManager.SwipeDirection.Left)
		{
			TurnPlayer(Vector2.left);
		}
		else if (direction == SwipeManager.SwipeDirection.Right)
		{
			TurnPlayer(Vector2.right);
		}
	}

	private void TurnPlayer(Vector2 direction)
	{
		float angle = (direction == Vector2.left) ? -90f : 90f;
		transform.Rotate(0, angle, 0, Space.World);
	}
}
