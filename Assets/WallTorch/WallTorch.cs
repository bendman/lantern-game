using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorch : MonoBehaviour
{
	public void Light()
	{
		transform.Find("TorchFlame").gameObject.SetActive(true);
	}
}
