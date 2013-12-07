using UnityEngine;
using System.Collections;

public class HUD_mp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!networkView.isMine){enabled = false;}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		int ping = 0;
		if(Network.connections.Length > 0){
			ping = Network.GetAveragePing(Network.connections[0]);
		}

		GUI.Label(new Rect(Screen.width - 150, 30, 100, 30), "Velocity: " + Mathf.CeilToInt(rigidbody2D.velocity.magnitude));
		GUI.Label(new Rect(150, Screen.height - 30, 100, 30), "Players: " + (Network.connections.Length + 1)); // +1 for the server
		GUI.Label(new Rect(Screen.width / 2, Screen.height - 30, 100, 30), "ping: " + ping);
	}
}
