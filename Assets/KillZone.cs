using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{	
	void OnCollisionStay2D (Collision2D coll)
	{
		coll.transform.SendMessage ("onKillZone", SendMessageOptions.DontRequireReceiver);
	}

}
