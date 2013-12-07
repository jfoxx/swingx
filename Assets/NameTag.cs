using UnityEngine;
using System.Collections;

public class NameTag : MonoBehaviour {

	string playerName = "Player";
	Health_mp health;
	TextMesh text;

	void Start () {
		playerName = transform.parent.name;
		text = GetComponent<TextMesh>();
		health = transform.parent.GetComponent<Health_mp>();
	}

	void Update(){
		text.text = playerName + " | + " + health.health;
	}
}
