using UnityEngine;
using System.Collections;

public class Health_mp : MonoBehaviour
{

	public Transform explosionPrefab;
	public float health;
	public float max_health = 100;
	
	
	// Use this for initialization
	void Start ()
	{
		health = max_health;
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
