using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;
	Vector3 targetPosition;
	public float minDistance;
	public float maxDistance;
	public float distance;
	public float scrollSpeed = 100;
	
	
	void Start () 
	{
		targetPosition = transform.position;
		distance = maxDistance;
	}
	
	public virtual float scroll 
	{
		get {
			return Input.GetAxis ("Scroll");
		} 
	}
	
	void Update() 
	{
		distance = Mathf.Lerp(distance, distance + ( - scroll * scrollSpeed),0.5f);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
	}
	
	
	void FixedUpdate () 
	{
		if (target != null) 
		{
			
			targetPosition = new Vector3(target.transform.position.x, target.transform.position.y , -distance);
			//transform.LookAt(target.transform);
		}
		else
		{
			Debug.Log("Player is null in LookAt");
		}
		if(targetPosition != transform.position)
		{
			transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
		}
		
		
	}
}
