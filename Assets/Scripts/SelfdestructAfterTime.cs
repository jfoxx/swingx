using UnityEngine;
using System.Collections;

public class SelfdestructAfterTime : MonoBehaviour {
	public float selfdestructTime = 1;
	float timer;
	void Start () {
		timer = selfdestructTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer > 0){
			timer -= Time.deltaTime;
		}
		if(timer <= 0){
			Destroy(gameObject);
		}
	}
}
