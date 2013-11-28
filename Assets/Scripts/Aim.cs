using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour
{

	public float raycastDist = 10;
	public float sensitivity = 6f;
	
	public virtual float vertical {
		get {
			return Input.GetAxis ("Vertical");
		} 
	}
	
	public virtual float horizontal {
		get {
			return Input.GetAxisRaw ("Horizontal");
		} 
	}


	void Start ()
	{
		transform.eulerAngles = new Vector3(0, 0, 15f);
	}
	
	void Update ()
	{        
		
		if (vertical != 0) 
		{        
			transform.Rotate (0, 0, vertical * sensitivity * 30 * Time.deltaTime);
		}
		
		if (horizontal < 0) 
		{
			if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
				transform.eulerAngles = new Vector3 (0, 180, 360 - transform.localEulerAngles.z + 180);
			} else{
				transform.eulerAngles = new Vector3 (0, 180, transform.localEulerAngles.z);
				
			}
		}
		
		if (horizontal > 0) 
		{
			if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270){
				transform.eulerAngles = new Vector3 (0, 0, 360 - transform.localEulerAngles.z + 180);
			} else{
				transform.eulerAngles = new Vector3 (0, 0, transform.localEulerAngles.z);
			} 
		}
		
		//transform.eulerAngles = new Vector3 (0, transform.localEulerAngles.y, Mathf.Clamp (transform.localEulerAngles.z, 90, 270));
		//                Debug.Log(transform.eulerAngles);
	}

}
