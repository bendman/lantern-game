using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Goal : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent && other.transform.parent.CompareTag("Player"))
		{
			GameManager.FinishGame(other.transform.parent.GetComponent<Player>());
		}
	}
}
