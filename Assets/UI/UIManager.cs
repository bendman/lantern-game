using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager instance { get; private set; }

	[Header("UI Scenes")]
	public UIScene sceneMultiplayerSetup;
	public UIScene sceneMultiplayerLobby;
	public UIScene sceneVictory;
	public UIScene sceneLoss;

	private static UIScene activeUI;
	private static UIScene outboundUI;

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
	
	/// Transition to another UI scene
	public static void SetActiveUI(UIScene inboundUI)
	{
		Debug.Log("Switching active UI");
		HideUI();

		// Show the new active scene
		activeUI = inboundUI;
		activeUI.ShowScene();
	}

	/// Hide the previous UI scene
	public static void HideUI()
	{
		if (!activeUI) { return; }

		outboundUI = activeUI;
		outboundUI.HideScene();
	}
}
