﻿using UnityEngine;
using System.Collections;

public class GrappleLine : MonoBehaviour
{


	public float distance;
	public GameObject player;
	public GameObject grapple;
	public LineRenderer line;

	void Start ()
	{
		player = GameObject.Find ("Player_mp");
		grapple = GameObject.Find ("GrapplePoint_mp");
		line = GetComponent<LineRenderer> ();
		line.SetWidth (0.2f, 0.2f);

	}

	void Update ()
	{
		if (grapple != null && player != null) {

			distance = Vector2.Distance (grapple.transform.position, player.transform.position);
			line.SetPosition (0, player.transform.position);
			line.SetPosition (1, grapple.transform.position);

		}
	}
}