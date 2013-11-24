using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{

	public SpriteRenderer myRenderer;
	public bool isChecked;
	public GameManager gameManager;

	void Start ()
	{
		myRenderer = GetComponent<SpriteRenderer> ();
		if (myRenderer == null) {
			Debug.Log ("no sprite renderer found");
		}
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> (); 
//		gameManager = GameManager.Instance; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

//	void onHit ()
//	{
//		Debug.Log ("i was Grabbed!!");
//		if (!isChecked) {
//			isChecked = true;
//			gameManager.SendMessage ("checkpointReached");
//			if (myRenderer != null) {
//				myRenderer.color = Color.green;
//			}
//		}
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.transform.CompareTag("Player")){
			if (!isChecked) {
				isChecked = true;
				gameManager.SendMessage ("checkpointReached");
				if (myRenderer != null) {
					myRenderer.color = Color.green;
				}
			}
		}
	}
}
