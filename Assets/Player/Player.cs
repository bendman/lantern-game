using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	private float maxSpeed = 4.5f;

	private bool isMoving = true;
	// private int pendingTurnDirection = 0; // -1, 0, 1 = Left, None, Right
	private Vector2 pendingTurnDirection = Vector2.zero;
	private Vector3 pendingTurnPosition;
	private Vector3 pendingStopPoint = Vector3.zero;
	private Animator myAnimator;
	private Animator bodyAnimator;
	private Animator cameraBoomAnimator;
	private bool isPendingDecision;
	private Vector3 previousDecisionPoint = Vector3.zero;

	// public override void OnStartLocalPlayer()
	// {
	// }

	private void Awake()
	{
		myAnimator = transform.GetComponent<Animator>();
		bodyAnimator = transform.Find("PlayerBody").GetComponent<Animator>();
		cameraBoomAnimator = transform.Find("CameraBoom").GetComponent<Animator>();
	}

	private void Update()
	{
		Debug.DrawRay(transform.position + Vector3.up, transform.forward * GridSystem.gridSquareSize, Color.blue);
		Debug.DrawRay(transform.position + Vector3.up, transform.right * GridSystem.gridSquareSize, Color.green);
		Debug.DrawRay(transform.position + Vector3.up, -transform.right * GridSystem.gridSquareSize, Color.red);
		//userMoving = CanMove();
		if (!isLocalPlayer || !GameManager.IsStarted()) { return; }

		if (!isPendingDecision) { HandleForwardMovement(); }
		else { HandleDecisions(); }
		//HandleTurns();
	}

	private void HandleForwardMovement() {
		Debug.Log("HandleForwardMovement");
		if (isMoving) { transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime); }

		if (GridSystem.GetPoint(transform.position) == previousDecisionPoint) { Debug.Log("in previous decision point"); return; }

		// No stop point, so continue forward looking for one on the way
		if (pendingStopPoint == Vector3.zero)
		{

			bool up = HasAdjascentWall(transform.forward);
			bool right = HasAdjascentWall(transform.right);
			bool left = HasAdjascentWall(-transform.right);

			Debug.Log("no pending stop point, wall check. up: " + up + "  right: " + right + "  left: " + left);

			if (up || !right || !left)
			{
				pendingStopPoint = GridSystem.GetForwardPoint(transform.position, transform.forward);
				Debug.Log("failed wall check. pendingStopPoint: " + pendingStopPoint);
			}
		}
		else if (GridSystem.IsBeyondTargetPosition(pendingStopPoint, transform.forward, transform.position))
		{
			Debug.Log("beyond pendingStopPoint: " + pendingStopPoint);
			isPendingDecision = true;
			StopWalking();
		}
	}

	private bool HasAdjascentWall(Vector2 direction) { return HasAdjascentWall(new Vector3(direction.x, 0, direction.y)); }
	private bool HasAdjascentWall(Vector3 direction)
	{
		int wallLayer = 1 << 9;
		bool hasWall = Physics.Raycast(transform.position, direction, GridSystem.gridSquareSize, wallLayer);
		return hasWall;
	}

	public void StartWalking()
	{
		bodyAnimator.SetBool("isWalking", true);
		isMoving = true;
	}

	public void StopWalking()
	{
		bodyAnimator.SetBool("isWalking", false);
		isMoving = false;
		pendingStopPoint = Vector3.zero;
	}

	[ClientRpc]
	public void RpcOnVictory()
	{
		StopWalking();
		bodyAnimator.SetTrigger("doVictory");
		cameraBoomAnimator.SetTrigger("doVictory");
	}

	[ClientRpc]
	public void RpcOnLoss()
	{
		StopWalking();
		bodyAnimator.SetTrigger("doLoss");
		cameraBoomAnimator.SetTrigger("doVictory");
	}

	private void HandleDecisions()
	{
		// HandleTurns();
	}

// 	private void HandleTurns()
// 	{
// 		// Determine what direction the player is facing, and
// 		// check if they have reached the next available turn.
// 		if (pendingTurnDirection == Vector2.zero) { return; }
// //		if (!GridSystem.IsBeyondTargetPosition(pendingTurnPosition, transform.forward, transform.position)){ return; }
// 		Debug.Log("Executing turn");

// 		ExecuteTurn(pendingTurnDirection);
// 		pendingTurnDirection = Vector2.zero;
// 	}

	private void OnSwipe(SwipeManager.SwipeDirection direction, Vector2 delta) {
		if(!isLocalPlayer || !GameManager.IsStarted()) {
			return;
		}

		Vector2 turnDirection;

		// Ensure they swipe far enough, not just tap
		if (delta.magnitude < 50f) { return; }
		else if (direction == SwipeManager.SwipeDirection.Left)
		{
			turnDirection = Vector2.left;
		}
		else if (direction == SwipeManager.SwipeDirection.Right)
		{
			turnDirection = Vector2.right;
		}
		else if (direction == SwipeManager.SwipeDirection.Up)
		{
			turnDirection = Vector2.up;
		}
		else
		{
			turnDirection = Vector2.down;
		}

		Debug.Log("OnSwipe turnDirection:" + turnDirection);

		if (HasAdjascentWall(transform.TransformDirection(turnDirection))) { Debug.Log("adjascent wall"); return; }
		TurnPlayer(turnDirection);
		if (isPendingDecision) { ExecuteTurn(pendingTurnDirection); }
	}

	[ClientRpc]
	public void RpcSpawn()
	{
		Debug.Log("spawning local player");
		if (!isLocalPlayer) { return; }

		// Bind to swipe events to handle turning
		SwipeManager.OnSwipe += OnSwipe;

		// Disable non-player camera and enable player camera and light if it's the local player
		Camera.main.gameObject.SetActive(false);
		GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
		StartWalking();
	}

	[ClientRpc]
	public void RpcStop()
	{
		StopWalking();
	}


	private void TurnPlayer(Vector2 direction)
	{
		// TODO: check for the next available turn in that direction
		pendingTurnDirection = direction;
		//pendingTurnPosition = GridSystem.GetForwardPoint(transform.position, transform.forward);
	}

	private void ExecuteTurn(Vector2 direction)
	{
		Debug.Log("Executing turn");
		float angle;
		if (direction == Vector2.left) { angle = -90f; }
		else if (direction == Vector2.right) { angle = 90f; }
		else if (direction == Vector2.up) { angle = 0; }
		else { angle = 180f; }
		transform.Rotate(0, angle, 0, Space.World);
		previousDecisionPoint = GridSystem.GetPoint(transform.position);
		isPendingDecision = false;
		StartWalking();
		// if (direction == Vector2.left) { myAnimator.SetTrigger("LeftTurn"); }
		// else if (direction == Vector2.right) { myAnimator.SetTrigger("RightTurn"); }
	}
}
