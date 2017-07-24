using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetupUI : UIScene
{
	private NetworkManager manager;
	private GameObject ipInput;
	private GameObject connectButton;
	private GameObject hostButton;
	private GameObject startButton;

	public override void Awake()
	{
		base.Awake();

		manager = FindObjectOfType<NetworkManager>();
		ipInput = transform.Find("IPInput").gameObject;
		connectButton = transform.Find("ConnectButton").gameObject;
		hostButton = transform.Find("HostButton").gameObject;

		// Hide host button for WebGL clients, because they can't host
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			hostButton.SetActive(false);
		}
	}

	private void Update()
	{
		if (NetworkServer.active)
		{
			ipInput.SetActive(false);
			connectButton.SetActive(false);
			hostButton.SetActive(false);
		}
		else if (NetworkClient.active && manager.IsClientConnected())
		{
			ipInput.SetActive(false);
			connectButton.SetActive(false);
			hostButton.SetActive(false);
			startButton.SetActive(false);
		}
	}

	public void StartClient()
	{
		Debug.Log("Starting client");
		manager.StartClient();
		UIManager.SetActiveUI(UIManager.instance.sceneMultiplayerLobby);
	}

	public void StartServer()
	{
		Debug.Log("Starting host");
		// manager.StartServer();
		manager.StartHost();
		UIManager.SetActiveUI(UIManager.instance.sceneMultiplayerLobby);
	}

	// Mobile WebGL input isn't working, so disable this for now
	// public void OpenKeyboard()
	// {
	// 	TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
	// }
	//
	// public void OnGUI()
	// {
	// 	Debug.Log("on gui");
	// 	manager.networkAddress = GUI.TextField(new Rect(50f, 50f, 200f, 60f), manager.networkAddress);
	// }
	//
	// public void SetServerIP(string address)
	// {
	// 	manager.networkAddress = address;
	// }
}
