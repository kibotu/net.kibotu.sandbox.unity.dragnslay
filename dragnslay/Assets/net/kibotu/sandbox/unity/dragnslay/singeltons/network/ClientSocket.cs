using UnityEngine;
using System;
using SocketIO.socketio.Messages;
using SocketIO.socketio;
using SimpleJson;

public class ClientSocket : MonoBehaviour
{
	private Client socket;
	
	private static ClientSocket _instance;

 	public static ClientSocket Instance {
		get { return _instance ?? (_instance = new GameObject("ClientSocket").AddComponent<ClientSocket>()); }
    }
	
	public void Connect(string url) {
		Debug.Log("Starting Socket client...");
		
		socket = new Client(url);
		
		socket.Opened += SocketOpened;
		socket.Message += SocketMessage;
		socket.SocketConnectionClosed += SocketConnectionClosed;
		socket.Error += SocketError;
		
		// make the socket.io connection
		socket.Connect();
	}
	
	void SocketOpened(object sender, EventArgs e)
	{
		Debug.Log("SocketOpened");
	}
		
	void SocketMessage(object sender, MessageEventArgs e)
	{	
		Debug.Log("SocketMessage");
		
	 	if(e != null)
	    {
	        Debug.Log("Message: " + e.Message.Event + " " + e.Message.MessageText);
	    }
	}
	
	void SocketError(object sender, ErrorEventArgs e)
	{
		Debug.Log("SocketError");
	}

	void SocketConnectionClosed(object sender, EventArgs e)
	{
		Debug.Log("SocketConnectionClosed");
	}

    public void Emit(string message, JsonMessage jsonString)
    {
        socket.Emit(message, jsonString);
    }

    public void Emit(string message, string jsonString)
    {
        Emit(message, new JsonMessage(jsonString));
    }

    public void Emit(string message, JsonObject jsonString) 
    {
		socket.Emit(message, jsonString);
	}

    public void On(string eventName, Action<IMessage> action)
    {
        socket.On(eventName, action);
    }
}

