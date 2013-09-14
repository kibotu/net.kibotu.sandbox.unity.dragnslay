using System;
using Assets.net.kibotu.sandbox.unity.dragnslay.model;
using SimpleJson;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public UIToolkit buttonToolkit;
    private float startTime = 0;
    private AndroidJavaClass socket; 

	// Use this for initialization
	void Start ()
	{
        Debug.Log("drawing button");
        var playButton = UIButton.create("button.png", "button.png", 0, 0);
        playButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
        playButton.scale = new Vector3(0.3f, 0.3f, 0);
        AndroidJNI.AttachCurrentThread();
        AndroidJNIHelper.debug = false; 
	}
	
	// Update is called once per frame
	void Update () {
        startTime += Time.deltaTime;
        if (startTime > 3f)
        {
            startTime = 0;
            Emit("send", createHelloWorldMessage());
        }
	}

    public void Emit(string name, JsonObject message)
    {
        Emit(name, message.ToString());
    }

    public void Emit(string name, string message)
    {
        #if UNITY_ANDROID
        if (socket == null)
        {
            socket = new AndroidJavaClass("net.kibotu.sandbox.unity.android.SocketFacade");
            socket.CallStatic("setUrl", "http://172.19.253.37:3000");
        }
            socket.CallStatic("Emit",name, message);
        #endif
    }

    public JsonObject createMessage()
    {
        return new JsonObject{
            {"name", "move-units"},
            {"source", gameObject.name},
            {"dest", "2"},
            {"amount", "1"}
        };
    }

    public JsonObject createHelloWorldMessage()
    {
        return new JsonObject
            {
                {"message", "hallo welt"},
                {"username", "android"},
                {"name", "message"},
            };
    }
}
