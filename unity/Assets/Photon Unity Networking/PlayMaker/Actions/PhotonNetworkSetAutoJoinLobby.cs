// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines if the PhotonNetwork should join the 'lobby' when connected to the Master server. " +
		"If this is false, PHOTON / CONNECTED TO MASTER will be called when connection to the Master is available." +
		 "PHOTON / JOINED LOBBY will NOT be called if this is false.\n" +
		 "Note: The room listing will not become available. Rooms can be created and joined (randomly) without joining the lobby (and getting sent the room list).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1108")]
	public class PhotonNetworkSetAutoJoinLobby : FsmStateAction
	{
		[Tooltip("Define if PhotonNetwork should join the 'lobby' when connected to the Master server")]
		public FsmBool autoJoinLobby;
		
		public override void Reset()
		{
			autoJoinLobby  = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.autoJoinLobby = autoJoinLobby.Value;
			
			Finish();
		}

	}
}