// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network is connected.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W905")]
	public class PhotonNetworkGetIsConnected : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network is connected.")]
		public FsmBool isConnected;
		
		[Tooltip("Send this event if the Photon network is connected.")]
		public FsmEvent isConnectedEvent;
		
		[Tooltip("Send this event if the Photon network is NOT connected.")]
		public FsmEvent isNotConnectedEvent;
		
		public override void Reset()
		{
			isConnected = null;
			isConnectedEvent = null;
			isNotConnectedEvent = null;
		}

		public override void OnEnter()
		{
			checkIsConnected();
			
			Finish();
		}
		
		void checkIsConnected()
		{
			bool _isConnected = PhotonNetwork.connected;
			isConnected.Value = _isConnected;
			
			if (_isConnected )
			{
				if (isConnectedEvent!=null)
				{
					Fsm.Event(isConnectedEvent);
				}
			}
			else if (isNotConnectedEvent!=null)
			{
				Fsm.Event(isNotConnectedEvent);
			}
		}

	}
}