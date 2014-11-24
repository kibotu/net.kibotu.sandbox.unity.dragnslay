// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Remove the RPC function calls accociated with a Game Object.\n\nNOTE: The Game Object must have a NetworkView component attached.")]
	public class NetworkViewRemoveRPCs : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("Remove the RPC function calls accociated with this Game Object.\n\nNOTE: The GameObject must have a NetworkView component attached.")]
		public FsmOwnerDefault gameObject;
		
		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoRemoveRPCsFromViewID();
			
			Finish();
		}

		void DoRemoveRPCsFromViewID()
		{
			GameObject targetGo = Fsm.GetOwnerDefaultTarget(gameObject);
			if (targetGo == null || targetGo.networkView == null)
			{
				return;
			}
			
			Network.RemoveRPCs(targetGo.networkView.viewID);
		}
		
	}
}

#endif