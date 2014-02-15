// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Delete the owner custom property of a GameObject.\n A PhotonView component is required on the gameObject")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1117")]
	public class PhotonViewDeleteOwnerCustomProperty : FsmStateAction
	{
		
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The custom property key to delete")]
		public FsmString customPropertyKey;
		
		[Tooltip("Send this event if the custom property was deleted")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the custom property deletion failed")]
		public FsmEvent failureEvent;
		
		
		private GameObject go;
		
		private PhotonView _networkView;
		
		private void _getNetworkView()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<PhotonView>();
		}
		
		public override void Reset()
		{
			customPropertyKey = "My Property";
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			_getNetworkView();
				
			if (DeleteOwnerProperty())
			{
				Fsm.Event(successEvent);	
			}else{
				Fsm.Event(failureEvent);	
			}
			
			Finish();
		}
		
		private bool DeleteOwnerProperty()
		{
			if (_networkView==null)
			{
				return false;
			}

			PhotonPlayer _player = _networkView.owner;
			if (_player==null)
			{
				return false;
			}
			
			Hashtable _prop = new Hashtable();
			
			_prop[customPropertyKey.Value] = null;
			_player.SetCustomProperties(_prop);
			
			return true;
		}
		
	}
}