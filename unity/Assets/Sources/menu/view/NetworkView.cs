using Assets.Sources.game;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class NetworkView : MonoBehaviour, IStringMessageEvent, IJSONMessageEvent
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

            var atlas = GameObject.Find("network_atlas").GetComponent<UIToolkit>();

            var padding = 0.00f;

            disconnected = atlas.addSprite("disconnected.png", 0, 0);
            disconnected.positionFromTopLeft(padding, 0.2f);
            disconnected.color = Color.white;

            connected = atlas.addSprite("connected.png", 0, 0);
            connected.positionFromTopLeft(padding, 0.23f);
            connected.color = Color.grey;

            activity = atlas.addSprite("activity.png", 0, 0);
            activity.positionFromTopLeft(padding, 0.26f);
            activity.color = Color.grey;

            ok = atlas.addSprite("ok.png", 0, 0);
            ok.positionFromTopLeft(padding, 0.29f);
            ok.color = Color.grey;

            error = atlas.addSprite("error.png", 0, 0);
            error.positionFromTopLeft(padding, 0.32f);
            error.color = Color.grey;

            #if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJNI.AttachCurrentThread();
                AndroidJNIHelper.debug = false;
            #endif

            SocketHandler.SharedConnection.OnConnectEvent += OnConnected;
            SocketHandler.SharedConnection.OnJSONEvent += OnJSONEvent;
            SocketHandler.SharedConnection.OnStringEvent += OnStringEvent;
            SocketHandler.SharedConnection.OnConnectionFailedEvent += OnConnectionFailedEvent;
            SocketHandler.SharedConnection.OnReconnectEvent += OnReconnectEvent;
            SocketHandler.SharedConnection.OnErrorEvent += OnErrorEvent;
            SocketHandler.SharedConnection.OnDisconnectEvent += OnDisconnectEvent;
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
