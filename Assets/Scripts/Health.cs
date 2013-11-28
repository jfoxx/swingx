using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{

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

	void applyDamage (float damage)
	{
		Debug.Log ("damage taken: " + damage);
		health -= damage;
	}
	
	void onKillZone ()
	{
		die ();
	}
	
	void die ()
	{
		Debug.Log ("i died!");
		Destroy (gameObject);
	}
	
	void OnDestroy ()
	{
		Debug.Log ("i was destroyed");        
	}
}
