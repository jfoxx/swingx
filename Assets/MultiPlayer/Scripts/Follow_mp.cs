using UnityEngine;
using System.Collections;

public class Follow_mp : MonoBehaviour
{

	public Transform target;
	public GameObject finish;
	Vector3 targetPosition;
	float minDistance = 70;
	float maxDistance = 120;
	float distance;
	float overviewTimer = 0;
	float overviewTime = 2;

	bool zoomedIn = true;
	Vector3 startPosition = new Vector3(0,0,-110);
	
	void Start ()
	{
		if(!networkView.isMine){
			gameObject.SetActive(false);
		}

		targetPosition = startPosition;

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
			distance = Mathf.Lerp (distance, maxDistance, 0.2f);
		} else {
			distance = Mathf.Lerp (distance, minDistance, 0.2f);
		}

		//distance = Mathf.Lerp (distance, Mathf.Clamp(target.rigidbody2D.velocity.magnitude * 1.3f, minDistance, maxDistance), 0.01f);;

	}
	
	void FixedUpdate ()
	{

		if (overviewTimer < overviewTime) {
			overviewTimer += Time.deltaTime;
		}

		if (target != null) {
			if (overviewTimer > overviewTime) {
				if(target.gameObject == null){
					targetPosition = startPosition;
				}else{
					targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y +20, -distance);
				}
			}		
		}

		if (targetPosition != transform.position) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, 0.3f);
		}

	}
}
