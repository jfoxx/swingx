using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour {
	GameObject gameManager;

	void Start(){
		GameObject.Find("GameManager");
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		coll.transform.SendMessage ("onKillZone", SendMessageOptions.DontRequireReceiver);
	}

}
