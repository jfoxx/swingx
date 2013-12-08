using UnityEngine;
using System.Collections;

public class PlayerControll_mp : MonoBehaviour
{
	
	public Transform aimTransform;
	public Transform groundScannerTransform;

	public Transform grapplePointPrefab;
	public Transform grappleLinePrefab;

	public Transform grappleLineObject;

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
	public float walkFriction = 1.3f;

	Vector2 grapplePosition;
	bool grappleSet = false;

	public float grappleTime = 30;
	float grappleTimer;

	public float maxGrappleLength = 1;
	public float minGrappleLength = 20;

	NetworkManager networkManager; 

	void Start ()
	{
		if(!networkView.isMine){
			rigidbody2D.Sleep();
			rigidbody2D.isKinematic = true;
			return;
		}

		spring = GetComponent<SpringJoint2D> ();
		spring.enabled = false;

		grapplePosition = transform.position;
		audioSource = GetComponent<AudioSource> ();

		Transform grapplePointObject = Instantiate(grapplePointPrefab, transform.position, Quaternion.identity) as Transform;

		spring.connectedBody = grapplePointObject.rigidbody2D;
		spring.connectedBody.transform.parent = transform;


		grappleLineObject = transform.FindChild("Lightning Emitter");
		grappleLineObject.GetComponent<LightningBolt>().target = grapplePointObject.transform;
		

	}

	void Update ()
	{

		if(!networkView.isMine){return;}

		if(spring.connectedBody == null)
		{
			Transform grapplePointObject = Instantiate(grapplePointPrefab, transform.position, Quaternion.identity) as Transform;
			spring.connectedBody = grapplePointObject.rigidbody2D;
			spring.connectedBody.transform.parent = transform;
			grappleLineObject.GetComponent<GrappleLine>().grapple = grapplePointObject;
			grappleSet = false;
		}

//		spring.distance += scroll;
	
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

			RaycastHit2D myhit = Physics2D.Raycast (aimTransform.position, aimTransform.TransformDirection(Vector2.right), 1000f);


			if (myhit != null && myhit.transform != null) {


				if(myhit.transform.CompareTag("Grabable")){

					Debug.Log ("grabbed " + myhit.transform.name);
					audioSource.PlayOneShot (grappleHit);
					grapplePosition = myhit.point;

					spring.connectedBody.transform.position = myhit.point;
					spring.connectedBody.transform.parent = myhit.transform;

					myhit.transform.SendMessage ("onHit", SendMessageOptions.DontRequireReceiver);

					grappleSet = true;
					spring.enabled = true;
				}

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
			spring.connectedBody.transform.position = grapplePosition;
			spring.enabled = false;
		}

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
			return Input.GetButtonDown ("Fire2");
		} 
	}
	
	public virtual bool grappleStay {
		get {
			return Input.GetButton ("Fire2");
		} 
	}
	
	public virtual bool grappleExit {
		get {
			return Input.GetButtonUp ("Fire2");
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
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y  + jumpForce);
			jumping = false;
			
		}
		
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.relativeVelocity.magnitude > 20) 
		{
			audioSource.PlayOneShot (bumpSound);
		}

	}

	void OnDestroy()
	{
		if(networkView.isMine){
			NetworkManager.Instance.playerDied();
		}
	}

}
