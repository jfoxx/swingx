using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour {

	private AudioSource audioSource; 

	public AudioClip checkpointReached;
	public AudioClip allCheckpointsReached;

	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	

	void Update () {
		
	}

	public void OnCheckpointReached(){
		Debug.Log("OnCheckpointReached()");
		audioSource.PlayOneShot(checkpointReached);

	}

	public void OnAllCheckpointsReached(){
		Debug.Log("OnAllCheckpointsReached()");
		audioSource.PlayOneShot(allCheckpointsReached);
	}
}