using UnityEngine;
using System.Collections;

public class BodyAnimation : MonoBehaviour {

	Animator anim;
	PlayerControll controller;

	void Start () {
		anim = GetComponent<Animator>();
		anim.SetTrigger("idle");
		anim = GetComponentInChildren<Animator>();
	}
	

	void Update () {
		if(Input.GetButtonDown("Jump"))
		{
			anim.SetTrigger("jump");
		}
		if(Input.GetAxis("Horizontal") != 0){
			anim.SetTrigger("walk");
		}
	}
}
