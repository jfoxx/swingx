using UnityEngine;
using System.Collections;

public class NameTag : MonoBehaviour {

	string playerName = "Player";
	float health = 0;
	TextMesh text;
	bool nameSet = false;

	void Start () {
		if(networkView.isMine){
			playerName = PlayerPrefs.GetString("playerName");
		}
		text = GetComponent<TextMesh>();
	}

	[RPC]
	void updatePlayerName(string pName){
		playerName = pName;
		nameSet = true;
	}
	
	void Update(){

		if(!nameSet && networkView.isMine){
			networkView.RPC("updatePlayerName", RPCMode.OthersBuffered, PlayerPrefs.GetString("playerName"));
			nameSet = true;
		}

		health = transform.parent.GetComponent<Health_mp>().health;
		text.text = playerName;
	}

}
