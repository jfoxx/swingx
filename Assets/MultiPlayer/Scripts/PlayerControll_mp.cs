using UnityEngine;
using System.Collections;

public class PlayerControll_mp : MonoBehaviour
{
	
	public Transform aimTransform;
	public Transform groundScannerTransform;
	private AudioSource audioSource;
	public SpringJoint2D spring;
	
	public AudioClip grappleHit;
	public AudioClip grappleMiss;
	public AudioClip bumpSound;

	public bool grounded = false;
	public bool jumping = false;

	public float jumpForce = 8;
	public float force = 8;

	public float maxSpeed = 20;
	public float maxFreeSpeed = 60;

	public float controll;
	public float airControll = 0.3f;
	public float walkFriction = 1.5f;

	Vector2 grapplePosition;
	bool grappleSet = false;

	public float grappleTime = 30;
	float grappleTimer;

	public float maxGrappleLength = 1;
	public float minGrappleLength = 20;

	void Start ()
	{
		spring = GetComponent<SpringJoint2D> ();
		grapplePosition = transform.position;
		spring.enabled = false;
		audioSource = GetComponent<AudioSource> ();
		spring.connectedBody.transform.parent = transform;

		if(!networkView.isMine){enabled = false;}
		if(!networkView.isMine){return;}

//		if(!networkView.isMine){
//			if(!networkView.isMine){enabled = false;}
//
//		}
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if(!networkView.isMine){return;}

		if(coll.transform.CompareTag("Player")){
			if(coll.relativeVelocity.magnitude > 50 && coll.transform.rigidbody2D.velocity.magnitude > transform.rigidbody2D.velocity.magnitude){
				Debug.Log("velocity : " + coll.relativeVelocity.magnitude * 0.3f);
				coll.transform.SendMessage ("applyDamage",coll.relativeVelocity.magnitude);
			}
		}
	}

	void Update ()
	{

		if(!networkView.isMine){return;}

		spring.distance += scroll;
	
		RaycastHit2D groundScanner = Physics2D.Raycast (groundScannerTransform.transform.position, -Vector2.up, 0.2f);

		if (groundScanner.transform != null) {
			grounded = !groundScanner.transform.CompareTag ("Checkpoint");
		} else {
			grounded = false;
		}
		
		if (jump && grounded) {
			jumping = true;
		}
	
		if (!grounded) {
			controll = airControll;
		} else {
			controll = 1;
		}

		Debug.DrawRay (aimTransform.position, aimTransform.TransformDirection(Vector2.right), Color.green);

		if (grappleStart) {

			RaycastHit2D myhit = Physics2D.Raycast (aimTransform.position, aimTransform.TransformDirection(Vector2.right), 100f);

			Debug.Log(myhit.transform.name);

			if (myhit != null && myhit.transform != null) {

				//if(myhit.transform.CompareTag("Grabable")){

					Debug.Log ("grabbed " + myhit.transform.name);
					audioSource.PlayOneShot (grappleHit);
					grapplePosition = myhit.point;

					spring.connectedBody.transform.position = myhit.point;
					spring.connectedBody.transform.parent = myhit.transform;

					myhit.transform.SendMessage ("onHit", SendMessageOptions.DontRequireReceiver);

					grappleSet = true;
					spring.enabled = true;
				//}

			} else {
				audioSource.PlayOneShot (grappleMiss);
			}
		}
		
		if (grappleExit) {
			grappleSet = false;
			spring.connectedBody.transform.parent = transform;
		}

		if (!grappleSet) {
			grapplePosition = transform.position;
			spring.enabled = false;
		}

		if(!spring.connectedBody.gameObject.activeInHierarchy){
			spring.connectedBody.transform.parent = transform;
			spring.connectedBody.gameObject.SetActive(true);
			grappleSet = false;

		}

		spring.connectedBody.transform.position = grapplePosition;
	}
	
	public virtual float scroll {
		get {
			return Input.GetAxis ("Scroll");
		} 
	}
	
	public virtual bool jump {
		get {
			return Input.GetButton ("Jump");
		}
	}
	
	public virtual float horizontal {
		get {
			return Input.GetAxisRaw ("Horizontal");
		} 
	}
	
	public virtual bool grappleStart {
		get {
			return Input.GetButtonDown ("Fire1");
		} 
	}
	
	public virtual bool grappleStay {
		get {
			return Input.GetButton ("Fire1");
		} 
	}
	
	public virtual bool grappleExit {
		get {
			return Input.GetButtonUp ("Fire1");
		} 
	}
	
	void FixedUpdate ()
	{
		if(!networkView.isMine){return;}

		if (horizontal == 0 && grounded) {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x / walkFriction, rigidbody2D.velocity.y);
		}
		
		// If the object is grounded and isn't moving at the max speed or higher apply force to move it
		if (rigidbody2D.velocity.magnitude < maxSpeed && grounded && horizontal != 0) {
			rigidbody2D.AddForce (Vector2.right * horizontal * force * controll);
		}

		// moving right
		if (rigidbody2D.velocity.x <= 0) {
			if (rigidbody2D.velocity.x < maxSpeed * controll && !grounded && horizontal != 0) {
				rigidbody2D.AddForce (Vector2.right * horizontal * controll * force);
			}
		}

		// moving left
		if (rigidbody2D.velocity.x >= 0) {
			if (rigidbody2D.velocity.x > -maxSpeed * controll && !grounded && horizontal != 0) {
				rigidbody2D.AddForce (Vector2.right * horizontal * controll * force);
			}
		} 
		
		if (jumping && grounded) {
			//audio.clip = jumpSound;
			//audio.Play();
			rigidbody2D.velocity = rigidbody2D.velocity + (Vector2.up * jumpForce);
			jumping = false;
			
		}
		
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.relativeVelocity.magnitude > 20) {
			audioSource.PlayOneShot (bumpSound);
		}
	}

}
