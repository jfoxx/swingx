using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager_mp : MonoBehaviour {

	public List<Player> players;

	private GameState gameState;

	public int playerCount = 0;

	private static PlayerManager_mp instance;
	
	public static PlayerManager_mp Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new PlayerManager_mp();
			}
			
			return instance;
		}
	}	

	void Start ( ) 
	{
		instance = this;
		gameState = GameState.Instance;
		players = new List<Player>();
	}
	
	[RPC]
	void addPlayer(NetworkPlayer nPlayer){

		bool exists = false;
		foreach(Player pl in players)
		{
			if(pl.netId == int.Parse(nPlayer.ToString()))
			{
				Debug.Log("Player " + pl + " already exists in playerlist");
				exists = true;
			}
		}

		if(!exists)
		{
			Player pl = new Player();
			pl.score = 0;
			pl.health = 100;
			players.Add(pl);
		}
	}
	
	[RPC]
	void removePlayer(NetworkPlayer nPlayer){
		
		Player playerToRemove = null;
		foreach(Player pl in players)
		{
			if(pl.netId == int.Parse(nPlayer.ToString()))
			{
				playerToRemove = pl;
			}
		}
		players.Remove(playerToRemove);
	}	

	void OnServerInitialized ()
	{
		networkView.RPC("addPlayer", RPCMode.AllBuffered, Network.player);
	}
	
	void OnPlayerConnected (NetworkPlayer player)
	{
		Debug.Log ("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);	
		addPlayer(player);
		networkView.RPC("addPlayer", RPCMode.OthersBuffered, Network.player);
	}
	
	void OnPlayerDisconnected (NetworkPlayer player)
	{	
		Network.DestroyPlayerObjects (player);
		removePlayer(player);
		networkView.RPC("removePlayer", RPCMode.OthersBuffered, Network.player);		
	}
}

[System.Serializable]
public class Player
{
	public int score;
	public string name;
	public float health;
	public int netId;
	
}

