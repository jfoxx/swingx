﻿using UnityEngine;
using System.Collections;

public class Aim_mp : MonoBehaviour
{

	public float raycastDist = 10;
	public float sensitivity = 6f;
	GameState gameState;

	public virtual float vertical {
		get {
			return Input.GetAxis ("Vertical");
		} 
	}
	
	public virtual float horizontal {
		get {
			return Input.GetAxisRaw ("Horizontal");
		} 
	}

	void Start ()
	{
		if(!networkView.isMine){enabled = false;}

		gameState = GameState.Instance;
		transform.eulerAngles = new Vector3 (0, 0, 15f);
	}

	void Update ()
	{        
		if(!networkView.isMine){return;}

		if (gameState.mouseAim) {

			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (
				new Vector3 (
					Input.mousePosition.x,
					Input.mousePosition.y,
					Input.mousePosition.z - Camera.main.transform.position.z
				)
			);
			
			//Rotates toward the mouse
			transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 ((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);

			if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) {
				transform.localScale = new Vector3(transform.localScale.x, -1 ,transform.localScale.z);
			} else {
				transform.localScale = new Vector3(transform.localScale.x, 1 ,transform.localScale.z);
				
			}

			
		} else {
				
			if (vertical != 0) {        
				transform.Rotate (0, 0, vertical * sensitivity * 30 * Time.deltaTime);
			}
			
			if (horizontal < 0) {
				if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) {
					transform.eulerAngles = new Vector3 (0, 180, 360 - transform.localEulerAngles.z + 180);
				} else {
					transform.eulerAngles = new Vector3 (0, 180, transform.localEulerAngles.z);
					
				}
			}
		
			if (horizontal > 0) {
				if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) {
					transform.eulerAngles = new Vector3 (0, 0, 360 - transform.localEulerAngles.z + 180);
				} else {
					transform.eulerAngles = new Vector3 (0, 0, transform.localEulerAngles.z);
				} 
			}
		}
	}

	void OnDestroy(){
		if(networkView.isMine){
			Network.RemoveRPCs(networkView.viewID);
		}
	}

}
