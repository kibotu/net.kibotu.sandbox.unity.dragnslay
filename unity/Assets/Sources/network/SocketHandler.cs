using System;
using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.utility;
using Newtonsoft.Json.Linq;
using SocketIO.Client;
using UnityEngine;
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

#endif

namespace Assets.Sources.network
{
    class SocketHandler : MonoBehaviour
    {
        public event Action<String> OnConnectEvent;
        public event Action<JObject> OnJSONEvent; // msgNewtonsoft.Json.Linq.JObject
        public event Action<String> OnStringEvent;
        public event Action<String> OnConnectionFailedEvent;
        public event Action<String> OnReconnectEvent;
        public event Action<String> OnErrorEvent;
        public event Action<String> OnDisconnectEvent;

        public string Ip = "undefined";

        #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        private Namespace _socket;
        #elif UNITY_ANDROID 
        private static AndroidJavaClass _socket;
        private const string SocketHandlerClass = "net.kibotu.sandbox.network.SocketClient";
        #endif
        private static SocketHandler _instance;
        private Queue<MessageData> messageQueue;

        public void Awake()
        {
            messageQueue = new Queue<MessageData>();
        }

        public static void Connect(string host, int port)
        {
            _instance.Ip = "http://" + host + ":" + port + "/";

            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            SharedConnection._socket = new SocketIOClient().Connect(_instance.Ip);
            SharedConnection.SetDelegates();
          
            #elif UNITY_ANDROID 

            AndroidJNIHelper.debug = true;
            if (_socket == null)
            {
                _socket = new AndroidJavaClass(SocketHandlerClass);
                // System.Threading.Thread(_socket.CallStatic("connect", host, port));
                _socket.CallStatic("connect", host, port);
            }

            #endif
        }

        public static void Connect(int port)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            NetworkHelper.DownloadJson("http://www.kibotu.net/server", result =>
            {
                _instance.Ip = "http://" + result[(string)result["network_interface"]] + ":" + port + "/";
                SharedConnection._socket = new SocketIOClient().Connect(_instance.Ip);
                SharedConnection.SetDelegates();
            });

            #elif UNITY_ANDROID

            AndroidJNIHelper.debug = true;
            if (_socket == null)
            {
                _socket = new AndroidJavaClass(SocketHandlerClass);
                _socket.CallStatic("connect", port);
            }
            
            #endif
        }

        private void SetDelegates()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            _socket.On("connect", (args, callback) => ConnectCallback("Successfully connected. " + _socket.Name));

            _socket.On("message", (args, callback) => { foreach (JObject t in args) JSONCallback(t); });
            
            #endif
        }

        public static SocketHandler SharedConnection
        {
            get { return _instance ?? (_instance = new GameObject("SocketHandler").AddComponent<SocketHandler>()); }
        }

        /// <summary>
        /// Emitting a message to the server.
        /// </summary>
        /// <param name="name">MessageData name.</param>
        /// <param name="message">Json message.</param>
        public static void Emit(string name, JObject message)
        {
            Emit(name, message.ToString());
        }

        public static void Emit(string name, string message)
        {
            var msg = new MessageData {name = name, message = message};
            Debug.Log("Enqueue " + msg.name + " " + msg.message);
            SharedConnection.messageQueue.Enqueue(msg);
        }

        protected void ConnectCallback(string error)
        {
            OnConnectEvent(error);
        }

        protected void StringCallback(string message)
        {
            foreach (JObject t in JArray.Parse(message)) JSONCallback(t); 
        }

        protected void JSONCallback(JObject message)
        {
            OnJSONEvent(message);
        }

        protected void ReconnectCallback(string message)
        {
            OnReconnectEvent(message);
        }

        protected void DisconnectCallback(string error)
        {
            OnDisconnectEvent(error);
        }

        protected void ErrorCallback(string error)
        {
            OnErrorEvent(error);
        }

        protected void ConnectionFailedCallback(string message)
        {
            OnConnectionFailedEvent(message);
        }

        public void Update()
        {
            while (messageQueue.Count > 0) // todo merge multiple messages into one
            {
                var msg = messageQueue.Dequeue();
                #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
                _socket.Emit(msg.name, msg.message);
                #elif UNITY_ANDROID
                    _socket.CallStatic("Emit", msg.name, msg.message);
                #endif
            }
        }
    }
}
