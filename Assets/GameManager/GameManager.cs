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

		PlaceTorches();
	}

	/// Temporary until we get a maze in place, this just places torches
	/// at regular intervals.
	private void PlaceTorches()
	{
		float levelSize = GridSystem.gridSquareSize * GridSystem.levelSquaresPerSide;

		float maxX = levelSize / 2;// - (GridSystem.gridSquareSize / 2);
		float maxZ = levelSize / 2;// - (GridSystem.gridSquareSize / 2);

		for (float xPos = -maxX; xPos <= maxX; xPos += GridSystem.gridSquareSize)
		{
			for (float zPos = -maxZ; zPos <= maxZ; zPos += GridSystem.gridSquareSize)
			{
				PlaceTorch(new Vector3(xPos, 2f, zPos));
			}
		}
	}
	
	public static void PlaceTorch(Vector3 position)
	{
		GameObject torch = Instantiate(instance.torchPrefab, position, Quaternion.identity);
		NetworkServer.Spawn(torch);
	}
}
