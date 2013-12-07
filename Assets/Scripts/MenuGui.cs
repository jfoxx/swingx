using UnityEngine;
using System.Collections;

public class MenuGui : MonoBehaviour
{

	GameState state;
	public bool mouseAim = false;
	private float top;
	private float left;
	private float height;
	private float width;
	private Rect menuWindowRect;

	public GUISkin skin;


	void Start ()
	{
		state = GameState.Instance;
	}

	void Update ()
	{
		height = Screen.height / 2;
		width = 300;
		top = (Screen.height / 2) - (height/2);
		left = (Screen.width / 2) - (width/2);
		
		menuWindowRect = new Rect (left, top, width, height);
	}

	void OnGUI ()
	{
		GUI.skin = skin;
		if (!state.showMenu) {
			return;
		}

		menuWindowRect = GUI.Window (1, menuWindowRect, windowFunction, "Menu");
	}

	void windowFunction (int windowID)
	{

		if (GUILayout.Button ("Main Menu")) {
			GameState.Instance.setLevel ("main");
			Application.LoadLevel ("main");
		}


		GUILayout.Space (10);

		string pauseText = state.paused ? "Resume" : "Pause";
		
		if (GUILayout.Button (pauseText)) {
			if (state.paused) {
				state.resume ();
			} else {
				state.pause ();
			}
		}

		GUILayout.Space (10);
		
		state.mouseAim = GUILayout.Toggle (state.mouseAim, "use mouse to aim");

		GUILayout.FlexibleSpace ();

		if(!Application.isEditor && !Application.isWebPlayer){
			if(GUILayout.Button("Quit")){
				Application.Quit();
			}
		}
	}
}
