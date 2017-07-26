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
	private Vector3 pendingStopPoint;
	private Animator myAnimator;
	private Animator bodyAnimator;
	private Animator cameraBoomAnimator;

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
		//userMoving = CanMove();
		Debug.Log("canmove: " + CanMove());
		if (!isLocalPlayer || !GameManager.IsStarted() || !CanMove()) { return; }

		HandleForwardMovement();
		//HandleTurns();
	}

	private bool CanMove() {
		bool canMove = true;
		int wallLayer = 1 << 9;

		bool up = Physics.Raycast(transform.position, transform.forward, GridSystem.gridSquareSize);//, wallLayer);
		Debug.DrawRay(transform.position + Vector3.up, transform.forward * GridSystem.gridSquareSize, Color.blue);

		bool right = Physics.Raycast(transform.position, transform.right, GridSystem.gridSquareSize);//, wallLayer);
		Debug.DrawRay(transform.position + Vector3.up, transform.right * GridSystem.gridSquareSize, Color.red);

		bool left = Physics.Raycast(transform.position, -transform.right, GridSystem.gridSquareSize);//, wallLayer);
		Debug.DrawRay(transform.position + Vector3.up, -transform.right * GridSystem.gridSquareSize, Color.green);


		Debug.Log("up: " + up + "  right: " + right + "  left: " + left);

		//Checks for walls
		if(up || !right || !left) {
			pendingStopPoint = GridSystem.GetPoint(transform.position);

			if(GridSystem.IsBeyondTargetPosition(pendingStopPoint, transform.forward, transform.position)) {
				canMove = HandleTurns();
			}
		}

		return canMove;
	}

	public void StartWalking()
	{
		bodyAnimator.SetBool("isWalking", true);
		cameraBoomAnimator.SetBool("isVictory", false);
		isMoving = true;
	}

	public void StopWalking()
	{
		bodyAnimator.SetBool("isWalking", false);
		cameraBoomAnimator.SetBool("isVictory", true);
		isMoving = false;
	}

	private void HandleForwardMovement()
	{
		transform.Translate(Vector3.forward * maxSpeed * Time.deltaTime);
	}

	private bool HandleTurns()
	{
		// Determine what direction the player is facing, and
		// check if they have reached the next available turn.
		if (pendingTurnDirection == Vector2.zero) {return false; }
//		if (!GridSystem.IsBeyondTargetPosition(pendingTurnPosition, transform.forward, transform.position)){ return; }

		ExecuteTurn(pendingTurnDirection);
		pendingTurnDirection = Vector2.zero;
		return true;
	}

	private void OnSwipe(SwipeManager.SwipeDirection direction, Vector2 delta) {
		if(!isLocalPlayer || !GameManager.IsStarted()) {
			return;
		}

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
		float angle = (direction == Vector2.left) ? -90f : 90f;
		transform.Rotate(0, angle, 0, Space.World);
		// if (direction == Vector2.left) { myAnimator.SetTrigger("LeftTurn"); }
		// else if (direction == Vector2.right) { myAnimator.SetTrigger("RightTurn"); }
	}
}
