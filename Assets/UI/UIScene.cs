using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIScene : MonoBehaviour
{
	public virtual void Awake()
	{
		if (gameObject.activeSelf) { UIManager.SetActiveUI(this); }
	}

	public void ShowScene()
	{
		gameObject.SetActive(true);
	}

	public void HideScene()
	{
		gameObject.SetActive(false);
	}
}
