using UnityEngine;
using System.Collections;
using SocketIOClient;
using System;

public class UnitySocketIOClient
{

	Client socket;
	
	public void Execute() {
		Debug.Log("Starting Socket client...");
		
		socket = new Client("http://127.0.0.1:3000/");
		
		socket.Opened += SocketOpened;
		socket.Message += SocketMessage;
		socket.SocketConnectionClosed += SocketConnectionClosed;
		socket.Error += SocketError;
		
		socket.On("message", (data) =>
		{
			Debug.Log("message received " + data );
		});
	
		
		// make the socket.io connection
		socket.Connect();
	}
	
	void SocketOpened(object sender, EventArgs e)
	{
		Debug.Log("SocketOpened: " + sender + " " + e);
	}
		
	void SocketMessage(object sender, MessageEventArgs e)
	{
		Debug.Log("SocketMessage: " + sender + " " + e);
		// uncomment to show any non-registered messages
		//if (string.IsNullOrEmpty(e.Message.Event))
		//    Console.WriteLine("Generic SocketMessage: {0}", e.Message.MessageText);
		//else
		//    Console.WriteLine("Generic SocketMessage: {0} : {1}", e.Message.Event, e.Message.JsonEncodedMessage.ToJsonString());
	}
	
	void SocketError(object sender, ErrorEventArgs e)
	{
		Debug.Log("SocketError: " + sender + " " + e);
		Debug.Log("socket client error:");
		Debug.LogError(e.Message);
	}

	void SocketConnectionClosed(object sender, EventArgs e)
	{
		Debug.Log("SocketConnectionClosed: " + sender + " " + e);
		Debug.Log("WebSocketConnection was terminated!");
	}
}

