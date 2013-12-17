using UnityEngine;
using System.Collections;

public class Hill : MonoBehaviour {


	private float interval = 1;
	private float timer;
	private GameObject manager;

	void Start () 
	{
		timer = interval;
		manager = GameObject.Find("Manager");
	}
	
	void Update () 
	{	
		if(timer > 0){
			timer -= Time.deltaTime;
		}

	}

	void OnTriggerStay2D(Collider2D coll) 
	{
		if(timer <= 0 && coll.transform.CompareTag("player"))
		{
			updatePlayeScore(coll.networkView.owner);
		}
	}

	void updatePlayeScore(NetworkPlayer player)
	{
		timer = interval;
		manager.networkView.RPC("updatePlayerScore", RPCMode.AllBuffered, player);
	}

}
