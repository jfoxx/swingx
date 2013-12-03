using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{

	public Transform target;
	public GameObject finish;
	Vector3 targetPosition;
	float minDistance = 30;
	float maxDistance = 80;
	float distance;
	float inspectFinishTimer = 0;
	float inspectFinishTime = 2;
	float inspectMapTimer = 0;
	float inspectMapTime = 2;
	bool zoomedIn = false;
	Vector3 startPosition = new Vector3(0,0,-40);
	
	void Start ()
	{
		finish = GameObject.Find ("Finish");
		if(finish != null){
			targetPosition = finish.transform.position;
		}
		else
		{
			targetPosition = startPosition;
		}
		distance = maxDistance;
	}
	
	public virtual bool zoom {
		get {
			return Input.GetButtonDown ("zoom");
		} 
	}
	void Update()
	{

		if (zoom) {
			zoomedIn = !zoomedIn;
		}

		if(zoomedIn){
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
				targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y +10, -distance);
			} else {
				targetPosition = new Vector3 (finish.transform.position.x, finish.transform.position.y, -40);
			}			
		}

		if (targetPosition != transform.position) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, 0.1f);
		}

	}
}
