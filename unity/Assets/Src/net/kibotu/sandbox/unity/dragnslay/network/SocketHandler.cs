using System;
using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using Newtonsoft.Json.Linq;
using SimpleJson;
using UnityEngine;
using SocketIO.Client;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
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

        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private Namespace _socket;
        #elif UNITY_ANDROID 
        private AndroidJavaClass _socket;
        private string SocketHandlerClass = "net.kibotu.sandbox.network.SocketClient";
        #endif
        private static SocketHandler _instance;
        private Queue<MessageData> messageQueue;

        public void Awake()
        {
            messageQueue = new Queue<MessageData>();
        }

        public void Connect(string host, int port)
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR

            _socket = new SocketIOClient().Connect("http://" + host + ":" + port + "/");
            SetDelegates();
          
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

        public void Connect(int port)
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR

            NetworkHelper.DownloadJson("http://www.kibotu.net/server", result =>
            {
                _socket = new SocketIOClient().Connect("http://" + result[(string)result["network_interface"]] + ":" + port + "/");
                SetDelegates();
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
            _socket.On("connect", (args, callback) => ConnectCallback("Successfully connected. " + _socket.Name));

            _socket.On("message", (args, callback) => { foreach (JObject t in args) JSONCallback(t); });
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
        public void Emit(string name, JsonObject message)
        {
            Emit(name, message.ToString());
        }

        public void Emit(string name, string message)
        {
            var msg = new MessageData {name = name, message = message};
            Debug.Log("Enqueue " + msg.name + " " + msg.message);
            messageQueue.Enqueue(msg);
        }

        public void ConnectCallback(string error)
        {
            OnConnectEvent(error);
        }

        public void StringCallback(string message)
        {
            // remove json array artefact [ ] // todo handle multiple messages
            if(message[0].Equals('[')) Debug.LogError("error: received multiple messages, dropping all but first");
            OnStringEvent(message[0].Equals('[') ? message.Substring(1, message.Length - 2) : message);
        }

        public void JSONCallback(JObject message)
        {
            OnJSONEvent(message);
        }

        public void ReconnectCallback(string message)
        {
            OnReconnectEvent(message);
        }

        public void DisconnectCallback(string error)
        {
            OnDisconnectEvent(error);
        }

        public void ErrorCallback(string error)
        {
            OnErrorEvent(error);
        }

        public void ConnectionFailedCallback(string message)
        {
            OnConnectionFailedEvent(message);
        }

        public void Update()
        {
            while (messageQueue.Count > 0) // todo merge multiple messages into one
            {
                var msg = messageQueue.Dequeue();
                #if UNITY_STANDALONE_WIN || UNITY_EDITOR
                _socket.Emit(msg.name, msg.message);
                #elif UNITY_ANDROID
                _socket.CallStatic("Emit", msg.name, msg.message);
                #endif
            }
        }
    }
}
