using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public GameObject[] checkpoints;
	GameObject player;
	public int currentLevel;
	public int totalCheckpoints;
	public int reachedCheckpoints;
	public GameState gameState;
	public bool allCheckpointsReached = false;
	public GameObject gate;

	void Start ( ) 
	{
		gameState = GameState.Instance;
		currentLevel = Application.loadedLevel;
		player = GameObject.Find("Player");
		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		totalCheckpoints = checkpoints.Length;
		reachedCheckpoints = 0;
		gate = GameObject.Find("Gate");

	}
	
	void Update ( ) 
	{
		if (player == null) {
			Application.LoadLevel(currentLevel);
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
		Application.LoadLevel(currentLevel);
	}
	
	void checkpointReached()
	{
		reachedCheckpoints ++;

	}
}

