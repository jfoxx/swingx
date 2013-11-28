using UnityEngine;
using System.Collections;

public class GrappleLine : MonoBehaviour
{


	public float distance;
	public Transform player;
	public Transform grapple;
	public LineRenderer line;

	void Start ()
	{

		player = transform.parent;
		grapple = player.FindChild("GrapplePoint");
		line = GetComponent<LineRenderer> ();
		line.SetWidth (0.2f, 0.2f);

	}

	void Update ()
	{

		if (grapple != null && player != null) {

			distance = Vector2.Distance (grapple.position, player.position);
			line.SetPosition (0, player.position);
			line.SetPosition (1, grapple.position);

		}
	}
}