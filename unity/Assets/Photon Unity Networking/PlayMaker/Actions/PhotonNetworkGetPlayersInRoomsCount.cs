// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the count of players currently inside a room. \n" +
		"This is updated on the MasterServer (only) in 5sec intervals (if any count changed).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W908")]
	public class PhotonNetworkGetPlayersInRoomsCount : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of players currently inside a room.")]
		
		public FsmInt playersInRoomsCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			playersInRoomsCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			getPlayersInRoomsCount();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			getPlayersInRoomsCount();
		}
		
		void getPlayersInRoomsCount()
		{
			playersInRoomsCount.Value = PhotonNetwork.countOfPlayersInRooms;
			
		}

	}
}