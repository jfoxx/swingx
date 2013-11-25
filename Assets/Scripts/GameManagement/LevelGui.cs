using UnityEngine;
using System.Collections;

public class LevelGui : MonoBehaviour {



	void Start () 
	{
		print ("Loaded: " + GameState.Instance.getLevel());
	}

	void OnGUI()
	{		
				float height = Screen.height - 200;
				float width = 150;
				float left = 0;
				float top = Screen.height / 2 - height/2;

		GUILayout.BeginArea(new Rect(left, top, width, height));


		if (GUILayout.Button ( "intro"))
		{
			Debug.Log ("intro");
			GameState.Instance.setLevel("intro");
		}

		GUILayout.Space(5);
		
		if (GUILayout.Button ("Level 1"))
		{
			Debug.Log ("Moving to level 1");
			GameState.Instance.setLevel("level1");

		}

		GUILayout.Space(5);

		if (GUILayout.Button ("Level 2"))
		{
			Debug.Log ("Moving to level 2");
			GameState.Instance.setLevel("level2");
		}

		GUILayout.FlexibleSpace();

		if(!Application.isEditor && !Application.isWebPlayer){

			if (GUILayout.Button ( "Quit"))
			{
				Debug.Log ("exit");
				Application.Quit();
			}
		}
		GUILayout.EndArea();
	}
}
