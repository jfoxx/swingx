using UnityEngine;
using System.Collections;

public class GrappleLine : MonoBehaviour
{


	public float distance;
	public GameObject player;
	public GameObject grapple;
	public LineRenderer line;

	void Start ()
	{
		player = GameObject.Find ("Player");
		grapple = GameObject.Find ("GrapplePoint");
		line = GetComponent<LineRenderer> ();
		line.SetWidth (0.05f, 0.05f);

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