using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public AudioClip shootSound;

	public Transform ProjectilePrefab;

	public float reloadTime = 1;
	private float reloadTimer;
	private bool canShoot = false;
	AudioSource audio;
	private Transform aimTransform;

	public virtual bool fire {
		get {
			return Input.GetButtonDown ("Fire1");
		} 
	}

	void Start () { 
		if(!networkView.isMine){return;}
		canShoot = true;
		reloadTimer = reloadTime;
		aimTransform = transform.Find("Head").transform.Find("Aim");
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!networkView.isMine){return;}
		canShoot = reloadTimer >= reloadTime;

		if(reloadTimer < reloadTime){
			reloadTimer += Time.deltaTime;

		}

		if(fire && canShoot){
			shoot();
		}
	}

	void shoot(){
		audio.PlayOneShot(shootSound);
		if(!networkView.isMine){return;}
		Network.Instantiate(ProjectilePrefab, aimTransform.position, aimTransform.rotation, 0);
		reloadTimer = 0;
	}
}
