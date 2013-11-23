using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour
{

	public Transform aimTransform;
	public bool grounded = false;
	public bool jumping = false;
	public float jumpForce = 8;
	public AudioClip jumpSound;
	public AudioClip shootForceLineSound;
	public float maxSpeed = 20;
	public float maxFreeSpeed = 60;
	public float force = 8;
	public float airControll = 0.3f;
	public float controll;
	public float walkFriction = 1.5f;
	public int state = 0;
	public float jumpLimit = 0;
	public Vector2 grapplePosition;
	public SpringJoint2D spring;
	public float grappleTime = 30;
	public float grappleTimer;
	public bool grappleSet = false;

	void Start ()
	{
		spring = GetComponent<SpringJoint2D>();
		grapplePosition = transform.position;
		spring.enabled = false;
	}

	void Update ()
	{

		RaycastHit2D groundScanner = Physics2D.Raycast (transform.position, -Vector2.up, 1f) ;

		grounded = groundScanner.transform != null;

		if (jump && grounded) {
			jumping = true;
		}

		if (!grounded) {
			controll = airControll;
		} else {
			controll = 1;
		}

		Debug.DrawRay(transform.position, -(transform.position - aimTransform.position), Color.green);

		if (grappleStart) {

			RaycastHit2D myhit = Physics2D.Raycast (transform.position, -(transform.position - aimTransform.position), 10000f);

			if (myhit != null && myhit.transform != null) {
				Debug.Log(myhit.transform.name);
				//if (myhit.transform.CompareTag ("Checkpoint")) {
					grapplePosition = myhit.point;
					Debug.Log("yepp");
					myhit.transform.SendMessage ("onHit", SendMessageOptions.DontRequireReceiver);
					grappleSet = true;
					spring.enabled = true;
//				}
			}
		}
		
		if (grappleExit || grappleTimer <= 0) {
			grappleSet = false;
			grappleTimer = grappleTime;
			
		}
		
		if (grappleStay && grappleSet) {
			
			//grapplePosition = hit.transform.position;
			grappleTimer -= Time.deltaTime;
			Debug.DrawLine (transform.position, spring.connectedBody.transform.position, Color.red);
			//spring.connectedBody.transform.position = transform.position;
			
		}
		
		if (!grappleSet) {
			grapplePosition = transform.position;
			spring.enabled = false;
			
		}
		spring.connectedBody.transform.position = grapplePosition;

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
		
		if (rigidbody2D.velocity.x < maxSpeed * controll && !grounded && horizontal != 0) {
			rigidbody2D.AddForce (Vector2.right * horizontal * controll * force);
			
		}
		
		if (rigidbody2D.velocity.magnitude > maxFreeSpeed) {
			rigidbody2D.velocity = Vector3.ClampMagnitude (rigidbody2D.velocity, maxFreeSpeed);
		}
		
		if (jumping && grounded) {
			//audio.clip = jumpSound;
			//audio.Play();
			rigidbody2D.velocity = rigidbody2D.velocity + (Vector2.up * jumpForce);
			jumping = false;
			
		}
		
	}
}
