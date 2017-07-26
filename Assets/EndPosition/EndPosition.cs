using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPosition : MonoBehaviour {
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent && other.transform.parent.CompareTag("Player"))
		{
			Debug.Log("player collision with target");
			GameManager.FinishGame(other.transform.parent.GetComponent<Player>());
		}
	}
}
