using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLighter : MonoBehaviour
{
	private void Awake()
	{
		
	}
	
	private void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("WallTorch"))
		{
			other.GetComponent<WallTorch>().Light();
		}
	}
}
