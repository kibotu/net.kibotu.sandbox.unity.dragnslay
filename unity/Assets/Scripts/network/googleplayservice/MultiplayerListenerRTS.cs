using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.menu;
using Assets.Sources.utility;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.network.googleplayservice
{
    public class MultiplayerListenerRTS : RealTimeMultiplayerListener
    {
		public Action<float> RoomSetupProgress;
		public Action<bool> RoomConnected;
		public Action LeftRoom;
		public Action<string[]> PeersConnected;
		public Action<string[]> PeersDisconnected;
		public Action<JObject, string, bool> RealTimeMessageReceived;

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
			var json = GooglePlayServiceHelper.ToJObject (data);
			Debug.Log("OnRealTimeMessageReceived: reliably: " + isReliable + " senderId: " + senderId + " bytes: " + data.Count() + " msg: " + json);
			if(RealTimeMessageReceived != null) RealTimeMessageReceived(json, senderId, isReliable);
        }
    }
}
