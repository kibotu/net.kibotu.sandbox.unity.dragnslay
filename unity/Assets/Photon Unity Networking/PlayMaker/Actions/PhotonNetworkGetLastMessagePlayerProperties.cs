// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the player properties of the last photon message (OnPhotonPlayerConnected, OnPhotonPlayerDisconnected, OnPhotonPlayerPropertiesChanged or OnMasterClientSwitched).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W906")]
	public class PhotonNetworkGetLastMessagePLayerProperties : FsmStateAction
	{
		
		[ActionSection("Player Properties")] 
		
		[Tooltip("The Photon player name")]
		[UIHint(UIHint.Variable)]
		public FsmString name;
		
		[Tooltip("The Photon player ID")]
		[UIHint(UIHint.Variable)]
		public FsmInt ID;
		
		[Tooltip("The Photon player isLocal property")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLocal;
		
		[Tooltip("The Photon player isLocal isMasterClient")]
		[UIHint(UIHint.Variable)]
		public FsmBool isMasterClient;

		
		[Tooltip("Custom Properties you have assigned to this player.")]
		[CompoundArray("player Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmVar[] customPropertiesValues;
		
		[ActionSection("Events")] 
		
		[Tooltip("Send this event if the player was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if no player was found.")]
		public FsmEvent failureEvent;
			
		
		public override void Reset()
		{
			name = null;
			ID = null;
			isLocal = null;
			isMasterClient = null;
			successEvent = null;
			failureEvent = null;
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
			
		}

		public override void OnEnter()
		{
			bool ok;
			ok =getLastMessagePlayerProperties();
			
			if (ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}
			Finish();
		}

		bool getLastMessagePlayerProperties()
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
			
			PhotonPlayer _player = _proxy.lastMessagePhotonPlayer;
			if (_player==null)
			{
				return false;
			}
			
			name.Value = _player.name;
			ID.Value   = _player.ID;
			isLocal.Value = _player.isLocal;
			isMasterClient.Value = _player.isMasterClient;
			
			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (!_player.customProperties.ContainsKey(key.Value))
				{
					return false;
				}
				PlayMakerPhotonProxy.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_player.customProperties[key.Value]);
				i++;
			}
			
			return true;
		}
	
		
		
	}
}