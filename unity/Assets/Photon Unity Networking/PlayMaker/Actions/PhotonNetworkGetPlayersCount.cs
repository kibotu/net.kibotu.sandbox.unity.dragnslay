// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the count of players currently using this application. \n" +
		"This is updated on the MasterServer (only) in 5sec intervals (if any count changed).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W907")]
	public class PhotonNetworkGetPlayersCount : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of players currently using this application.")]
		public FsmInt playersCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			playersCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			getPlayersCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			getPlayersCount();
		}
		
		void getPlayersCount()
		{
			playersCount.Value = PhotonNetwork.countOfPlayers;
			
		}

	}
}