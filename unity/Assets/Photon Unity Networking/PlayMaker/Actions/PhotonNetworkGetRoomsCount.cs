// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the number of rooms available.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W911")]
	public class PhotonNetworkGetRoomsCount : FsmStateAction
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of available rooms.")]
		public FsmInt roomsNumber;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			roomsNumber = null;
			
		}

		public override void OnEnter()
		{
			getRoomsNumber();
			
		if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			getRoomsNumber();
		}
		
		void getRoomsNumber()
		{

			roomsNumber.Value = PhotonNetwork.GetRoomList().Length;
			
		}

	}
}