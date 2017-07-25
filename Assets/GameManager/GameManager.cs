using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
	//
	// Static properties
	//
	private static GameManager instance;

	//
	// Instance properties
	//
	public MapMaker mapper;
	public string level = "testing";
	[SerializeField]
	private GameObject torchPrefab;
	[SyncVar]
	private bool isRoundStarted;

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

		// Initialize round
		isRoundStarted = false;
	}

	public static bool IsStarted() { return instance.isRoundStarted; }

	private void Start()
	{
		mapper.makeMap(level + ".csv", GridSystem.gridSquareSize);
	}

	public void StartRound()
	{
		Debug.Log("Setting Round Started");
		isRoundStarted = true;
		Player[] players = FindObjectsOfType<Player>();
		foreach (Player player in players)
		{
			Debug.Log("Spawning players");
			player.RpcSpawn();
		}

		UIManager.HideUI();
		PlaceTorches();
	}

	/// Temporary until we get a maze in place, this just places torches
	/// at regular intervals.
	private void PlaceTorches()
	{
		for (int xSquare = 0; xSquare <= GridSystem.levelSquaresPerSide; xSquare += 1)
		{
			for (int zSquare = 0; zSquare <= GridSystem.levelSquaresPerSide; zSquare += 1)
			{
				if (xSquare % 2 == zSquare % 2) { continue; }
				PlaceTorch(new Vector3(
					(xSquare - (GridSystem.levelSquaresPerSide / 2) - 0.5f) * GridSystem.gridSquareSize,
					2f,
					(zSquare - (GridSystem.levelSquaresPerSide / 2) - 0.5f) * GridSystem.gridSquareSize
				));
			}
		}
	}

	public static void PlaceTorch(Vector3 position)
	{
		GameObject torch = Instantiate(instance.torchPrefab, position, Quaternion.identity);
		NetworkServer.Spawn(torch);
	}

	public static void FinishGame(NetworkBehaviour winningPlayer)
	{
		if (winningPlayer.isLocalPlayer)
		{
			UIManager.SetActiveUI(UIManager.instance.sceneVictory);
		}
		else
		{
			UIManager.SetActiveUI(UIManager.instance.sceneLoss);
		}

		Player[] players = FindObjectsOfType<Player>();
		foreach (Player player in players)
		{
			player.RpcStop();
		}
	}
}
