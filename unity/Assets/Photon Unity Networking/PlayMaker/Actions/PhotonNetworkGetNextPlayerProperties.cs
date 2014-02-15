// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Let's you loop through the Players in the room. This action works only when you are in a room.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1112")]
	public class PhotonNetworkGetNextPlayerProperties : FsmStateAction
	{
		[ActionSection("set up")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Include the owner player in the list, else list only other players")]
		public FsmBool includeSelf;
		
		[ActionSection("player properties")]
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The player index in the list. Do not rely on that, this can change anytime.")]
		public FsmInt playerListIndex;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The player name.")]
		public FsmString playerName;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Identifier of this player in current room.")]
		public FsmInt playerID;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Only one player is controlled by each client. Others are not local.")]
		public FsmBool playerIsLocal;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The player with the lowest actorID is the master and could be used for special tasks.")]
		public FsmBool playerIsMasterClient;
		
		[ActionSection("player custom properties")]
		
		[Tooltip("Custom Properties you have assigned to this player.")]
		[CompoundArray("player Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[Tooltip("Values related to the properties")]
		[UIHint(UIHint.Variable)]
		public FsmVar[] customPropertiesValues;
		
		
		[ActionSection("Events")] 
		
		[Tooltip("Event to send if we are not in a room. We can only get the list of players if we are in a room")]
		public FsmEvent notInRoomEvent;
		
		[RequiredField]
		[Tooltip("Event to send to get the next player.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send if there is no players at all")]
		public FsmEvent noPlayersEvent;
		
		[RequiredField]
		[Tooltip("Event to send when there are no more players to loop.")]
		public FsmEvent finishedEvent;

		public override void Reset()
		{
			includeSelf = false;
			
			playerListIndex = null;
			playerName = null;
			playerID = null;
			playerIsLocal = null;
			playerIsMasterClient = null;
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
			
			notInRoomEvent = null;
			loopEvent = null;
			finishedEvent = null;
			noPlayersEvent = null;
		}
		
		// cache the rooms
		private PhotonPlayer[] players;
		
		// increment a room index as we loop through the hits
		private int nextPlayerIndex;
		private PhotonPlayer _player;
		
		public override void OnEnter()
		{
			
			//check if we are in a room or not
			if (PhotonNetwork.room == null)
			{
				Fsm.Event(notInRoomEvent);
				Finish();
				return; 
			}
				
			if (nextPlayerIndex==0)
			{
				if (includeSelf.Value)
				{
					players = PhotonNetwork.playerList;
				}else{
					players = PhotonNetwork.otherPlayers;
				}
			}
			
			if (players.Length==0)
			{
				nextPlayerIndex = 0;
				Fsm.Event(noPlayersEvent);
				Fsm.Event(finishedEvent);
				Finish();
				return;
			}
			
			if (nextPlayerIndex>=players.Length)
			{
				nextPlayerIndex = 0;
				Fsm.Event(finishedEvent);
				Finish();
				return;
			}
			
			_player = players[nextPlayerIndex];
			
			// we get the player properties
			playerID.Value	= _player.ID;
			playerIsLocal.Value =	_player.isLocal;
			playerName.Value = _player.name;
			playerIsMasterClient = _player.isMasterClient;
			
			
			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				PlayMakerPhotonProxy.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_player.customProperties[key.Value]);
				i++;
			}
			
			
			nextPlayerIndex++;
			
			Fsm.Event(loopEvent);
			
			Finish();
		}
	}
}