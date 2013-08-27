using UnityEngine;
using System.Collections;
using System;
using SocketIO.socketio.Messages;
using SocketIO.socketio;
using SimpleJson;

public class ClientSocket
{
	Client socket;
	
	public void Execute() {
		Debug.Log("Starting Socket client...");
		
		socket = new Client("http://127.0.0.1:3000/");
		
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
		
		JsonObject message = new JsonObject();
		message.Add("message", "hello world");
		message.Add("username", "unity");
		socket.Emit("message", message);
		
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
}

