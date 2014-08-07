using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Sources.components.data;
using Assets.Sources.utility;
using Newtonsoft.Json.Linq;
//using SimpleJson;
//using SocketIO.Client;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

#endif

namespace Assets.Sources.network
{
    class SocketHandler : MonoBehaviour
    {
        #region delegates

        public event Action<String> OnConnectEvent;
        public event Action<JObject> OnJSONEvent;
        public event Action<String> OnStringEvent;
        public event Action<String> OnConnectionFailedEvent;
        public event Action<String> OnReconnectEvent;
        public event Action<String> OnErrorEvent;
        public event Action<String> OnDisconnectEvent;
        public bool LoggingEnabled = false;

        #endregion

        #region init

        public string Ip = "undefined";

        #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
//        private Namespace _socket;
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

        public static SocketHandler SharedConnection
        {
            get { return _instance ?? (_instance = new GameObject("SocketHandler").AddComponent<SocketHandler>()); }
        }

        #endregion

        #region connect

        public static void Connect(string host, int port)
        {
            SocketIOClient.Client socket;

            socket = new SocketIOClient.Client("http://188.106.177.88:1337/");
            socket.On("message", (fn) =>
            {
                Debug.Log("connect - socket");
                Debug.Log("response " + fn.MessageText + " " + fn.Json.ToJsonString() + " " + fn.RawMessage);

//                Dictionary<string, string> args = new Dictionary<string, string>();
//                args.Add("message", "what's up?");
//                socket.Emit("SEND", args);
            });
            socket.Error += (sender, e) =>
            {
                Debug.Log("socket Error: " + e.Message.ToString());
            };
            socket.Connect();
//
//
//            _instance.Ip = "http://" + host + ":" + port + "/";
//
//            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
//
//            SharedConnection._socket = new SocketIOClient().Connect(_instance.Ip);
//            SharedConnection.SetDelegates();
//          
//            #elif UNITY_ANDROID 

            AndroidJNIHelper.debug = true;
//            if (_socket == null)
//            {
//                _socket = new AndroidJavaClass(SocketHandlerClass);
//                // System.Threading.Thread(_socket.CallStatic("connect", host, port));
//                _socket.CallStatic("connect", host, port);
//            }

//            #endif
        }

        public static void Connect(int port)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            NetworkHelper.DownloadJson("http://www.kibotu.net/server", result =>
            {
                _instance.Ip = "http://" + result[(string)result["network_interface"]] + ":" + port + "/";
//                SharedConnection._socket = new SocketIOClient().Connect(_instance.Ip);
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

        #endregion

        #region emit

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
            if(SharedConnection.LoggingEnabled) 
                Debug.Log("Enqueue '" + msg.name + "' " + msg.message);
            SharedConnection.messageQueue.Enqueue(msg);
        }

        public static void EmitNow(string name, JObject message)
        {
            SharedConnection.EmitNow(name, message.ToString());
        }

        public void EmitNow(string name, string message)
        {
            SharedConnection.EmitNow(new MessageData { name = name, message = message });
        }

        protected void EmitNow(MessageData msg)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
//               _socket.Emit(msg.name, msg.message);
            #elif UNITY_ANDROID
                _socket.CallStatic("Emit", msg.name, msg.message);
            #endif

            if(LoggingEnabled) 
                Debug.Log("EmitNow '" + msg.name + "' " + msg.message);
        }

        public void Update()
        {
            while (messageQueue.Count > 0) // todo merge multiple messages into one
            {
                EmitNow(messageQueue.Dequeue());
            }
        }
        #endregion

        #region delegates

        private void SetDelegates()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

//            _socket.On("connect", (args, callback) => ConnectCallback("Successfully connected. " + _socket.Name));
//            _socket.On("message", (args, callback) => { foreach (JObject t in args) JSONCallback(t); });

            #endif
        }

        protected void ConnectCallback(string error)
        {
            Debug.Log("ConnectCallback " + error);
            OnConnectEvent(error);
        }

        protected void StringCallback(string message)
        {
            Debug.Log("StringCallback " + message);
            foreach (JObject t in JArray.Parse(message)) JSONCallback(t); 
        }

        protected void JSONCallback(JObject message)
        {
            Debug.Log("JSONCallback " + message);
            OnJSONEvent(message);
        }

        protected void ReconnectCallback(string message)
        {
            Debug.Log("ReconnectCallback " + message);
            OnReconnectEvent(message);
        }

        protected void DisconnectCallback(string error)
        {
            Debug.Log("ConnectCallback " + error);
            DisconnectCallback(error);
        }

        protected void ErrorCallback(string error)
        {
            Debug.Log("ErrorCallback " + error);
            OnErrorEvent(error);
        }

        protected void ConnectionFailedCallback(string message)
        {
            Debug.Log("ConnectionFailedCallback " + message);
            OnConnectionFailedEvent(message);
        }

        #endregion
    }
}
