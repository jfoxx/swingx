using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof (Health))]
[RequireComponent (typeof (AudioSource))]
public class Breakable : MonoBehaviour {
	
	SpriteRenderer ren;
	AudioSource audioSource;
	public AudioClip crackSound;
	Health health;

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		ren = GetComponent<SpriteRenderer>();
		health = GetComponent<Health>();
	}

	void Update () {
		ren.color = new Color(ren.color.r, ren.color.g, ren.color.b, health.health / 100);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.relativeVelocity.magnitude > 70){
			Debug.Log(coll.relativeVelocity.magnitude);
			audioSource.PlayOneShot(crackSound);
			transform.SendMessage("applyDamage", coll.relativeVelocity.magnitude * 10, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnDestroy()
	{
		audioSource.PlayOneShot(crackSound);

	}
}
