using UnityEngine;
using System.Collections;

public class HUD_mp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!networkView.isMine){enabled = false;}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.Label(new Rect(Screen.width - 150, 30, 100, 30), "Velocity " + Mathf.CeilToInt(rigidbody2D.velocity.magnitude));
	}
}
