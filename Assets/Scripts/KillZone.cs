using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{	
	public GameObject target;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if(target != null)
		{
			transform.position = new Vector3(target.transform.position.x, -200, 0);
		}
		else
		{
			target = GameObject.FindGameObjectWithTag("Player");
		}
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		coll.transform.SendMessage ("onKillZone", SendMessageOptions.DontRequireReceiver);
	}

}
