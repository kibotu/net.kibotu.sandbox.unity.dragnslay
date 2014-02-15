// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Create a room With advanced settings.Please refer to Photon documentation.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1134")]
	public class PhotonNetworkCreateRoomAdvanced : FsmStateAction
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[Tooltip("Is the room visible")]
		public FsmBool isVisible;
		
		[Tooltip("Is the room open")]
		public FsmBool isOpen;
			
		[Tooltip("Max numbers of players for this room.")]
		public FsmInt maxNumberOfPLayers;
		
		[ActionSection("Custom Properties")]
		
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("The Custom Property to set")]
		public FsmString[] customPropertyKey;
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar[] customPropertyValue;
		
		[ActionSection("Lobby custom Properties")]
		[Tooltip("Properties listed in the lobby.")]
		public FsmString[] lobbyCustomProperties;
		
		public override void Reset()
		{
			roomName  = new FsmString() {UseVariable=true};
			isVisible = true;
			isOpen = true;
			maxNumberOfPLayers = 100;
			customPropertyKey = null;
			customPropertyValue = null;
			lobbyCustomProperties = null;
		}

		public override void OnEnter()
		{
			
		
			string _roomName = null;
			if ( ! string.IsNullOrEmpty(roomName.Value) )
			{
				_roomName = roomName.Value;
			}
				

			Hashtable _props = new Hashtable();
			
			int i = 0;
			foreach(FsmString _prop in customPropertyKey)
			{
				_props[_prop.Value] =  PlayMakerPhotonProxy.GetValueFromFsmVar(this.Fsm,customPropertyValue[i]);
				i++;
			}
			
			
			string[] lobbyProps = new string[lobbyCustomProperties.Length];
			
			int j = 0;
			foreach(FsmString _visibleProp in lobbyCustomProperties)
			{
				lobbyProps[j] = _visibleProp.Value;
				j++;
			}

			PhotonNetwork.CreateRoom(_roomName,isVisible.Value,isOpen.Value,maxNumberOfPLayers.Value,_props,lobbyProps);
			
			
			Finish();
		}

	}
}