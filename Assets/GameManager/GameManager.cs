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
	}
	
}
