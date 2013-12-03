using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SoundEffects))]
public class GameManager_mp : MonoBehaviour 
{
	public GameState gameState;

	GameObject[] players;
	GameObject gate;

	public string currentLevel;

	public GameObject[] checkpoints;
	public int totalCheckpoints;
	public int reachedCheckpoints;

	public bool allCheckpointsReached = false;

	//not implemented
	float waitAfterFinishTimer = 0;
	float waitAfterFinishTime = 1;
	
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
		players 							= GameObject.FindGameObjectsWithTag("Player");

	}
	
	void Update ( ) 
	{

		if (players == null) {
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

	void OnGUI()
	{
		if(totalCheckpoints > 0){
			GUI.Box(new Rect (Screen.width - 200, Screen.height - 80, 200, 30), "Red circles found: " + reachedCheckpoints + "/" + totalCheckpoints);
		}
	}
}

