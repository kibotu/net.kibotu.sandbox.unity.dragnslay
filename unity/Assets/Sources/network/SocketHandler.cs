using System;
using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.utility;
using Newtonsoft.Json.Linq;
using SocketIO;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

#endif

namespace Assets.Sources.network
{
    class SocketHandler
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

        #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
        #elif UNITY_ANDROID 
        private static AndroidJavaClass _socket;
        private const string SocketHandlerClass = "net.kibotu.sandbox.network.SocketClient";
        #endif
        private static SocketHandler _instance;
        private SocketIOComponent _socket;
        private readonly Queue<MessageData> _messageQueue;

        public SocketHandler()
        {
            _messageQueue = new Queue<MessageData>();
        }

        public static SocketHandler SharedConnection { get { return _instance ?? (_instance = new SocketHandler()); } }

        #endregion

        #region connect

        public static void Connect(string host, int port)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

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

        public static void Connect()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            NetworkHelper.DownloadJson("http://www.kibotu.net/server", result =>
            {
                SharedConnection._socket = new GameObject("SocketHandler").AddComponent<SocketIOComponent>();
                SharedConnection._socket.SetUri(result[(string)result["network_interface"]].ToString(), Int32.Parse(result["tcp_port"].ToString()));
                SharedConnection._socket.Init();
                SharedConnection._socket.Connect();
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

        public void TestOpen(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        }

        public void TestError(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
        }

        public void TestClose(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
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
            SharedConnection._messageQueue.Enqueue(msg);
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
            while (_messageQueue.Count > 0) // todo merge multiple messages into one
            {
                EmitNow(_messageQueue.Dequeue());
            }
        }
        #endregion

        #region delegates

        private void SetDelegates()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

            _socket.On("open", ConnectCallback);
            _socket.On("connect", ConnectCallback2);
            _socket.On("error", ErrorCallback);
            _socket.On("close", DisconnectCallback);
            _socket.On("reconnect", ReconnectCallback);
            _socket.On("message", StringCallback);

            #endif
        }

        protected void ConnectCallback(SocketIOEvent error)
        {
            Debug.Log("ConnectCallback " + error);
            OnConnectEvent(error.name + " " + error.data);
        }

        protected void ConnectCallback2(SocketIOEvent error)
        {
            Debug.Log("ConnectCallback2 " + error);
            OnConnectEvent(error.name + " " + error.data);
        }

        protected void StringCallback(SocketIOEvent message)
        {
            Debug.Log("StringCallback " + message);
//            foreach (JObject t in JArray.Parse(message)) JSONCallback(t); 
        }

        protected void JSONCallback(JObject message)
        {
            Debug.Log("JSONCallback " + message);
            OnJSONEvent(message);
        }

        protected void ReconnectCallback(SocketIOEvent message)
        {
            Debug.Log("ReconnectCallback " + message);
            OnReconnectEvent(message.name);
        }

        protected void DisconnectCallback(SocketIOEvent error)
        {
            Debug.Log("DisconnectCallback " + error);
            OnDisconnectEvent(error.name);
        }

        protected void ErrorCallback(SocketIOEvent error)
        {
            Debug.Log("ErrorCallback " + error);
            OnErrorEvent(error.name);
        }

        protected void ConnectionFailedCallback(SocketIOEvent message)
        {
            Debug.Log("ConnectionFailedCallback " + message.name + " " + message.data);
            OnConnectionFailedEvent(message.name + " " + message.data);
        }

        #endregion
    }
}
