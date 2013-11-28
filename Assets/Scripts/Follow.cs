using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{

	public Transform target;
	public GameObject finish;
	Vector3 targetPosition;
	public float minDistance = 5;
	public float maxDistance = 60;
	public float distance;
	public float scrollSpeed = 100;
	public float inspectFinishTimer = 0;
	public float inspectFinishTime = 3;
	
	void Start ()
	{
		finish = GameObject.Find ("Finish");
		targetPosition = finish.transform.position;
		distance = maxDistance;
	}
	
	public virtual bool zoom {
		get {
			return Input.GetButton ("zoom");
		} 
	}
	void Update(){

		if (zoom) {
			distance = Mathf.Lerp (distance, maxDistance, 0.4f);
		} else {
			distance = Mathf.Lerp (distance, minDistance, 0.4f);
		}

	}
	
	void FixedUpdate ()
	{
		
		
		if (inspectFinishTimer < inspectFinishTime) {
			inspectFinishTimer += Time.deltaTime;
		}

		if (target != null) {
			if (inspectFinishTimer > inspectFinishTime) {
				targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y, -distance);
				//transform.LookAt(target.transform);
			} else {
				targetPosition = new Vector3 (finish.transform.position.x, finish.transform.position.y, -20);
			}
			
		} else {
			Debug.Log ("Player is null in LookAt");
		}

		if (targetPosition != transform.position) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, 0.1f);
		}

	}
}
