using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour
{

	public Transform aimTransform;
	Rigidbody2D grapplePrefab;

	public bool grounded = false;
	public bool jumping = false;
	public float jumpForce = 8;
	public AudioClip jumpSound;
	public AudioClip shootForceLineSound;
	public float maxSpeed = 20;
	public float maxFreeSpeed = 60;
	public float force = 10;
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
	public Animator anim;

	void Start ()
	{
		anim = GetComponent<Animator>();
		spring = GetComponent<SpringJoint2D>();
		grapplePrefab = transform.FindChild("GrapplePoint").GetComponent<Rigidbody2D>();
		spring.connectedBody = grapplePrefab;
		grapplePosition = transform.position;
		spring.enabled = false;
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if(coll.transform.CompareTag("Player"))
		if(coll.relativeVelocity.magnitude > 30 && coll.transform.rigidbody2D.velocity.magnitude > transform.rigidbody2D.velocity.magnitude){
			Debug.Log("velocity : " + coll.relativeVelocity.magnitude * 0.3f);
			coll.transform.SendMessage ("applyDamage",coll.relativeVelocity.magnitude);
		}
	}

	void Update ()
	{


		if(!networkView.isMine){
			this.rigidbody2D.gravityScale = 0;
			return;
		}

		anim.SetFloat("speed", Mathf.Abs(horizontal));

		RaycastHit2D groundScanner = Physics2D.Raycast (transform.position, -Vector2.up, 1.7f) ;

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

			RaycastHit2D myhit = Physics2D.Raycast (transform.position, -(transform.position - aimTransform.position), 1000f);

			if (myhit != null && myhit.transform != null) {
				Debug.Log("grabbed " + myhit.transform.name);
					grapplePosition = myhit.point;
					myhit.transform.SendMessage ("onHit", SendMessageOptions.DontRequireReceiver);
					grappleSet = true;
					spring.enabled = true;
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
		if(!networkView.isMine){return;}

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
		

		
		if (jumping && grounded) {
			rigidbody2D.velocity = rigidbody2D.velocity + (Vector2.up * jumpForce);
			jumping = false;
			
		}
		
	}
}
