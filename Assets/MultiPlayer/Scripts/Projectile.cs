using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public AudioClip bumpSound;
	public Transform explosionPrefab;
	public float startSpeed = 100;
	AudioSource audioSource;

	private bool isShot = false;

	public virtual bool detonate {
		get {
			return Input.GetButtonUp ("Fire1");
		} 
	}

	void Start () {
		audioSource = GetComponent<AudioSource>();
		rigidbody2D.velocity = transform.TransformDirection(Vector2.right * startSpeed);
		if(!networkView.isMine){return;}
	}

	void Update () {
		if(networkView.isMine){
			if(detonate){
				//Network.Destroy(gameObject);
			}
		}
	}

	void OnDestroy(){
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		if(networkView.isMine){
			Network.RemoveRPCs(networkView.viewID);
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.relativeVelocity.magnitude > 20) {
			audioSource.PlayOneShot (bumpSound);
		}
//
//		if(networkView.isMine){
//
//			if(coll.transform.networkView != null){
//
//				if(!coll.transform.networkView.isMine && coll.transform.CompareTag("Player")){
//					Network.Destroy(gameObject);
//				}
//			}
//		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(!networkView.isMine){return;}
		if(other.transform.networkView != null){
			
			if(!other.transform.networkView.isMine && other.transform.CompareTag("Player")){
				Network.Destroy(gameObject);
			}
		}
	}
}
