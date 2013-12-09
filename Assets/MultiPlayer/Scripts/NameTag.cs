using UnityEngine;
using System.Collections;

public class NameTag : MonoBehaviour {

	string playerName = "Player";
	float health = 0;
	TextMesh text;
	bool nameSet = false;
	PlayerManager_mp playerManager;

	float timer = 2;

	void Start () {

		playerManager = GameObject.Find ("Manager").GetComponent<PlayerManager_mp>();

		if(networkView.isMine){
			playerName = playerManager.playerName;
		}

		text = GetComponent<TextMesh>();

	}

	[RPC]
	void updatePlayerName(string pName){
		playerName = pName;
		nameSet = true;
	}

	void Update(){

		if(timer > 0){
			timer -= Time.deltaTime;
		}

		if(!nameSet && networkView.isMine && timer < 0 ){

			networkView.RPC("updatePlayerName", RPCMode.AllBuffered, PlayerPrefs.GetString("playerName"));

			playerManager.networkView.RPC("updatePlayerName", RPCMode.AllBuffered, networkView.owner, PlayerPrefs.GetString("playerName"));

			nameSet = true;
		}

		health = transform.parent.GetComponent<Health_mp>().health;

		text.text = playerName;
	}

}
