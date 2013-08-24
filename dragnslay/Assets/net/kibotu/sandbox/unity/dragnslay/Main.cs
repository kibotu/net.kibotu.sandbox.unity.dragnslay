using UnityEngine;
using System;
using System.Collections;
using SocketIOClient;
using SimpleJSON;
using SocketIOClient.Messages;

public class Main : MonoBehaviour {
	
	public Texture btnTexture;
	private bool buttonIsVisible = true;
	private Client socket;
	
    void OnGUI() {
        if (!btnTexture) {
            Debug.LogError("Please assign a texture on the inspector");
            return;
        }
        if (buttonIsVisible && GUI.Button(new Rect(10, 10, 50, 50), btnTexture)) {
            Debug.Log("Clicked the button with an image");
			buttonIsVisible = false;
			startGame();
		}
    }
	
	void startGame() {
		
		Planet [] p = new Planet[10] { new Planet() };
		// add planets to stage
		
		// spawn ships
		
		// touch events
		
		// server conection + transmitting touch events
	}
	
	void connectToServer() {
		socket = new Client("http://172.16.3.13:3000");
		socket.Connect();
		
		
		socket.C((data) => { 
			Debug.Log("received message : " + data);
		});
		
		socket.On ("message", (data) => { 
			Debug.Log("received message : " + data);
		});
		
	 	socket.Emit("send", new JSONMessage("{ message : \"hallo world\", username : \"unity\" }")); //, "", (data) => { Debug.Log("successfully send event: " + data); } );	
	}
	
	void Start () {
	
	}
	
	void Update () {
	
	}
}
