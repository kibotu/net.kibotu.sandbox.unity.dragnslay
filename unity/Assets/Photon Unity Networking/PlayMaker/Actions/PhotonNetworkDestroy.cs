// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Destroys given GameObject. This GameObject must’ve been instantiated using PhotonNetworkInstantiate and must have a PhotonView at it’s root.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1136")]
	public class PhotonNetworkDestroy : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Destroys this GameObject")]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			gameObject = null;
			
		}

		public override void OnEnter()
		{
			doDestroy();
			
			Finish();
		}
		
		
		void doDestroy()
		{
			
			var go = gameObject.Value;

			if (go != null)
			{
				PhotonNetwork.Destroy(go);
			}
			
			
		}// doDestroy
	
	}
}