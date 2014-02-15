// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Create a room with given title. This will fail if the room title is already in use.\n" +
		"Note: If you don't want to create a unique room-name yourself, leave the room name to empty, and the server will assign a roomName (a GUID as string).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W903")]
	public class PhotonNetworkCreateRoom : FsmStateAction
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[Tooltip("Is the room visible")]
		public FsmBool isVisible;
		
		[Tooltip("Is the room open")]
		public FsmBool isOpen;
			
		[Tooltip("Max numbers of players for this room.")]
		public FsmInt maxNumberOfPLayers;
		
		
		
		public override void Reset()
		{
			roomName  = null;
			isVisible = true;
			isOpen = true;
			maxNumberOfPLayers = 100;
		}

		public override void OnEnter()
		{
			
			
			
			byte max = (byte)maxNumberOfPLayers.Value;
		
			if (string.IsNullOrEmpty(roomName.Value))
			{
				PhotonNetwork.CreateRoom(null,isVisible.Value,isOpen.Value,max);
			}else{
				PhotonNetwork.CreateRoom(roomName.Value,isVisible.Value,isOpen.Value,max);
			}
			
			Finish();
		}

	}
}