using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour {

	GameState state;
	void Start () {
		state = GameState.Instance;
	}

	void Update () {
			
	}

	void OnGUI(){
		if(state.showMenu){
			if (GUI.Button (new Rect (30, 30, 150, 30), "Main Menu"))
			{
				GameState.Instance.setLevel("main");
				Application.LoadLevel("main");
			}
		}
	}
}
