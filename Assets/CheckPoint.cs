using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{

	public SpriteRenderer myRenderer;
	public bool isChecked;

	void Start ()
	{
		myRenderer = GetComponent<SpriteRenderer> ();
		if (myRenderer == null) {
			Debug.Log ("no sprite renderer found");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void onHit ()
	{
		Debug.Log ("i was hit");
		isChecked = true;
		if(myRenderer != null){
			myRenderer.color = Color.green;
		}
	}
}
