using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
	
	public GameObject PlayerPrefab;
	public GameObject CameraPrefab;
	public string gameTypeName;
	private int playerCount = 0;
	bool isRefreshing = false;
	float refreshRequestLength = 5f;
	HostData[] hostData;
	
	void Start ()
	{
		
		gameTypeName = "swingX_" + Application.unityVersion + "_server";
		Debug.Log (gameTypeName);
		
	}
	
	public IEnumerator RefreshHostList ()
	{
		
		Debug.Log ("Refreshing host...");
		MasterServer.RequestHostList (gameTypeName);
		
		float timeStarted = Time.time;
		float timeEnd = Time.time + refreshRequestLength;
		
		while (Time.time < timeEnd) {
			hostData = MasterServer.PollHostList ();
			yield return new WaitForEndOfFrame ();
		}
		
		if (hostData == null || hostData.Length == 0) {
			Debug.Log ("No active servers found.");
		} else {
			Debug.Log ("hostData: " + hostData.Length);
		}
		
	}
	
	private void startServer ()
	{
		
		Debug.Log ("start new server");
		Network.InitializeSecurity ();
		Network.InitializeServer (8, 26566, !Network.HavePublicAddress ());
		
	}
	
	private void spawnPlayer ()
	{
		
		Debug.Log ("Spawn player");
		
		Network.Instantiate (PlayerPrefab, Vector3.zero, Quaternion.identity, 0);
		
	}

	private void spawnCamera ()
	{
		Debug.Log ("Spawn Camers");
		Instantiate (CameraPrefab, Vector3.zero, Quaternion.identity);	
	}
	
	void OnServerInitialized ()
	{
		
		Debug.Log ("server init.");
		MasterServer.RegisterHost (gameTypeName, "swingX_server_" + Random.Range (1000, 9999));
		spawnPlayer ();
		
	}
	
	void OnMasterServerEvent (MasterServerEvent masterServerEvent)
	{
		
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("RegistrationSucceeded");
		}
		
		if (masterServerEvent == MasterServerEvent.HostListReceived) {
			//Debug.Log ("HostListReceived" );
		}
		
	}
	
	void OnNetworkInstantiate (NetworkMessageInfo info)
	{
		Debug.Log ("New object instantiated by " + info.sender);
	}
	
	void OnPlayerDisconnected (NetworkPlayer player)
	{
		
		Debug.Log ("Clean up after player " + player);
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
		
	}
	
	void OnPlayerConnected (NetworkPlayer player)
	{
		
		Debug.Log ("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);
		
	}
	
	void OnApplicationQuit ()
	{
		if (Network.isServer) {
			
			Network.Disconnect (200);
			MasterServer.UnregisterHost ();
			
		}
		
		if (Network.isClient) {			
			Network.Disconnect (200);			
		}
	}
	
	void OnGUI ()
	{		
		
		if (Network.isClient) {
			Debug.Log ("im a client");
			if (GUILayout.Button ("Spawn")) {
				spawnPlayer ();
			}
			return;
		}
		
		if (Network.isServer) {
			
			GUILayout.Label ("im a server");
			return;
		} 
		
		float height = Screen.height - 200;
		float width = 150;
		float left = Screen.width / 2 - width;
		float top = Screen.height / 2 - height / 2;
		
		GUILayout.BeginArea (new Rect (left, top, width, height));
		
		
		if (GUILayout.Button ("Start new Server")) {
			startServer ();
		}
		
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Refresh Server List")) {
			StartCoroutine ("RefreshHostList");
		}
		
		GUILayout.Space (5);
		
		if (hostData != null) {
			
			foreach (HostData host in hostData) {
				
				if (GUILayout.Button (host.gameName)) {
					Network.Connect (host);
				}
			}
		}
		
		GUILayout.EndArea ();
	}
	
}