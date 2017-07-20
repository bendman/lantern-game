using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorch : MonoBehaviour
{
	public void Light()
	{
		Debug.Log("lighting");
		// gameObject.SetActive(true);
		transform.Find("TorchFlame").gameObject.SetActive(true);
	}
}
