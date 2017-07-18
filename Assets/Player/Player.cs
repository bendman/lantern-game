using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	private float maxSpeed = 3f;

	private bool userMoving = true;

	public override void OnStartLocalPlayer()
	{
		Debug.Log("starting local player");
		// Bind to swipe events to handle turning
		SwipeManager.OnSwipe += OnSwipe;

		// Disable non-player camera and enable player camera and light if it's the local player
		Camera.main.gameObject.SetActive(false);
		GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
	}

	private void Update()
	{
		if (isLocalPlayer && userMoving)
		{
			transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime);
		}
	}

	private void OnSwipe(SwipeManager.SwipeDirection direction, Vector2 delta)
	{
		if (!isLocalPlayer) { return; }

		Debug.Log("swiping");
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
