using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MainMenu : MonoBehaviour, IStringMessageEvent, IJSONMessageEvent
    {
        private UISprite disconnected;
        private UISprite connected;
        private UISprite activity;
        private UISprite ok;
        private UISprite error;

        public void Start ()
        {
            // @ see http://forum.unity3d.com/threads/87917-Prime31-UIToolkit-Multi-Resolution-GUI-Solution-Ready-for-Use-and-it-s-free/page79
            // @ see http://timshaya.wordpress.com/category/uitoolkit/
            var playButton = UIButton.create("button.png", "button.png", 0, 0);
            playButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
            playButton.centerize();
            playButton.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            playButton.onTouchUpInside += OnPlayButtonClicked;

            var toolkit = GameObject.Find("playButton").GetComponent<UIToolkit>();

            disconnected = toolkit.addSprite("disconnected.png", 0, 0);
            disconnected.position = new Vector3(360, -10, 0);
            disconnected.color = Color.white;

            connected = toolkit.addSprite("connected.png", 0, 0);
            connected.position = new Vector3(380, -10, 0);
            connected.color = Color.grey;

            activity = toolkit.addSprite("activity.png", 0, 0);
            activity.position = new Vector3(400, -10, 0);
            activity.color = Color.grey;

            ok = toolkit.addSprite("ok.png", 0, 0);
            ok.position = new Vector3(420, -10, 0);
            ok.color = Color.grey;

            error = toolkit.addSprite("error.png", 0, 0);
            error.scale = new Vector3(1.2f,1.2f,0);
            error.position = new Vector3(440, -10, 0);
            error.color = Color.grey;


        }

        public void OnPlayButtonClicked(UIButton button)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJNI.AttachCurrentThread();
                AndroidJNIHelper.debug = false;
            #endif

            SocketHandler.Instance.OnConnectEvent += OnConnected;
            SocketHandler.Instance.OnJSONEvent += OnJSONEvent;
            SocketHandler.Instance.OnStringEvent += OnStringEvent;
            SocketHandler.Instance.OnConnectionFailedEvent += OnConnectionFailedEvent;
            SocketHandler.Instance.OnReconnectEvent += OnReconnectEvent;
            SocketHandler.Instance.OnErrorEvent += OnErrorEvent;
            SocketHandler.Instance.OnDisconnectEvent += OnDisconnectEvent;

            var world = GameObject.Find("World");
            world.AddComponent<WorldData>();
            SocketHandler.Instance.OnStringEvent += world.AddComponent<Game1vs1>().OnStringEvent;

            // serverIp = "http://192.168.198.50:3000"; 
            //host = "178.0.89.213";

            SocketHandler.Instance.Connect("172.16.2.141", 1337); // wooga guest wlan ip
            // SocketHandler.Instance.Connect("172.19.253.37", 1337); 
        }

        public void OnConnected(string error)
        {
            Debug.Log("OnConnected " + error);
            connected.color = Color.white;
            disconnected.color = Color.grey;
        }

        public void OnStringEvent(string message)
        {
            Debug.Log("OnStringEvent " + error);
            activity.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
               += () => activity.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
        }

        public void OnJSONEvent(string message)
        {
            Debug.Log("OnJSONEvent " + error);
            activity.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
                += () => activity.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
        }

        public void OnConnectionFailedEvent(string error)
        {
            Debug.Log("OnConnectionFailedEvent " + error);
            connected.color = Color.grey;
            disconnected.color = Color.white;
        }

        public void OnReconnectEvent(string error)
        {
            Debug.Log("OnReconnectEvent " + error); 
            connected.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
                += () => disconnected.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
        }

        public void OnErrorEvent(string message)
        {
            Debug.Log("OnErrorEvent " + error);
            error.color = Color.white;
        }

        public void OnDisconnectEvent(string message)
        {
            Debug.Log("OnDisconnectEvent " + error);
            connected.color = Color.grey;
            disconnected.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut);
        }
    }
}
