using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.network.googleplayservice
{
    public class MultiplayerListenerRTS : RealTimeMultiplayerListener
    {
        #region Connection Type

        public enum ConnectionType
        {
            TCP, UDP
        }

        public ConnectionType Type { get; set; }

        public bool UseReliableMessages()
        {
            return Type == ConnectionType.TCP;
        }

        #endregion

        #region Delegates

        public Action<float> RoomSetupProgress;
		public Action<bool> RoomConnected;
		public Action LeftRoom;
		public Action<string[]> PeersConnected;
		public Action<string[]> PeersDisconnected;
        public Action<JObject, string> RealTimeMessageReceived;

        #endregion

        #region Forward network events

        public void OnRoomSetupProgress(float percent)
        {
            Debug.Log("OnRoomSetupProgress: " + percent);
			if(RoomSetupProgress != null) RoomSetupProgress (percent);
        }

        public void OnRoomConnected(bool success)
        {
            Debug.Log("OnRoomConnected: " + success);
			if(RoomConnected != null) RoomConnected(success);
        }

        public void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
			if(LeftRoom != null) LeftRoom ();
        }

        public void OnPeersConnected(string[] participantIds)
        {
           	Debug.Log("OnRoomConnected: " + participantIds.Count());
			if(PeersConnected != null) PeersConnected (participantIds);
        }

        public void OnPeersDisconnected(string[] participantIds)
        {
            Debug.Log("OnPeersDisconnected: " + participantIds.Count());
			if(PeersDisconnected != null) PeersDisconnected (participantIds);
        }

        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
		{
            // Reliable messaging. With reliable messaging, data delivery, integrity, and ordering are guaranteed. 
            // You can choose to be notified of the delivery status by using a callback. 
            // Reliable messaging is suitable for sending non-time-sensitive data. You can also use reliable messaging to send large
            // data sets where the data can be split into smaller segments, sent over the network, and then reassembled by the receiving client.
            // Reliable messaging might have high latency. The maximum size of a reliable message that you can send is 1400 bytes.
            // 
            // Unreliable messaging. The game client sends the data only once ('fire-and-forget') with no guarantee of data delivery or data arriving in order. 
            // However, integrity is guaranteed, so there is no need to add a checksum. Unreliable messaging has low latency and is suitable for sending 
            // data that is time-sensitive. Your app is responsible for ensuring that the game behaves correctly if messages are dropped in transmission 
            // or received out of order. The maximum size for an unreliable message that you can send is 1168 bytes.

            // care for order
            
			var currentMessage = ToJObject (data);
            // 1) acknowledge package

            Debug.Log("Received: reliably: " + isReliable + " senderId: " + senderId + " bytes: " + data.Count() + " msg: " + currentMessage + " ack: " + currentMessage["message"].Equals("acknowledged"));

            if (currentMessage["message"].Equals("acknowledged"))
            {
                // forward acknowledged message
                var json = PackageDameon.Unverified.Acknowledge(currentMessage);
                if (RealTimeMessageReceived != null && json != null)
                    RealTimeMessageReceived(json, senderId);
            }
            else
            {
                // 1) send acknowledge message
                BroadcastMessage(PackageFactory.CreateReceivedMessage(currentMessage["packageId"].ToObject<int>(), currentMessage["scheduleId"].ToObject<int>()));

                // 2) put into unverified list
                PackageDameon.Unverified.Add(currentMessage);
            }
        }

        #endregion

        #region send message

        public const int MaxBytesUnreliable = 1168;
        public const int MaxBytesReliable = 1400;

        public static void CheckMaxBytesCount(byte[] bytes, bool useReliable)
        {
            var byteCount = bytes.Count();
            if (useReliable)
            {
                if (byteCount >= MaxBytesReliable)
                    Debug.LogWarning("Send message is bigger than GPS supported maximum package size: " + byteCount +
                                     "/" + MaxBytesReliable + " bytes");
            }
            else
            {
                if (byteCount >= MaxBytesUnreliable)
                    Debug.LogWarning("Send message is bigger than GPS supported maximum package size: " + byteCount +
                                     "/" + MaxBytesUnreliable + " bytes");
            }
        }

        public void BroadcastMessage(JObject message)
        {
            if (!Social.localUser.authenticated)
                return;

            // 1) check max bytes count
            var bytes = ToBytes(message.ToString());
            var useReliable = UseReliableMessages();
            CheckMaxBytesCount(bytes, useReliable);

            // 2) remember message for later verification subsequential execution
            PackageDameon.Unverified.Add(message);

            Debug.Log("Send: reliably: " + useReliable + " bytes: " + bytes.Count() + " msg: " + message);

            // 3) actually send message
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(useReliable, bytes);
        }

        #endregion

        #region Serialization

        public static JObject ToJObject(byte[] data)
        {
            return JObject.Parse(GetString(data));
        }

        public static byte[] ToBytes(string str)
        {
            str = Zipper.ZipString(str);
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return Zipper.UnzipString(new string(chars));
        }

        #endregion
    }
}
