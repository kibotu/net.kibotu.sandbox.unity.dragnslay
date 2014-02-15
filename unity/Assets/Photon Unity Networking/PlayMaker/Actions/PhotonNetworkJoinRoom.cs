// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Join room with given title. If no such room exists, An Photon Error Event will occur (FAILED TO JOIN ROOM")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W914")]
	public class PhotonNetworkJoinRoom : FsmStateAction
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		public override void Reset()
		{
			roomName  = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.JoinRoom(roomName.Value);
			
			Finish();
		}

	}
}