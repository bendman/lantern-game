using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerLobbyUI : UIScene
{
	private NetworkManager manager;
	private GameObject startButton;

	public override void Awake()
	{
		base.Awake();

		manager = FindObjectOfType<NetworkManager>();
		startButton = GameObject.Find("StartButton");
	}

	private void Start()
	{
		// Only the server (host) can start the game
		if (!NetworkServer.active)
		{
			startButton.SetActive(false);
		}
	}
}
