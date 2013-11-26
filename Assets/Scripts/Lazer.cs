using UnityEngine;
using System.Collections;

public class Lazer : MonoBehaviour {

	public float distance;
	public GameObject player;

	public LineRenderer line;
	Vector2 target;

	
	void Start ()
	{
		player = GameObject.Find ("Player");

		line = GetComponent<LineRenderer> ();
		line.SetWidth (0.1f, 0.1f);
	}
	
	void Update ()
	{

		RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.TransformDirection(Vector2.right), 1000f);
		if(hit.transform != null){
			target = hit.point;
		}else{
			target = transform.TransformPoint(Vector2.right * 10000);
		}

		line.SetPosition (0, transform.position);
		line.SetPosition (1, target);
	}
}
