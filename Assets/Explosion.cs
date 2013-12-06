using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	float radius = 10f;
	void Start () {
		Collider2D [] collissions = Physics2D.OverlapCircleAll(transform.position, radius);
		Debug.Log ("CircleOverap");
		foreach(Collider2D coll in collissions){
			if(coll.rigidbody2D != null){
				Debug.Log ("CircleOverap " + coll.transform.name);

				Vector2 force = coll.transform.TransformDirection(transform.position);

				coll.rigidbody2D.velocity = force * 10;

				if(coll.networkView.isMine){
					coll.transform.SendMessage("applyDamage",  20);
				}else{
					coll.networkView.RPC ("applyDamage", coll.networkView.owner, 20);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
