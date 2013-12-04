using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof (Health_mp))]
[RequireComponent (typeof (AudioSource))]
public class Breakable_mp : MonoBehaviour {
	
	SpriteRenderer ren;
	AudioSource audioSource;
	public AudioClip crackSound;
	Health_mp health;

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		ren = GetComponent<SpriteRenderer>();
		health = GetComponent<Health_mp>();
	}

	void Update () {
		ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, health.health / 100);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.relativeVelocity.magnitude > 70){
			Debug.Log(coll.relativeVelocity.magnitude);
			networkView.RPC("OnCollision", RPCMode.All, coll.relativeVelocity.magnitude * 0.6f);
		}
	}

	[RPC]
	void OnCollision(float damage){
		audioSource.PlayOneShot(crackSound);
		networkView.RPC("applyDamage", RPCMode.AllBuffered , damage);
	}

	void OnDestroy()
	{
		audioSource.PlayOneShot(crackSound);

	}
}
