// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Enable or disable the processing of Photon network messages.\n\nIf this is disabled no Photon RPC call execution or Photon network view synchronization takes place.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W916")]
	public class PhotonNetworkSetIsMessageQueueRunning : FsmStateAction
	{
		[Tooltip("Is Message Queue Running. If this is disabled no Photon RPC call execution or Photon network view synchronization takes place")]
		public FsmBool isMessageQueueRunning;
		
		public override void Reset()
		{
			isMessageQueueRunning = null;
		}

		public override void OnEnter()
		{
			UnityEngine.Debug.Log("set isMessageQueueRunning to "+isMessageQueueRunning.Value);
			PhotonNetwork.isMessageQueueRunning = isMessageQueueRunning.Value;
			
			Finish();
		}

	}
}