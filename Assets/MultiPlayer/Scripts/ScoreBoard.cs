using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour
{
	
	GameState state;
	PlayerManager_mp playerManager;
	private float top;
	private float left;
	private float height;
	private float width;
	private Rect menuWindowRect;
	private bool showScore = false;
	public GUISkin skin;

	public virtual bool score {
		get {
			return Input.GetButtonDown ("Score");
		}
	}
	
	void Start ()
	{
		state = GameState.Instance;
		playerManager = GetComponent<PlayerManager_mp> ();
	}
	
	void Update ()
	{
		if (score) {
			toggleScore ();
		}
	}

	void toggleScore ()
	{
		showScore = !showScore;
	}
	
	void OnGUI ()
	{
		if (!showScore) {
			return;
		}

		GUI.skin = skin;

		height = Screen.height / 3;
		width = 300;
		top = (Screen.height / 2) - (height / 2);
		left = (Screen.width / 2) - (width / 2);

		menuWindowRect = new Rect (left, top, width, height);
		
		menuWindowRect = GUI.Window ((int)WindowID.Score, menuWindowRect, windowFunction, "Scores");
	}
	
	void windowFunction (int windowID)
	{
		GUILayout.BeginHorizontal ();

		GUILayout.BeginVertical ();
		GUILayout.Label ("Player");

		foreach (Player player in playerManager.players) 
		{
			GUILayout.Label (player.name);
			GUILayout.Space (5);
		}
		GUILayout.EndVertical ();

		GUILayout.BeginVertical ();
		GUILayout.Label ("Score");
		foreach (Player player in playerManager.players) 
		{
			GUILayout.Label (player.score + "");
			GUILayout.Space (5);
		}
		GUILayout.EndVertical ();

		GUILayout.BeginVertical ();
		GUILayout.Label ("Ping");
		foreach (Player player in playerManager.players) 
		{
			if (player.networkPlayer == Network.player) 
			{
				GUILayout.Label ("");
			} else {
				GUILayout.Label (Network.GetLastPing (player.networkPlayer) + "");
			}
			GUILayout.Space (5);
		}
		GUILayout.EndVertical ();

		GUILayout.EndHorizontal ();
	}
}

