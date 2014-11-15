using System;
using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using SocketIO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        private static SocketHandler _instance;
//        private SocketIOComponent _socket;
//        private readonly Queue<MessageData> _messageQueue;

        public SocketHandler()
        {
//            _messageQueue = new Queue<MessageData>();
#if UNITY_EDITOR
            EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;
#endif
        }

        public static SocketHandler SharedConnection { get { return _instance ?? (_instance = new SocketHandler()); } }

        #endregion

        #region connect

        private void ConnectInternal(string host, int port)
        {
//            _socket = new GameObject("SocketHandler").AddComponent<SocketIOComponent>();
//            _socket.SetUri(host, port);
//            _socket.Init();
//            Debug.Log("Connecting to: " + SharedConnection._socket.url);
//            _socket.Connect();
//            SetDelegates();
        }

        public static void Connect(string host, int port)
        {
            SharedConnection.ConnectInternal(host, port);
        }

        public static void Connect()
        {
            NetworkHelper.DownloadJson("http://www.kibotu.net/server", result => SharedConnection.ConnectInternal(result[(string)result["network_interface"]].ToString(), Int32.Parse(result["tcp_port"].ToString())));
        }

        public static void Disconnect()
        {
//            SharedConnection._socket.Disconnect();
        }

        void HandleOnPlayModeChanged()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
#endif
                Disconnect();
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
            Game.ExecuteOnMainThread.Enqueue(() => SharedConnection.EmitNow(msg));
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
//            _socket.Emit(msg.name, msg.message);

            if(LoggingEnabled) 
                Debug.Log("EmitNow '" + msg.name + "' " + msg.message);
        }

        #endregion

        #region delegates

        private void SetDelegates()
        {
//            _socket.On("open", ConnectCallback);
//            _socket.On("connect", ConnectCallback2);
//            _socket.On("error", ErrorCallback);
//            _socket.On("close", DisconnectCallback);
//            _socket.On("reconnect", ReconnectCallback);
//            _socket.On("message", JSONCallback);
        }

//        protected void ConnectCallback(SocketIOEvent error)
//        {
//            Debug.Log("ConnectCallback " + error);
//            OnConnectEvent(error.name + " " + error.data);
//        }

//        protected void ConnectCallback2(SocketIOEvent error)
//        {
//            Debug.Log("ConnectCallback2 " + error);
//            OnConnectEvent(error.name + " " + error.data);
//        }

//        protected void StringCallback(SocketIOEvent message)
//        {
////            Debug.Log("StringCallback " + message);
////            foreach (JObject t in JArray.Parse(message)) JSONCallback(t); 
//        }

//        protected void JSONCallback(SocketIOEvent message)
//        {
//            //            Debug.Log("JSONCallback " + message);
//            OnJSONEvent((JObject)JsonConvert.DeserializeObject(message.data.ToString())); // #cloud todo use only one jsonobject lib
//        }

//        protected void ReconnectCallback(SocketIOEvent message)
//        {
//            Debug.Log("ReconnectCallback " + message);
//            OnReconnectEvent(message.name);
//        }

//        protected void DisconnectCallback(SocketIOEvent error)
//        {
//            Debug.Log("DisconnectCallback " + error);
//            OnDisconnectEvent(error.name);
//        }

//        protected void ErrorCallback(SocketIOEvent error)
//        {
//            Debug.Log("ErrorCallback " + error);
//            OnErrorEvent(error.name);
//        }

//        protected void ConnectionFailedCallback(SocketIOEvent message)
//        {
//            Debug.Log("ConnectionFailedCallback " + message.name + " " + message.data);
//            OnConnectionFailedEvent(message.name + " " + message.data);
//        }

        #endregion
    }
}
