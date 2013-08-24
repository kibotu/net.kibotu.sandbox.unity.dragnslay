using UnityEngine;
using System;
using System.Collections;
using SocketIOClient;

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
		socket = new Client("http://172.16.3.13:3000");
		socket.Connect();
		
		
	}
	
	void Start () {
	
	}
	
	void Update () {
	
	}
}
