// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("DisConnect to Photon Network: \n" +
		"Makes this client disconnect from the photon server, a process that leaves any room and calls OnDisconnectedFromPhoton on completition. \n")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1109")]
	public class PhotonNetworkDisconnect : FsmStateAction
	{
		public override void OnEnter()
		{
			PhotonNetwork.Disconnect();
			Finish();
		}

	}
}