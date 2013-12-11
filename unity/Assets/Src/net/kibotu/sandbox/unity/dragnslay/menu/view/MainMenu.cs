using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
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

            //var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            //var map = UIButton.create(in_game_hud, "skills.png", "skills.png", 100, 100);
            //map.centerize();

            var mainMenuToolkit = GameObject.Find("main_menu").GetComponent<UIToolkit>();

            var playButton = UIButton.create(mainMenuToolkit, "button.png", "button.png", 0, 0);
            playButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
            playButton.centerize();
            playButton.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            playButton.onTouchUpInside += OnPlayButtonClicked;

            disconnected = mainMenuToolkit.addSprite("disconnected.png", 0, 0);
            disconnected.position = new Vector3(360, -10, 0);
            disconnected.color = Color.white;

            connected = mainMenuToolkit.addSprite("connected.png", 0, 0);
            connected.position = new Vector3(380, -10, 0);
            connected.color = Color.grey;

            activity = mainMenuToolkit.addSprite("activity.png", 0, 0);
            activity.position = new Vector3(400, -10, 0);
            activity.color = Color.grey;

            ok = mainMenuToolkit.addSprite("ok.png", 0, 0);
            ok.position = new Vector3(420, -10, 0);
            ok.color = Color.grey;

            error = mainMenuToolkit.addSprite("error.png", 0, 0);
            error.scale = new Vector3(1.2f,1.2f,0);
            error.position = new Vector3(440, -10, 0);
            error.color = Color.grey;

            #if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJNI.AttachCurrentThread();
                AndroidJNIHelper.debug = false;
            #endif
        }

        public void OnPlayButtonClicked(UIButton button)
        {
            SocketHandler.SharedConnection.OnConnectEvent += OnConnected;
            SocketHandler.SharedConnection.OnJSONEvent += OnJSONEvent;
            SocketHandler.SharedConnection.OnStringEvent += OnStringEvent;
            SocketHandler.SharedConnection.OnConnectionFailedEvent += OnConnectionFailedEvent;
            SocketHandler.SharedConnection.OnReconnectEvent += OnReconnectEvent;
            SocketHandler.SharedConnection.OnErrorEvent += OnErrorEvent;
            SocketHandler.SharedConnection.OnDisconnectEvent += OnDisconnectEvent;
            
            var game = new GameObject("Game").AddComponent<Game1vs1>();
            SocketHandler.SharedConnection.OnJSONEvent += game.OnJSONEvent;

            SocketHandler.Connect(1337);  
        }

        public void OnConnected(string error)
        {
            connected.color = Color.white;
            disconnected.color = Color.grey;
        }

        public void OnStringEvent(string message)
        {
            Game.ExecuteOnMainThread.Enqueue(() =>
            {
               activity.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
                  += () => activity.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
            });
        }

        public void OnJSONEvent(JObject message)
        {
            Debug.Log(message["message"]);
            Game.ExecuteOnMainThread.Enqueue(() =>
            {
                activity.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
                    += () => activity.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
            });
        }

        public void OnConnectionFailedEvent(string error)
        {
            connected.color = Color.grey;
            disconnected.color = Color.white;
        }

        public void OnReconnectEvent(string error)
        {
            Game.ExecuteOnMainThread.Enqueue(() =>
            {
                connected.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut).onComplete
                    += () => disconnected.colorFromTo(0.25f, Color.white, Color.grey, Easing.Bounce.easeInOut);
            });
        }

        public void OnErrorEvent(string message)
        {
            error.color = Color.white;
        }

        public void OnDisconnectEvent(string message)
        {
            Game.ExecuteOnMainThread.Enqueue(() =>
            {
                connected.color = Color.grey;
                disconnected.colorFromTo(0.25f, Color.grey, Color.white, Easing.Bounce.easeInOut);
            });
        }
    }
}
