using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SoundEffects))]
public class GameManager : MonoBehaviour 
{
	public GameState gameState;

	GameObject player;
	GameObject gate;

	public string currentLevel;

	public GameObject[] checkpoints;
	public int totalCheckpoints;
	public int reachedCheckpoints;

	public bool allCheckpointsReached = false;

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameManager();
			}

			return instance;
		}
	}	

	void Start ( ) 
	{
		gameState 					= GameState.Instance;
		currentLevel 				= gameState.currentLevel;

		gate 								= GameObject.Find("Gate");
		player 							= GameObject.Find("Player");
		checkpoints 				= GameObject.FindGameObjectsWithTag("Checkpoint");

		totalCheckpoints 		= checkpoints.Length;
		reachedCheckpoints 	= 0;

	}
	
	void Update ( ) 
	{
		if (player == null) {
			gameState.setLevel("main");
		}
		if (reachedCheckpoints >= totalCheckpoints && !allCheckpointsReached){
			allCheckpointsReached = true;
		}
		if (allCheckpointsReached) {
			if(gate != null){
				Destroy(gate);
			}
		}
	}

	void OnFinish ()
	{
		gameState.setLevel("main");
	}
	
	void checkpointReached()
	{
		reachedCheckpoints ++;
		gameObject.SendMessage("OnCheckpointReached");

	}
	void OnGUI()
	{
		GUI.Box(new Rect (Screen.width - 200, Screen.height - 80, 200, 30), "Red circles found: " + reachedCheckpoints + "/" + totalCheckpoints);
	}
}

