using UnityEngine;
using System.Collections;

public class Health_mp : MonoBehaviour
{

	public Transform explosionPrefab;
	public float health;
	public float max_health = 100;
	
	GameObject manager;

	void Start ()
	{
		health = max_health;

		manager = GameObject.Find ("Manager");
		manager.networkView.RPC ("updatePlayerHealth", RPCMode.All, networkView.owner, health);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health <= 0) {
			health = 0;
			die ();
		}
	}

	[RPC]
	void applyDamage (float damage)
	{
		Debug.Log ("damage taken: " + damage);

		health -= damage;

		networkView.RPC("updatePlayerHealth", RPCMode.Others, health);

		manager.networkView.RPC ("updatePlayerHealth", RPCMode.All, networkView.owner, health);
	}

	[RPC]
	void updatePlayerHealth(float pHealth){
		health = pHealth;
	}

	void onKillZone ()
	{
		health = 0;
	}
	
	void die ()
	{
		Debug.Log ("i died!");

		if(explosionPrefab != null){
			Network.Instantiate(explosionPrefab, transform.position, Quaternion.identity,0);
		}

		Network.RemoveRPCs(networkView.viewID);
		Network.Destroy(gameObject);
	}
	
	void OnDestroy ()
	{
		Debug.Log ("i was destroyed");        
	}

	void OnGUI(){
		if(!networkView.isMine){return;}
		GUI.Label(new Rect(Screen.width-100, Screen.height - 30, 100, 30), "healt: " + health);
	}
}
