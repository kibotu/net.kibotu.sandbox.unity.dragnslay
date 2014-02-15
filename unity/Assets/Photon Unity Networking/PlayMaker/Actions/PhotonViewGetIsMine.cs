// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network View is controlled by a GameObject. \n A PhotonView component is required on the gameObject")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W918")]
	public class PhotonViewGetIsMine : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network view is controlled by this object.")]
		public FsmBool isMine;
		
		[Tooltip("Send this event if the Photon network view controlled by this object.")]
		public FsmEvent isMineEvent;
		
		[Tooltip("Send this event if the Photon network view is NOT controlled by this object.")]
		public FsmEvent isNotMineEvent;
		
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
			gameObject = null;
			isMine = null;
			isMineEvent = null;
			isNotMineEvent = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();
			
			checkIsMine();
			
			Finish();
		}
		
		void checkIsMine()
		{
			if (_networkView ==null)
			{
				return;	
			}
			bool _isMine = _networkView.isMine;
			isMine.Value = _isMine;
			
			if (_isMine )
			{
				if (isMineEvent!=null)
				{
					Fsm.Event(isMineEvent);
				}
			}
			else if (isNotMineEvent!=null)
			{
				Fsm.Event(isNotMineEvent);
			}
		}

	}
}