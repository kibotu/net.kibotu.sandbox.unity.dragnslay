// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Updates and synchronizes the named custom property of this Room. New properties are added, existing values are updated.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1114")]
	public class PhotonNetworkSetRoomCustomProperty : FsmStateAction
	{
		[Tooltip("The Custom Property to set or update")]
		public FsmString customPropertyKey;
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar customPropertyValue;
		
		[Tooltip("Send this event if the custom property was set or update.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the custom property set failed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			customPropertyValue = null;
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			SetRoomProperty();
			
			Finish();
		}
		
		void SetRoomProperty()
		{
			Room _room = PhotonNetwork.room;
			bool _isInRoom = _room!=null;
			
			if (!_isInRoom )
			{
				Fsm.Event(failureEvent);	
				return;
			}
			
			Hashtable _prop = new Hashtable();
			
			_prop[customPropertyKey.Value] =  PlayMakerPhotonProxy.GetValueFromFsmVar(this.Fsm,customPropertyValue);
			_room.SetCustomProperties(_prop);
			Fsm.Event(successEvent);
		}

	}
}