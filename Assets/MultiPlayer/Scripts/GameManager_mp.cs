using UnityEngine;
using System.Collections;


public class GameManager_mp : MonoBehaviour 
{

	public GameState gameState;

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

	}

}


