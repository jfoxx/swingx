using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Our Startscreen GUI

	void OnGUI () 
	{

		float height = 150;
		float width = Screen.width/ 2;
		float left = Screen.width/2 -width/2;
		float top = Screen.height / 2 - height/2;
		if(GUI.Button(new Rect (left, top, width, height), "Start Game"))
		{
			startGame();
		}
	}
	private void startGame()
	{
		Debug.Log("Starting game");
		
		DontDestroyOnLoad(GameState.Instance);
		GameState.Instance.startState();
	}
	
}
