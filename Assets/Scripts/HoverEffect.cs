using UnityEngine;
using System.Collections;

public class HoverEffect : MonoBehaviour {

	ParticleSystem particles;
	void Start () {
		particles = GetComponent<ParticleSystem>();
		particles.emissionRate = 0;
	}
	
	void Update () {
	
	}

	void OnOver(){
		particles.emissionRate = 100;
	}

	 void OnOut(){
		particles.emissionRate = 10;
	}
}
