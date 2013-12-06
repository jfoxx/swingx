using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public Transform ProjectilePrefab;

	public float reloadTime = 1;
	private float reloadTimer;
	private bool canShoot = false;

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
		if(!networkView.isMine){return;}
		Network.Instantiate(ProjectilePrefab, aimTransform.position, aimTransform.rotation, 0);
		reloadTimer = 0;
	}
}
