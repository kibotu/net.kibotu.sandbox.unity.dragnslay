using Assets.net.kibotu.sandbox.unity.dragnslay.model;
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
	}
	
	// Update is called once per frame
	void Update () {
        startTime += Time.deltaTime;
        if (startTime > 3f)
        {
            startTime = 0;
            Emit("hallo", "welt");
        }
	}

    // @see http://forum.unity3d.com/threads/100751-Android-Plugin-JNI-Question
    private void useJNI()
    {
        using (AndroidJavaClass clsUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject objActivity = clsUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {

                objActivity.CallStatic("Emit", "hallo", "welt");
            }
        }
    }

    public void Emit(string name, string message)
    {
        if (socket == null) socket = new AndroidJavaClass("net.kibotu.sandbox.unity.android.SocketFacade");
        socket.CallStatic("Emit", name, message);
    }
}
