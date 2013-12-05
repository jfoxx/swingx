using UnityEngine;
using System.Collections;

public class Follow_mp : MonoBehaviour
{

	public Transform target;
	public GameObject finish;
	Vector3 targetPosition;
	float minDistance = 40;
	float maxDistance = 80;
	float distance;
	float overviewTimer = 0;
	float overviewTime = 2;

	bool zoomedIn = false;
	Vector3 startPosition = new Vector3(0,0,-80);
	
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
			distance = Mathf.Lerp (distance, maxDistance, 0.4f);
		} else {
			distance = Mathf.Lerp (distance, minDistance, 0.4f);
		}

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
					targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y +10, -distance);
				}
			}		
		}

		if (targetPosition != transform.position) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, 0.3f);
		}

	}
}
