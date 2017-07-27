using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorch : MonoBehaviour
{
	public void Light()
	{
		GetComponent<Animator>().SetBool("isLit", true);
	}
}
