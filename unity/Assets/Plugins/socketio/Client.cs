using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleJson;
using SocketIOClient.Eventing;
using SocketIOClient.Messages;
using WebSocket4Net;
using Debug = UnityEngine.Debug;

namespace SocketIOClient
{
	/// <summary>
	/// Class to emulate socket.io javascript client capabilities for .net classes
	/// </summary>
	/// <exception cref = "ArgumentException">Connection for wss or https urls</exception>  
	public class Client : IDisposable, SocketIOClient.IClient
	{
		private Timer socketHeartBeatTimer; // HeartBeat timer 
		//private Task dequeuOutBoundMsgTask;
		private Thread dequeuOutBoundMsgTask;
		private ConcurrentQueue<string> outboundQueue;
		private int retryConnectionCount = 0;
		private int retryConnectionAttempts = 3;
		private readonly static object padLock = new object(); // allow one connection attempt at a time

		/// <summary>
		/// Uri of Websocket server
		/// </summary>
		protected Uri uri;
		/// <summary>
		/// Underlying WebSocket implementation
		/// </summary>
        protected WebSocket wsClient;
		/// <summary>
		/// RegistrationManager for dynamic events
		/// </summary>
		protected RegistrationManager registrationManager;  // allow registration of dynamic events (event names) for client actions
		/// <summary>
		/// By Default, use WebSocketVersion.Rfc6455
		/// </summary>
		protected WebSocketVersion socketVersion = WebSocketVersion.Rfc6455;

		// Events
		/// <summary>
		/// Opened event comes from the underlying websocket client connection being opened.  This is not the same as socket.io returning the 'connect' event
		/// </summary>
		public event EventHandler Opened;
		public event EventHandler<MessageEventArgs> Message;
		public event EventHandler ConnectionRetryAttempt;
		public event EventHandler HeartBeatTimerEvent;
		/// <summary>
		/// <para>The underlying websocket connection has closed (unexpectedly)</para>
		/// <para>The Socket.IO service may have closed the connection due to a heartbeat timeout, or the connection was just broken</para>
		/// <para>Call the client.Connect() method to re-establish the connection</para>
		/// </summary>
		public event EventHandler SocketConnectionClosed;
		public event EventHandler<ErrorEventArgs> Error;

		/// <summary>
		/// ResetEvent for Outbound MessageQueue Empty Event - all pending messages have been sent
		/// </summary>
		public ManualResetEvent MessageQueueEmptyEvent = new ManualResetEvent(true);

		/// <summary>
		/// Connection Open Event
		/// </summary>
		public ManualResetEvent ConnectionOpenEvent = new ManualResetEvent(false);


		/// <summary>
		/// Number of reconnection attempts before raising SocketConnectionClosed event - (default = 3)
		/// </summary>
		public int RetryConnectionAttempts
		{
			get { return this.retryConnectionAttempts; }
			set { this.retryConnectionAttempts = value; }
		}

		/// <summary>
		/// Value of the last error message text  
		/// </summary>
		public string LastErrorMessage = "";

		/// <summary>
		/// Represents the initial handshake parameters received from the socket.io service (SID, HeartbeatTimeout etc)
		/// </summary>
		public SocketIOHandshake HandShake { get; private set; }

		/// <summary>
		/// Returns boolean of ReadyState == WebSocketState.Open
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return this.ReadyState == WebSocketState.Open;
			}
		}

		/// <summary>
		/// Connection state of websocket client: None, Connecting, Open, Closing, Closed
		/// </summary>
		public WebSocketState ReadyState
		{
			get
			{
				if (this.wsClient != null)
					return this.wsClient.State;
				else
					return WebSocketState.None;
			}
		}

		// Constructors
		public Client(string url)
			: this(url, WebSocketVersion.Rfc6455)
		{
		}

		public Client(string url, WebSocketVersion socketVersion)
		{
			this.uri = new Uri(url);

			this.socketVersion = socketVersion;

			this.registrationManager = new RegistrationManager();
			this.outboundQueue =  (new ConcurrentQueue<string>());
			this.dequeuOutBoundMsgTask = new Thread(dequeuOutboundMessages);
//			this.dequeuOutBoundMsgTask = Task.Factory.StartNew(() => dequeuOutboundMessages(), TaskCreationOptions.LongRunning);
			this.dequeuOutBoundMsgTask.Start();
		}

		/// <summary>
		/// Initiate the connection with Socket.IO service
		/// </summary>
		public void Connect()
		{
			lock (padLock)
			{
			    if (this.ReadyState == WebSocketState.Connecting || this.ReadyState == WebSocketState.Open) return;

			    try
			    {
			        this.ConnectionOpenEvent.Reset();
			        this.HandShake = this.requestHandshake(uri);// perform an initial HTTP request as a new, non-handshaken connection

			        if (this.HandShake != null && !string.IsNullOrEmpty(this.HandShake.SID) && !this.HandShake.HadError)
			        {
			            // ws://188.106.177.88:1337/socket.io/1/websocket/KWdZrUn5dt76E3DFAAAi
			            // ws://188.106.177.88:1337/EIO=2&transport=websocket&sKWdZrUn5dt76E3DFAAAi
			            var socketUri = string.Format("{0}://{1}:{2}/{4}/?EIO=2&transport=websocket&sid={3}",
			                uri.Scheme == Uri.UriSchemeHttps ? "wss" : "ws", uri.Host, uri.Port, HandShake.SID,"socket.io");

			            Debug.Log("Connect request: " + socketUri);
			            this.wsClient = new WebSocket(socketUri,
//								string.Format("{0}://{1}:{2}/socket.io/1/websocket/{3}", wsScheme, uri.Host, uri.Port, this.HandShake.SID),
			                string.Empty,
			                this.socketVersion);

			            this.wsClient.EnableAutoSendPing = true;
			                // #4 tkiley: Websocket4net client library initiates a websocket heartbeat, causes delivery problems
			            this.wsClient.Opened += this.wsClient_OpenEvent;
                        this.wsClient.MessageReceived += wsClient_MessageReceived;
			            this.wsClient.Error += this.wsClient_Error;
			            this.wsClient.Closed += wsClient_Closed;

			            this.wsClient.Open();
			        }
			        else
			        {
			            Debug.Log("Something went wrong with the handshake.");
			            this.LastErrorMessage = string.Format("Error initializing handshake with {0}", uri.ToString());
			            this.OnErrorEvent(this, new ErrorEventArgs(this.LastErrorMessage, new Exception()));
			        }
			    }
			    catch (Exception ex)
			    {
			        Debug.Log(string.Format("Connect threw an exception...{0}", ex));
			        this.OnErrorEvent(this, new ErrorEventArgs("SocketIO.Client.Connect threw an exception", ex));
			    }
			}
		}

		public IEndPointClient Connect(string endPoint)
		{
            Debug.Log("Connect");
			var nsClient = new EndPointClient(this, endPoint);
			Connect();
			Send(new ConnectMessage(endPoint));
			return nsClient;
		}

		protected void ReConnect()
		{
            Debug.Log("ReConnect");
			retryConnectionCount++;

			OnConnectionRetryAttemptEvent(this, EventArgs.Empty);

			closeHeartBeatTimer(); // stop the heartbeat time
			closeWebSocketClient();// stop websocket

			this.Connect();

			var connected = ConnectionOpenEvent.WaitOne(4000); // block while waiting for connection
			Debug.Log(string.Format("\tRetry-Connection successful: {0}", connected));
			if (connected)
				retryConnectionCount = 0;
			else
			{	// we didn't connect - try again until exhausted
				if (retryConnectionCount < RetryConnectionAttempts)
				{
					ReConnect();
				}
				else
				{
					Close();
					OnSocketConnectionClosedEvent(this, EventArgs.Empty);
				}
			}
		}
		
		/// <summary>
		/// <para>Asynchronously calls the action delegate on event message notification</para>
		/// <para>Mimicks the Socket.IO client 'socket.on('name',function(data){});' pattern</para>
		/// <para>Reserved socket.io event names available: connect, disconnect, open, close, error, retry, reconnect  </para>
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="action"></param>
		/// <example>
		/// client.On("testme", (data) =>
		///    {
		///        Debug.WriteLine(data.ToJson());
		///    });
		/// </example>
		public virtual void On(
			string eventName,
			Action<IMessage> action)
        {
            Debug.Log("On " + eventName);
			this.registrationManager.AddOnEvent(eventName, action);
		}
		public virtual void On(
			string eventName,
			string endPoint,
			Action<IMessage> action)
		{

            Debug.Log("On " + eventName);
			this.registrationManager.AddOnEvent(eventName, endPoint, action);
		}
		/// <summary>
		/// <para>Asynchronously sends payload using eventName</para>
		/// <para>payload must a string or Json Serializable</para>
		/// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
		/// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="payload">must be a string or a Json Serializable object</param>
		/// <remarks>ArgumentOutOfRangeException will be thrown on reserved event names</remarks>
		public void Emit(string eventName, Object payload, string endPoint , Action<Object>  callback)
		{
            Debug.Log("Emit2");
			string lceventName = eventName.ToLower();
			IMessage msg = null;
			switch (lceventName)
			{
				case "message":
					if (payload is string)
						msg = new TextMessage() { MessageText = payload.ToString() };
					else
						msg = new JSONMessage(payload);
					this.Send(msg);
					break;
				case "connect":
				case "disconnect":
				case "open":
				case "close":
				case "error":
				case "retry":
				case "reconnect":
					throw new System.ArgumentOutOfRangeException(eventName, "Event name is reserved by socket.io, and cannot be used by clients or servers with this message type");
				default:
					if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("/"))
						endPoint = "/" + endPoint;
					msg = new EventMessage(eventName, payload, endPoint, callback);
					if (callback != null)
						this.registrationManager.AddCallBack(msg);

					this.Send(msg);
					break;
			}
		}

		/// <summary>
		/// <para>Asynchronously sends payload using eventName</para>
		/// <para>payload must a string or Json Serializable</para>
		/// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
		/// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="payload">must be a string or a Json Serializable object</param>
		public void Emit(string eventName, Object payload)
        {
            Debug.Log("Emit");
			this.Emit(eventName, payload, string.Empty, null);
		}

		/// <summary>
		/// Queue outbound message
		/// </summary>
		/// <param name="msg"></param>
		public void Send(IMessage msg)
        {
            Debug.Log("Send");
			this.MessageQueueEmptyEvent.Reset();
			if (this.outboundQueue != null)
				this.outboundQueue.Enqueue(msg.Encoded);
		}
		
		public void Send(string msg) {

            Debug.Log("Send2");
			IMessage message = new TextMessage() { MessageText =  msg };
			Send(message);
		}

		private void Send_backup(string rawEncodedMessageText)
        {
            Debug.Log("Send_backup");
			this.MessageQueueEmptyEvent.Reset();
			if (this.outboundQueue != null)
				this.outboundQueue.Enqueue(rawEncodedMessageText);
		}

		/// <summary>
		/// if a registerd event name is found, don't raise the more generic Message event
		/// </summary>
		/// <param name="msg"></param>
		protected void OnMessageEvent(IMessage msg)
		{
            Debug.Log("OnMessageEvent " + msg.MessageText + " " + msg.Json.ToJsonString() + " " + msg.RawMessage);
			bool skip = false;
			if (!string.IsNullOrEmpty(msg.Event))
				skip = this.registrationManager.InvokeOnEvent(msg); // 

			var handler = this.Message;
			if (handler != null && !skip)
			{
				Debug.Log(string.Format("webSocket_OnMessage: {0}", msg.RawMessage));
				handler(this, new MessageEventArgs(msg));
			}
		}
		
		/// <summary>
		/// Close SocketIO4Net.Client and clear all event registrations 
		/// </summary>
		public void Close()
        {
            Debug.Log("Close");
			this.retryConnectionCount = 0; // reset for next connection cycle
			// stop the heartbeat time
			this.closeHeartBeatTimer();

			// stop outbound messages
			this.closeOutboundQueue();

			this.closeWebSocketClient();

			if (this.registrationManager != null)
			{
				this.registrationManager.Dispose();
				this.registrationManager = null;
			}

		}

		protected void closeHeartBeatTimer()
        {
            Debug.Log("closeHeartBeatTimer");
			// stop the heartbeat timer
			if (this.socketHeartBeatTimer != null)
			{
				this.socketHeartBeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
				this.socketHeartBeatTimer.Dispose();
				this.socketHeartBeatTimer = null;
			}
		}
		protected void closeOutboundQueue()
        {
            Debug.Log("closeOutboundQueue");
			// stop outbound messages
			if (this.outboundQueue != null)
			{
				//this.outboundQueue.TryDequeue(); // stop adding any more items;
				//this.dequeuOutBoundMsgTask.Wait(700); // wait for dequeue thread to stop
				//this.outboundQueue = n
				this.outboundQueue = null;
			}
		}
		protected void closeWebSocketClient()
		{
            Debug.Log("closeWebSocketClient");
			if (this.wsClient != null)
			{
				// unwire events
				this.wsClient.Closed -= this.wsClient_Closed;
				this.wsClient.MessageReceived -= wsClient_MessageReceived;
				this.wsClient.Error -= wsClient_Error;
				this.wsClient.Opened -= this.wsClient_OpenEvent;

				if (this.wsClient.State == WebSocketState.Connecting || this.wsClient.State == WebSocketState.Open)
				{
					try { this.wsClient.Close(); }
					catch { Debug.Log("exception raised trying to close websocket: can safely ignore, socket is being closed"); }
				}
				this.wsClient = null;
			}
		}

		// websocket client events - open, messages, errors, closing
		private void wsClient_OpenEvent(object sender, EventArgs e)
		{
            Debug.Log("wsClient_OpenEvent " + e.ToString());

			this.socketHeartBeatTimer = new Timer(OnHeartBeatTimerCallback, new object(), HandShake.HeartbeatInterval, HandShake.HeartbeatInterval);
			this.ConnectionOpenEvent.Set();

			this.OnMessageEvent(new EventMessage() { Event = "open" });
			if (this.Opened != null)
			{
				try { this.Opened(this, EventArgs.Empty); }
				catch (Exception ex) { Debug.Log(ex); }
			}

		}

		/// <summary>
		/// Raw websocket messages from server - convert to message types and call subscribers of events and/or callbacks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wsClient_MessageReceived(object sender, MessageReceivedEventArgs e)
		{

            Debug.Log("wsClient_MessageReceived " + e.Message);
			IMessage iMsg = SocketIOClient.Messages.Message.Factory(e.Message);
			
			if (iMsg.Event == "responseMsg")
				Debug.Log(string.Format("InvokeOnEvent: {0}", iMsg.RawMessage));
			switch (iMsg.MessageType)
			{
				case SocketIOMessageTypes.Disconnect:
					this.OnMessageEvent(iMsg);
					if (string.IsNullOrEmpty(iMsg.Endpoint)) // Disconnect the whole socket
						this.Close();
					break;
//				case SocketIOMessageTypes.Heartbeat:
//					this.OnHeartBeatTimerCallback(null);
//					break;
				case SocketIOMessageTypes.Connect:
//				case SocketIOMessageTypes.Message:
//				case SocketIOMessageTypes.JSONMessage:
				case SocketIOMessageTypes.Event:
				case SocketIOMessageTypes.Error:
					this.OnMessageEvent(iMsg);
					break;
				case SocketIOMessageTypes.ACK:
					this.registrationManager.InvokeCallBack(iMsg.AckId, iMsg.Json);
					break;
				default:
					Debug.Log("unknown wsClient message Received...");
					break;
			}
		}

		/// <summary>
		/// websocket has closed unexpectedly - retry connection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wsClient_Closed(object sender, EventArgs e)
		{
            Debug.Log("wsClient_Closed");
			if (this.retryConnectionCount < this.RetryConnectionAttempts   )
			{
				this.ConnectionOpenEvent.Reset();
				this.ReConnect();
			}
			else
			{
				this.Close();
				this.OnSocketConnectionClosedEvent(this, EventArgs.Empty);
			}
		}

		private void wsClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.Log("wsClient_Error");
			this.OnErrorEvent(sender, new ErrorEventArgs("SocketClient error", e.Exception));
		}

		protected void OnErrorEvent(object sender, ErrorEventArgs e)
        {
            Debug.Log("OnErrorEvent");
			this.LastErrorMessage = e.Message;
			if (this.Error != null)
			{
				try { this.Error.Invoke(this, e); }
				catch { }
			}
			Debug.Log(string.Format("Error Event: {0}\r\n\t{1}", e.Message, e.Exception));
		}
		protected void OnSocketConnectionClosedEvent(object sender, EventArgs e)
        {
            Debug.Log("OnSocketConnectionClosedEvent");
			if (this.SocketConnectionClosed != null)
				{
					try { this.SocketConnectionClosed(sender, e); }
					catch { }
				}
			Debug.Log("SocketConnectionClosedEvent");
		}
		protected void OnConnectionRetryAttemptEvent(object sender, EventArgs e)
        {
            Debug.Log("OnConnectionRetryAttemptEvent2");
			if (this.ConnectionRetryAttempt != null)
			{
				try { this.ConnectionRetryAttempt(sender, e); }
				catch (Exception ex) { Debug.Log(ex); }
			}
			Debug.Log(string.Format("Attempting to reconnect: {0}", this.retryConnectionCount));
		}

		// Housekeeping
		protected void OnHeartBeatTimerCallback(object state)
        {
            Debug.Log("OnHeartBeatTimerCallback");
			if (this.ReadyState == WebSocketState.Open)
			{
				IMessage msg = new Heartbeat();
				try
				{
					if (this.outboundQueue != null)
					{
						this.outboundQueue.Enqueue(msg.Encoded);
						if (this.HeartBeatTimerEvent != null)
						{
							this.HeartBeatTimerEvent.BeginInvoke(this, EventArgs.Empty, EndAsyncEvent, null);
						}
					}
				}
				catch(Exception ex)
				{
					// 
					Debug.Log(string.Format("OnHeartBeatTimerCallback Error Event: {0}\r\n\t{1}", ex.Message, ex.InnerException));
				}
			}
		}
		private void EndAsyncEvent(IAsyncResult result)
        {
            Debug.Log("EndAsyncEvent");
			var ar = (System.Runtime.Remoting.Messaging.AsyncResult)result;
			var invokedMethod = (EventHandler)ar.AsyncDelegate;

			try
			{
				invokedMethod.EndInvoke(result);
			}
			catch
			{
				// Handle any exceptions that were thrown by the invoked method
				Debug.Log("An event listener went kaboom!");
			}
		}
		/// <summary>
		/// While connection is open, dequeue and send messages to the socket server
		/// </summary>
		protected void dequeuOutboundMessages()
        {
            Debug.Log("dequeuOutboundMessages");
			while (this.outboundQueue != null)
			{
				if (this.ReadyState == WebSocketState.Open)
				{
					string msgString;
					try
					{
						if (this.outboundQueue.TryDequeue(out msgString))
						{
							this.wsClient.Send(msgString);
						}
						else
							this.MessageQueueEmptyEvent.Set();
					}
					catch(Exception ex)
					{
						Debug.Log("The outboundQueue is no longer open...");
					}
				}
				else
				{
					this.ConnectionOpenEvent.WaitOne(2000); // wait for connection event
				}
			}
		}

	    /// <summary>
	    /// <para>Client performs an initial HTTP POST to obtain a SessionId (sid) assigned to a client, followed
	    ///  by the heartbeat timeout, connection closing timeout, and the list of supported transports.</para>
	    /// <para>The tansport and sid are required as part of the ws: transport connection</para>
	    /// </summary>
	    /// <param name="uri">http://localhost:3000</param>
	    /// <returns>Handshake object with sid value</returns>
        /// <example>DownloadString socket.io 0.9: 13052140081337757257:15:25:websocket,htmlfile,xhr-polling,jsonp-polling</example>  
        /// <example>DownloadString socket.io 1.0.6: 97:0{"sid":"_IowAnL1F3DjC3sRAAAF","upgrades":["websocket"],"pingInterval":25000,"pingTimeout":60000}</example> 
	    protected SocketIOHandshake requestHandshake(Uri uri)
	    {
	        var response = string.Empty;
	        var errorText = string.Empty;

	        using (var client = new WebClient())
	        {
	            try
	            {
//                    var query = "&t=" + Math.Round((DateTimeOffset.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds, 0);
	                
                    var request = string.Format("{0}://{1}:{2}/{4}/?EIO=3&transport=polling&b64=0{3}", uri.Scheme, uri.Host, uri.Port, uri.Query, "socket.io");
                    response = client.DownloadString(request);
                    //            var handshakeUrl = string.Format("{0}://{1}:{2}/{4}/1/{3}", uri.Scheme, uri.Host, uri.Port, query, resource);
//					value = client.DownloadString(string.Format("{0}://{1}:{2}/socket.io/1/{3}", uri.Scheme, uri.Host, uri.Port, uri.Query)); // #5 tkiley: The uri.Query is available in socket.io's handshakeData object during authorization
                    Debug.Log("Handshake Request: " + request);
                    Debug.Log("Handshake Response: " + response);

	                // 13052140081337757257:15:25:websocket,htmlfile,xhr-polling,jsonp-polling
	                if (string.IsNullOrEmpty(response))
	                    errorText = "Did not receive handshake string from server";
	            }
	            catch (Exception ex)
	            {
	                Debug.Log(ex);
	                errorText = string.Format("Error getting handsake from Socket.IO host instance: {0}", ex.Message);
	                //this.OnErrorEvent(this, new ErrorEventArgs(errMsg));
	            }
	        }
			return string.IsNullOrEmpty(errorText) ? SocketIOHandshake.LoadFromString(response) : new SocketIOHandshake {ErrorMessage = errorText};
		}

		public void Dispose()
		{
            dequeuOutBoundMsgTask.Abort();
			Dispose(true);
//			GC.SuppressFinalize(this);
		}

		// The bulk of the clean-up code 
		protected virtual void Dispose(bool disposing)
		{
		    if (!disposing) return;
		    // free managed resources
		    this.Close();
		    this.MessageQueueEmptyEvent.Close();
		    this.ConnectionOpenEvent.Close();
		}
	}
}
