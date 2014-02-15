// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the count of players currently looking for a room. \n" +
		"This is updated on the MasterServer (only) in 5sec intervals (if any count changed).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W909")]
	public class PhotonNetworkGetPlayersOnMasterCount : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of players currently looking for a room.")]
		public FsmInt playersOnMasterCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			playersOnMasterCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			getPlayersOnMasterCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			getPlayersOnMasterCount();
		}
		
		void getPlayersOnMasterCount()
		{
			playersOnMasterCount.Value = PhotonNetwork.countOfPlayersOnMaster;
			
		}

	}
}