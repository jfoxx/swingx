using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager_mp : MonoBehaviour {

	public List<Player> players;

	private GameState gameState;

	public int playerCount = 0;

	public virtual string playerName {
		get {
			return PlayerPrefs.GetString("playerName");
		}
	}

	void Start ( ) 
	{
		gameState = GameState.Instance;
		players = new List<Player>();
	}
	
	[RPC]
	void addPlayer(NetworkPlayer nPlayer)
	{
		int index = -1;
		
		for(int i = 0; i < players.Count; i++ )
		{
			if(players[i].networkPlayer == nPlayer)
			{
				index = i;
			}
		}
		
		if(index == -1) // add the player if its not in the list.
		{
			Debug.Log("Player [" + nPlayer.ToString() + "] not found.");
			Debug.Log("adding Player + [" + nPlayer.ToString() + "]");
			Player pl = new Player();
			pl.score = 0;
			pl.networkPlayer = nPlayer;
			players.Add(pl);
			playerCount++;
		}
	}
	
	[RPC]
	void removePlayer(NetworkPlayer nPlayer)
	{
		int index = -1;

		for(int i = 0; i < players.Count; i++ )
		{
			if(players[i].networkPlayer == nPlayer)
			{
				index = i;
				return;
			}
		}

		if(index == -1)
		{
			Debug.Log("Player [" + nPlayer.ToString() + "] not found.");
			return;
		}

		players.RemoveAt(index);
		playerCount--;

	}	

	[RPC]
	void updatePlayerHealth(NetworkPlayer nPlayer, float newHealth)
	{
		int index = -1;

		for(int i = 0; i < players.Count; i++ )
		{
			if(players[i].networkPlayer == nPlayer)
			{
				players[i].health = newHealth;
				index = i;
				return;
			}
		}

		if(index == -1)
		{
			Debug.Log("Player [" + nPlayer.ToString() + "] not found.");
		}
	}

	[RPC]
	void updatePlayerName(NetworkPlayer nPlayer, string pName)
	{	
		int index = -1;

		if(pName == "" || pName == null){
			pName = "Player " + Random.Range(1, 100);
		}

		for(int i = 0; i < players.Count; i++ )
		{
			if(players[i].networkPlayer == nPlayer)
			{
				players[i].name = pName;
				index = i;
			}
		}
		
		if(index == -1)
		{
			Debug.Log("Player [" + nPlayer.ToString() + "] not found.");
		}
	}

	// server connected, add player to list
	void OnServerInitialized () 
	{
		if(Network.isServer)
		{
			networkView.RPC("addPlayer", RPCMode.AllBuffered, Network.player);
			StartCoroutine("updatePlayerNameAfterSeconds");
		}
	}

	// a client connected, add player to list
	void OnPlayerConnected (NetworkPlayer player)
	{
		if(Network.isServer)
		{
			networkView.RPC("addPlayer", RPCMode.AllBuffered, player); 
		}
	}

	// a client disconnected, remove from the list
	void OnPlayerDisconnected (NetworkPlayer player)
	{
			networkView.RPC("removePlayer", RPCMode.AllBuffered, player);
			removePlayer(player);
			Debug.Log ("Clean up after player " + player);
			Network.RemoveRPCs (player);
			Network.DestroyPlayerObjects (player);
	}

	void OnConnectedToServer() 
	{
		Debug.Log("im a client and i just connected, my name is: " + playerName );
		StartCoroutine("updatePlayerNameAfterSeconds");
	}

	void publishName()
	{
		Debug.Log("publish my name [" + playerName + "]");
		updatePlayerName(Network.player, playerName);
		networkView.RPC("updatePlayerName", RPCMode.AllBuffered, Network.player, playerName);
	}

	public IEnumerator updatePlayerNameAfterSeconds ()
	{
		yield return new WaitForSeconds(2);
		publishName();
	}
	
}

[System.Serializable]
public class Player
{
	public int score;
	public string name;
	public float health;
	public NetworkPlayer networkPlayer;
	
}

