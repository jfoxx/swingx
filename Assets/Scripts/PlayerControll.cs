using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour
{

	public AudioClip grappleHit;
	public AudioClip grappleMiss;
	public AudioClip bumpSound;
	public Transform aimTransform;
	public bool grounded = false;
	public bool jumping = false;
	public float jumpForce = 8;
	public float maxSpeed = 20;
	public float maxFreeSpeed = 60;
	public float force = 8;
	public float airControll = 0.3f;
	public float controll;
	public float walkFriction = 1.5f;
	public int state = 0;
	public float jumpLimit = 0;
	public SpringJoint2D spring;
	public Vector2 grapplePosition;
	public float grappleTime = 30;
	public float grappleTimer;
	public bool grappleSet = false;
	public float maxGrappleLength = 1;
	public float minGrappleLength = 20;
	private AudioSource audioSource;

	void Start ()
	{
		spring = GetComponent<SpringJoint2D> ();
		grapplePosition = transform.position;
		spring.enabled = false;
		audioSource = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		spring.distance += scroll;


		RaycastHit2D groundScanner = Physics2D.Raycast (transform.position, -Vector2.up, 1.7f);
		if (groundScanner.transform != null) {
			if (!groundScanner.transform.CompareTag ("Checkpoint")) {
				grounded = true;
			} else {
				grounded = false;
			}
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

		Debug.DrawRay (transform.position, -(transform.position - aimTransform.position), Color.green);


		if (grappleStart) {
			RaycastHit2D myhit = Physics2D.Raycast (transform.position, - (transform.position - aimTransform.position), 1000f);
			if (myhit != null && myhit.transform != null) {
				Debug.Log ("grabbed " + myhit.transform.name);
				audioSource.PlayOneShot (grappleHit);
				grapplePosition = myhit.point;
				myhit.transform.SendMessage ("onHit", SendMessageOptions.DontRequireReceiver);
				grappleSet = true;
				spring.enabled = true;
			} else {
				audioSource.PlayOneShot (grappleMiss);
			}
		}
		
		if (grappleExit || grappleTimer <= 0) {
			grappleSet = false;
			grappleTimer = grappleTime;

			
		}
		
		if (grappleStay && grappleSet) {
			grappleTimer -= Time.deltaTime;
			Debug.DrawLine (transform.position, spring.connectedBody.transform.position, Color.red);
		}
		
		if (!grappleSet) {
			grapplePosition = transform.position;
			spring.enabled = false;
			spring.distance = 1;
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
