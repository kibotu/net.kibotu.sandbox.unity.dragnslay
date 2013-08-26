using UnityEngine;
using System.Collections;
using System;
using SocketIOClient.Messages;
using SocketIOClient;
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
	}
		
	void SocketMessage(object sender, MessageEventArgs e)
	{		
		
		socket.On("message", (data) =>
		{
			Debug.Log("message received " + data );
		});
		
		JsonObject message = new JsonObject();
		message.Add("message", "hello world");
		message.Add("username", "unity");
		socket.Emit("message", message);
		
		//socket.Emit("send", getJsonObject);
		
		Debug.Log("sender : " + sender == null);
		Debug.Log("message : " + message == null);
		
		
		Debug.Log("SocketMessage: " + e.Message.Json.ToJsonString());
		//if (string.IsNullOrEmpty(e.Message.Event))
		    //Debug.Log("Generic SocketMessage: {0}", e.Message.MessageText);
		//else
	    Debug.Log("Generic SocketMessage: " +  e.Message.Event + " " + e.Message.Json.ToJsonString());
		
		
		Debug.Log("dump message "  + ObjectDumper.Dump(e));
		Debug.Log("dump sender " + ObjectDumper.Dump(sender));
	}
	
 	/*private static JsonObject getJsonObject() {
        JsonObject jObject = new JsonObject();
        jObject.put("name", "message");
        jObject.put("message", "hallo welt").put("username", "unity");
        return jObject;
    }*/
	
	void SocketError(object sender, ErrorEventArgs e)
	{
		Debug.Log("SocketError: " + e);
	}

	void SocketConnectionClosed(object sender, EventArgs e)
	{
		Debug.Log("SocketConnectionClosed: " + e);
	}
}

