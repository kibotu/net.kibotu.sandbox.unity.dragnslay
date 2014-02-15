// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Leave the current game room.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W915")]
	public class PhotonNetworkLeaveRoom : FsmStateAction
	{

		public override void OnEnter()
		{
			PhotonNetwork.LeaveRoom();
			
			Finish();
		}
	}
}