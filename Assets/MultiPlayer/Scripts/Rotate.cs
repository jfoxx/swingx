using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float rotationSepeed = 5;

	void FixedUpdate () {
		transform.Rotate(0,0,rotationSepeed);
	}
}
