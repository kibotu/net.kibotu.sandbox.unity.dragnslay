// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Let's you loop through the available Photon rooms. This action works only when you are in the Lobby, use PhotonNetworkGetRoomProperties when you are in a room")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1115")]
	public class PhotonNetworkGetNextRoomProperties : FsmStateAction
	{
		
		[ActionSection("room properties")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("the room index in the list.")]
		public FsmInt roomListIndex;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("the name of the room.")]
		public FsmString RoomName;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("the number of players in the room.")]
		public FsmInt playerCount;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The limit of players to this room. This property is shown in lobby, too.\n" +
		 	"If the room is full (players count == maxplayers), joining this room will fail..")]
		public FsmInt maxPlayers;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Defines if the room can be joined. If not open, the room is excluded from random matchmaking. \n" +
			"This does not affect listing in a lobby but joining the room will fail if not open.")]
		public FsmBool open;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Defines if the room is listed in its lobby.")]
		public FsmBool visible;
		
		[Tooltip("Custom Properties you have assigned to this room.")]
		[CompoundArray("room Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		[Tooltip("Values of each properties")]
		public FsmVar[] customPropertiesValues;
		
		[ActionSection("Events")] 
		
		[Tooltip("Event to send if we are not in the lobby. We can only get the list of rooms if we are in the lobby")]
		public FsmEvent notInLobbyEvent;
		
		[RequiredField]
		[Tooltip("Event to send to get the next room.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send if there is no rooms at all")]
		public FsmEvent noRoomsEvent;
		
		[RequiredField]
		[Tooltip("Event to send when there are no more rooms to loop.")]
		public FsmEvent finishedEvent;

		public override void Reset()
		{
			roomListIndex = null;
			RoomName = null;
			maxPlayers = null;
			open = null;
			visible = null;
			playerCount = 0;
	
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
			
			notInLobbyEvent = null;
			loopEvent = null;
			finishedEvent = null;
			noRoomsEvent = null;
		}
		
		// cache the rooms
		private RoomInfo[] rooms;
		
		// increment a room index as we loop through the hits
		private int nextRoomIndex;
		private RoomInfo _room;
		
		public override void OnEnter()
		{
			
			//check if we are in a room or not
			//if (PhotonNetwoek.is)
			if (!PhotonNetwork.insideLobby)
			{
				Fsm.Event(notInLobbyEvent);
				Finish();
				return; 
			}
				
			if (nextRoomIndex==0)
			{
				rooms = PhotonNetwork.GetRoomList();
			}
			
			if (rooms.Length==0)
			{
				nextRoomIndex = 0;
				Fsm.Event(noRoomsEvent);
				Fsm.Event(finishedEvent);
				Finish();
				return;
			}
			
			if (nextRoomIndex>=rooms.Length)
			{
				nextRoomIndex = 0;
				Fsm.Event(finishedEvent);
				Finish();
				return;
			}
			
			_room = rooms[nextRoomIndex];
			
			// we get the room properties
			RoomName.Value = _room.name;
			maxPlayers.Value = _room.maxPlayers;
			open.Value = _room.open;
			visible.Value = _room.visible;
			playerCount.Value = _room.playerCount;
			
			
			
			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (_room.customProperties.ContainsKey(key.Value))
				{
					PlayMakerPhotonProxy.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_room.customProperties[key.Value]);
				}
				i++;
			}
			
			
			nextRoomIndex++;
			
			Fsm.Event(loopEvent);
			
			Finish();
		}
	}
}