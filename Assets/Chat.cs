using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Chat : MonoBehaviour {

	public GUISkin skin;

	public static bool usingChat;
	public bool showChat;

	private Rect chatAreaRect; 
	private Rect inputAreaRect;

	private float top;
	private float left;
	private float height;
	private float width;


	bool enterPressed = false;

	private List<ChatMessage> _messages = new List<ChatMessage>();

	private string _message = "";

	public virtual bool _chat{
		get{
			return Input.GetButtonDown("Chat");
		}
	}

	public virtual bool send{
		get{
			return Input.GetKeyDown(KeyCode.Return);
		}
	}

	void Start () {
		if (networkView.isMine){
			enabled = false;
		}
		showChat = false;
	}
	

	void Update () 
	{
		if(_chat)
		{
			toggleChat();
		}

		if(enterPressed)
		{
			enterPressed = false;


			if(_message != "")
			{
				sendMessage();
				_message = "";
			}

			toggleChat();
		}
		Debug.Log(_message);
	}

	void toggleChat(){
		showChat = !showChat;
		_message = "";
	}
	
	void OnGUI ()
	{
		GUI.skin = skin;

		height	= Screen.height / 2;
		width = 300;
		top = Screen.height - height - 50;
		left = 20;

		chatAreaRect = new Rect(left, top, width, height);

		GUILayout.BeginArea(chatAreaRect);

		GUILayout.FlexibleSpace();

		foreach(ChatMessage mess in _messages)
		{
			GUILayout.Space(5);
			GUILayout.Label("[" + mess.name + "]:  " + mess.text);
		}

		GUILayout.EndArea();


		if(showChat)
		{
			float inputheight	= 30;
			float inputwidth = 300;
			float inputtop = Screen.height - inputheight - 10;
			float inputleft = 20;

			inputAreaRect = new Rect (inputleft, inputtop, inputwidth, inputheight);
			GUILayout.BeginArea (inputAreaRect);

			_message = GUILayout.TextField( _message);

			if (Event.current.character == '\n' && showChat)
			{
				enterPressed = true;
			}

			GUILayout.EndArea();
		}

	}

	void sendMessage(){
		networkView.RPC("addMessage", RPCMode.AllBuffered, PlayerPrefs.GetString("playerName"), _message);
	}


	[RPC]
	void addMessage(string pName, string pText){

		Debug.Log("name: "+ pName);
		Debug.Log("text: "+ pText);

		ChatMessage mess = new ChatMessage();

		mess.name = pName;
		mess.text = pText;

		if(_messages.Count > 10)
		{
			_messages.RemoveAt(0);
		}	
	
		_messages.Add(mess);

	}

}


public class ChatMessage { 
	public string text = "";
	public string name = "";

}
