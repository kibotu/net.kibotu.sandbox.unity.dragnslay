// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you send a Fsm Event to a specific photon player.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W921")]
	public class PhotonViewRpcBroadcastFsmEventToPlayer : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The targeted player name.")]
		[UIHint(UIHint.Variable)]
		public FsmString targetPlayerName;
		
		//JFF: TOFIX: SHOULD NOT BE PUBLIC, BUT I NEED THIS TO DISPLAY GLOBAL EVENTS 
		[Tooltip("Leave to BroadCastAll.")]
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optionnal string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		[UIHint(UIHint.FsmString)]
		public FsmString stringData;
		
		
		public override void Reset()
		{
			targetPlayerName = null;
	
			// JFF: how can I set this silently without a plubic variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			remoteEvent = null;
			stringData = null;
		}

		public override void OnEnter()
		{
			DoREC();
			
			Finish();
		}

		void DoREC()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return;
			}
			
			if (remoteEvent.Name ==""){
				return;
			}
			
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				return;
			}
			
			PhotonPlayer _target = getPhotonPLayerTarget();
			if (_target.ID == -1){
				return;
			}
		
			if (! stringData.IsNone && stringData.Value != ""){
				_proxy.PhotonRpcFsmBroadcastEventWithString(_target,remoteEvent.Name,stringData.Value);
			}else{
				_proxy.PhotonRpcBroadcastFsmEvent(_target,remoteEvent.Name);
			}
			
		}
		
		PhotonPlayer getPhotonPLayerTarget()
		{
			if ( targetPlayerName.IsNone || targetPlayerName.Value == "")
			{
 				return null; 
			} 
			
			string _name = targetPlayerName.Value;
			
			foreach(PhotonPlayer _player in PhotonNetwork.otherPlayers)
			{
				if ( _name.Equals(_player.name))
				{
					return _player;
				}
			}
			return new PhotonPlayer(true,-1,"invalid");;
		}
		
		
		
	}
}