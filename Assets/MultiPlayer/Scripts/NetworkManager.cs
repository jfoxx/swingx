using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
	public GUISkin skin;

	public GameManager_mp gameManager;

	public GameObject PlayerPrefab;
	public GameObject CameraPrefab;
	public GameObject MapPrefab;

	GameObject cameraObject;
	GameObject playerObject;
	
	public string gameTypeName;

	public bool isRefreshing = false;
	public bool isAlive = false;
	public bool gameHasStarted = false;
	public bool cameraSpawned = false;

	Rect serverWindowRect;
	Rect menuWindowRect;

	string playerName;

	float refreshRequestLength = 5f;
	HostData[] hostData;

	public static NetworkManager Instance;

	void Start ()
	{
		playerName = PlayerPrefs.GetString("playerName");
		if(playerName == ""){
			playerName = "player " + Random.Range(100, 999);
		}
		gameTypeName = "swingX_" + Application.unityVersion + "_server";
		Debug.Log (gameTypeName);
		gameManager = GetComponent<GameManager_mp>();
		isAlive = false;
		gameHasStarted = false;
		Instance = this;
		StartCoroutine ("RefreshHostList");
	}
	
	public IEnumerator RefreshHostList ()
	{
		
		Debug.Log ("Refreshing host...");
		MasterServer.RequestHostList (gameTypeName);

		float timeEnd = Time.time + refreshRequestLength;
		
		while (Time.time < timeEnd) {
			isRefreshing = true;
			hostData = MasterServer.PollHostList ();
			yield return new WaitForEndOfFrame ();

		}
		isRefreshing = false;
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
	
	private void spawnMap ()
	{
		
		Debug.Log ("Spawn Map");		
		Network.Instantiate (MapPrefab, Vector3.zero, Quaternion.identity, 0);
	
	}

	private void spawnPlayer ()
	{
		
		Debug.Log ("Spawn player");		
		playerObject = Network.Instantiate (PlayerPrefab, Vector3.zero, Quaternion.identity, 0) as GameObject;
		playerObject.transform.name = playerName;
		
		cameraObject.gameObject.GetComponent<Follow_mp>().target = playerObject.transform;
		
		isAlive = true;
	}

	private void spawnCamera ()
	{
		Debug.Log ("Spawn Camera");		
		cameraObject = Network.Instantiate (CameraPrefab, Vector3.zero, Quaternion.identity, 0) as GameObject;
		cameraSpawned = true;

	}

	public void playerDied(){
		Debug.Log("playerDied was run!");
		isAlive = false;
	}

	void OnServerInitialized ()
	{
		Debug.Log ("server init.");
		MasterServer.RegisterHost (gameTypeName, "swingX_server_" + Random.Range (1000, 9999));
		spawnMap();
		gameHasStarted = true;
	}

	void OnConnectedToServer()
	{
		gameHasStarted = true;
	}

	void OnMasterServerEvent (MasterServerEvent masterServerEvent)
	{
		
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration Succeeded");
		}
		
		if (masterServerEvent == MasterServerEvent.HostListReceived) {
			Debug.Log ("HostListReceived" );
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

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("Disconnected from server: " + info);
		if(Network.isClient){
			Application.LoadLevel(GameState.Instance.currentLevel);
		}
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
		GUI.skin = skin;

		if (!isAlive && gameHasStarted) 
		{
			float respawnButtonHeight = 150;
			float respawnButtonWidth = Screen.width/ 2;
			float respawnButtonLeft = Screen.width/2 -respawnButtonWidth/2;
			float respawnButtonTop = Screen.height / 2 - respawnButtonHeight/2;

			if(GUI.Button(new Rect (respawnButtonLeft, respawnButtonTop, respawnButtonWidth, respawnButtonHeight), "Spawn"))
			{
				if(!cameraSpawned)
				{
					spawnCamera ();
				}

				spawnPlayer();

			}
			return;
		}

		if(isAlive || gameHasStarted)
		{
			return;
		}
		
		float height = Screen.height - 200;
		float width = 300;
		float left = 10;
		float top = Screen.height / 2 - height / 2;
		
		menuWindowRect = new Rect (left, top, width, height);

		menuWindowRect = GUI.Window (3, menuWindowRect, menuWindowFunction, "Menu");
	}

	void menuWindowFunction (int windowID){

		GUILayout.BeginHorizontal();

		GUILayout.Label("Player name: ");

		playerName = GUILayout.TextField (playerName);

		GUILayout.EndHorizontal();
		
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Start new Server")) 
		{
			startServer ();

			PlayerPrefs.SetString("playerName", playerName);
		}
		
		GUILayout.Space (5);
		
		if (GUILayout.Button ("Refresh Server List")) 
		{
			StartCoroutine ("RefreshHostList");
		}
		
		GUILayout.Space (5);

		if(isRefreshing){
			GUILayout.Label("Refreshing...");
		}
		if (hostData != null) {
			if(hostData.Length != 0){

				GUILayout.Label("Servers");
				
				foreach (HostData host in hostData) {
					GUILayout.BeginHorizontal();
					GUILayout.Label(host.gameName);
					
					if (GUILayout.Button ("join")) 
					{
						Network.Connect (host);
						PlayerPrefs.SetString("playerName", playerName);
					}
					
					GUILayout.EndHorizontal();
				}
			}
		} 
		if(!isRefreshing && hostData.Length == 0){
			GUILayout.Label("No active servers. Try refreshing again later.");
		}
	}
}