using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
	//
	// Static properties
	//
	private static GameManager instance = null;

	//
	// Instance properties
	//
	public string level = "testing";
	[SyncVar]
	private bool isRoundStarted;
	private MapMaker mapper;

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
		mapper = GetComponent<MapMaker>();
	}

	public static bool IsStarted() { return instance.isRoundStarted; }

	// private void Start()
	// {
	// 	mapper.MakeMap(level + ".csv", GridSystem.gridSquareSize);
	// }

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
	}

	public static void FinishGame(Player winningPlayer)
	{

		Player[] players = FindObjectsOfType<Player>();
		foreach (Player player in players)
		{
			if (winningPlayer == player)
			{
				UIManager.SetActiveUI(UIManager.instance.sceneVictory);
				player.RpcOnVictory();
			}
			else
			{
				UIManager.SetActiveUI(UIManager.instance.sceneLoss);
				player.RpcOnLoss();
			}
		}
	}
}
