﻿using UnityEngine;
using System.Collections;

public class NameTag : MonoBehaviour 
{

	string playerName = "Player";
	float health = 0;
	TextMesh text;
	bool nameSet = false;
	PlayerManager_mp playerManager;

	void Start () 
	{

		playerManager = GameObject.Find ("Manager").GetComponent<PlayerManager_mp>();

		if(networkView.isMine){
			playerName = playerManager.playerName;
		}
		else{
			playerName = playerManager.getPlayerName(networkView.owner);
		}

		text = GetComponent<TextMesh>();
	}
	
	void Update()
	{
		health = transform.parent.GetComponent<Health_mp>().health;
		if(!nameSet){
			text.text = playerName;
			nameSet = true;
		}
	}

}
