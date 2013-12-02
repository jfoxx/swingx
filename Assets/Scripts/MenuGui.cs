using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour {

	GameState state;
	public bool mouseAim = false;
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
			state.mouseAim = GUI.Toggle(new Rect(30, 70, 150, 20), state.mouseAim, "use mouse to aim");
		}
	}
}
