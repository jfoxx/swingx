using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float damage = 30f;
	public float radius = 10f;
	public NetworkPlayer creator;

	void Start () 
	{
		Collider2D [] collissions = Physics2D.OverlapCircleAll(transform.position, radius);

		Debug.Log ("CircleOverap");

		foreach(Collider2D coll in collissions){

			if(coll.rigidbody2D != null){

				if(coll.transform.CompareTag("Player")){

					if(!coll.networkView.isMine)
					{
						coll.networkView.RPC ("applyDamage", coll.networkView.owner, damage, creator);
					}

					Vector2 force = coll.transform.position - transform.position;
					
					coll.rigidbody2D.velocity = (force * 50);
				}
			}
		}
	}
}
