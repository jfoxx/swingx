﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public AudioClip bumpSound;
	public Transform explosionPrefab;
	public float startSpeed = 100;
	AudioSource audioSource;

	private bool isShot = false;

	void Start () {
		rigidbody2D.velocity = transform.TransformDirection(Vector2.right * startSpeed);
		audioSource = GetComponent<AudioSource>();
	}

	void Update () {

	}

	void OnDestroy(){
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		Debug.Log("Collision!!!!!!!!!!");
		if(coll.transform.CompareTag("Player")){
			Network.Destroy(gameObject);
		}

		if (coll.relativeVelocity.magnitude > 20) {
			audioSource.PlayOneShot (bumpSound);
		}

	}
}
