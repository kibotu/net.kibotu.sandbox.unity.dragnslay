// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the disconnect cause of the last photon message (OnConnectionFail, OnFailedToConnectToPhoton).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1137")]
	public class PhotonNetworkGetLastDisconnectionCause : FsmStateAction
	{
				
		[Tooltip("The disconnect cause")]
		[UIHint(UIHint.Variable)]
		public FsmString cause;
		
		
		[Tooltip("Send this event if the disconnection cause was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the disconnection cause was not found.")]
		public FsmEvent failureEvent;
		
		public override void Reset()
		{
			cause = null;
			
			successEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			bool ok = getLastDisconnectCause();

			if (ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}
			Finish();
		}

		bool getLastDisconnectCause()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return false;
			}
			
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				
				return false;
			}
			
			cause.Value = _proxy.lastDisconnectCause.ToString();
			
			return true;
		}
	
		
		
	}
}