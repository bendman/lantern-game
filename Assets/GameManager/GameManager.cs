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
		Vector3 levelSize = GameObject.Find("Floor").GetComponent<Renderer>().bounds.size;

		float maxX = levelSize.x / 2 - (GridSystem.scale / 2);
		float maxZ = levelSize.z / 2 - (GridSystem.scale / 2);

		for (float xPos = -maxX; xPos <= maxX; xPos += GridSystem.scale)
		{
			for (float zPos = -maxZ; zPos <= maxZ; zPos += GridSystem.scale)
			{
				PlaceTorch(new Vector3(xPos, 1.5f, zPos));
			}
		}
	}
	
	private void PlaceTorch(Vector3 position)
	{
		GameObject torch = Instantiate(torchPrefab, position, Quaternion.identity);
		NetworkServer.Spawn(torch);
	}
}
