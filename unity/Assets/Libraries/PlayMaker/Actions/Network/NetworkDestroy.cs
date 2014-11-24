// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

#if !(UNITY_FLASH || UNITY_NACL || UNITY_METRO || UNITY_WP8)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Destroy the object across the network.\n\nThe object is destroyed locally and remotely.\n\n" +
		"Optionally remove any RPCs accociated with the object.")]
	public class NetworkDestroy : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		[Tooltip("The Game Object to destroy.\nNOTE: The Game Object must have a NetworkView attached.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Remove all RPC calls associated with the Game Object.")]
		public FsmBool removeRPCs;		
		
		public override void Reset()
		{
			gameObject = null;
			removeRPCs = true;
		}

		public override void OnEnter()
		{
			DoDestroy();
			
			Finish();
		}

		void DoDestroy()
		{
			// get the target
			GameObject targetGo = Fsm.GetOwnerDefaultTarget(gameObject);
			if (targetGo == null || targetGo.networkView == null)
			{
				return;
			}

			if (removeRPCs.Value)
			{
				Network.RemoveRPCs(targetGo.networkView.owner);
			}
			Network.DestroyPlayerObjects(targetGo.networkView.owner);		
		}		
	}
}

#endif