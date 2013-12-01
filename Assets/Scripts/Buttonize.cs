using UnityEngine;
using System.Collections;

public class Buttonize : MonoBehaviour {

	bool hovering = false;
	Vector3 normalSize;
	Vector3 hoverSize;

	GameState gameState;

	public string level;

	void Start(){

		gameState = GameState.Instance;

		normalSize = transform.localScale;
		hoverSize = transform.localScale * 1.1f;
	}

	void Update () {
	
		if(hovering){
			transform.localScale = Vector3.Lerp(transform.localScale, hoverSize, 0.1f);
		}else {
			transform.localScale = Vector3.Lerp(transform.localScale, normalSize, 0.3f);
		}

	}

	void OnMouseDown(){
		Debug.Log("Ok You ARE talking to me.");
		
	}

	void OnMouseUp(){
		Debug.Log("done talking...");
		if(level != null){
			gameState.setLevel(level);
		}else{
			//sayWHAAAT?
		}
	}

	void OnMouseOver(){
		Debug.Log("What you want?");
		hovering = true;

	}
	void OnMouseExit(){
		Debug.Log("What you want?");
		hovering = false;
	}
}
