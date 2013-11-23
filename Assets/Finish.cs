using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
	GameObject gameManager;

	void Start ()
	{
		gameManager = GameObject.Find ("GameManager");
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if (coll.transform.name == "Player") {
			gameManager.SendMessage ("OnFinish");
		}
	}

}
