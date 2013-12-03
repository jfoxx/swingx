using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SoundEffects))]
public class GameManager_mp : MonoBehaviour 
{
	public GameState gameState;

	GameObject[] players;
	public string currentLevel;

	private static GameManager_mp instance;

	public static GameManager_mp Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameManager_mp();
			}

			return instance;
		}
	}	

	void Start ( ) 
	{
		gameState 		= GameState.Instance;
		currentLevel 	= gameState.currentLevel;
		players 			= GameObject.FindGameObjectsWithTag("Player");

	}
	
	void Update ( ) 
	{

		if (players.Length <= 0) {
			//gameState.setLevel("main");
		}

	}

	void OnGUI()
	{

	}
}

