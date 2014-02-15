// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Delete the named custom property of this Room.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1116")]
	public class PhotonNetworkDeleteRoomCustomProperty : FsmStateAction
	{
		
		[Tooltip("The custom property to delete")]
		public FsmString customPropertyKey;
		
		[Tooltip("Send this event if the custom property was deleted")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the custom property deletion failed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			DeleteRoomProperty();
			
			Finish();
		}
		
		void DeleteRoomProperty()
		{
			Room _room = PhotonNetwork.room;
			bool _isInRoom = _room!=null;
			
			if (!_isInRoom )
			{
				Fsm.Event(failureEvent);	
				return;
			}
			
			Hashtable _prop = new Hashtable();
			
			_prop[customPropertyKey.Value] = null;
			_room.SetCustomProperties(_prop);
			
			Fsm.Event(successEvent);
		}

	}
}