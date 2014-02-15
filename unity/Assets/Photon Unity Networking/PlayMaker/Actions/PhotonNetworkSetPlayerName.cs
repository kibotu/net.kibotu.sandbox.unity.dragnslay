// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set the name of the Photon player.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W922")]
	public class PhotonNetworkSetPlayerName : FsmStateAction
	{
		[Tooltip("The Photon player name")]
		[RequiredField]
		public FsmString playerName;
		
		public override void Reset()
		{
			playerName = null;
		}

		public override void OnEnter()
		{
			if (playerName== null)
			{
				return;
			}
			
			PhotonNetwork.playerName = playerName.Value;
			
			Finish();
		}

	}
}