using UnityEngine;
using System.Collections;

public class LevelGui : MonoBehaviour {
	
	// Initialize level
	void Start () 
	{
		print ("Loaded: " + GameState.Instance.getLevel());
	}
	
	// ---------------------------------------------------------------------------------------------------
	// OnGUI()
	// --------------------------------------------------------------------------------------------------- 
	// Provides a GUI on level scenes
	// ---------------------------------------------------------------------------------------------------
	void OnGUI()
	{		
		// Create buttons to move between level 1 and level 2
		if (GUI.Button (new Rect (30, 60, 150, 30), "intro"))
		{
			Debug.Log ("intro");
			GameState.Instance.setLevel("intro");
		}
		
		if (GUI.Button (new Rect (300, 60, 150, 30), "Level 1"))
		{
			Debug.Log ("Moving to level 1");
			GameState.Instance.setLevel("level1");

		}
		if (GUI.Button (new Rect (600, 60, 150, 30), "Level 2"))
		{
			Debug.Log ("Moving to level 2");
			GameState.Instance.setLevel("level2");
		}
		if(!Application.isEditor && !Application.isWebPlayer){

			if (GUI.Button (new Rect (30, Screen.height -60, 150, 30), "Quit"))
			{
				Debug.Log ("exit");
				Application.Quit();
			}
		}
	}
}
