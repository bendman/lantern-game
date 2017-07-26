using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
	// 
	// Static properties
	//
	public enum SwipeDirection { Left, Right, Up, Down }
	private static SwipeManager instance;

	// 
	// Instance properties
	//
	public delegate void Swipe(SwipeDirection direction, Vector2 delta);
	public static event Swipe OnSwipe;

	private Vector2 initialTouchPosition;

	//
	// Methods
	//

	private void Awake()
	{
		// Handle persistant instance
		if (instance != null) {
			Destroy(gameObject);
			return;
		}
		instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		HandleDowns();
		HandleUps();
	}

	private void HandleDowns()
	{
		// Handle touch start
		if (Input.touchSupported && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			initialTouchPosition = Input.GetTouch(0).position;
		}
		// Handle mouse down
		else if (Input.GetMouseButtonDown(0)) { initialTouchPosition = Input.mousePosition; }
	}

	private void HandleUps()
	{
		Vector2 finalTouchPosition;

		// Handle touch end
		if (Input.touchSupported && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			finalTouchPosition = Input.GetTouch(0).position;
		}
		// Handle mouse up
		else if (Input.GetMouseButtonUp(0)) { finalTouchPosition = Input.mousePosition; }
		// No touch end or mouse up, so exit
		else { return; }

		Vector2 touchDelta = finalTouchPosition - initialTouchPosition;

		SwipeDirection direction;

		// Horizontal Swipe
		if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
		{ direction = touchDelta.x < 0 ? SwipeDirection.Left : SwipeDirection.Right; }
		// Vertical Swipe
		else { direction = touchDelta.y < 0 ? SwipeDirection.Down : SwipeDirection.Up; }

		// Handle OnSwipe events
		if (OnSwipe != null) { OnSwipe(direction, touchDelta); }
	}


}
