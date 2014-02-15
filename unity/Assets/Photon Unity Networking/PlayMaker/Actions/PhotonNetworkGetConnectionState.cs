// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Send Events based on the status of the Photon network connection: Connected, Connecting, Disconnected, Disconnecting, InitializingApplication")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W904")]
	public class PhotonNetworkGetConnectionState : FsmStateAction
	{
		[Tooltip("Event to send if Photon Network state is 'Connected'.")]
		public FsmEvent isConnectedEvent;
		
		[Tooltip("Event to send if Photon Network state is 'Connecting'.")]
		public FsmEvent isConnectingEvent;
		
		[Tooltip("Event to send if Photon Network state is 'Disconnected'")]
		public FsmEvent isDisconnectedEvent;
		
		[Tooltip("Event to send if Photon Network state is 'Disconnecting'")]
		public FsmEvent isDisconnectingEvent;
		
		[Tooltip("Event to send if Photon Network state is 'InitializingApplication'")]
		public FsmEvent isInitializingApplicationEvent;

		[Tooltip("Repeat every frame. Useful if you're waiting for a particulare network state.")]
		public bool everyFrame;

		public override void Reset()
		{
			isConnectedEvent = null;
			isConnectingEvent = null;
			isDisconnectedEvent = null;
			isDisconnectingEvent = null;
			isInitializingApplicationEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoPhotonNetworkStateSwitch();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoPhotonNetworkStateSwitch();
		}

		void DoPhotonNetworkStateSwitch()
		{
			switch (PhotonNetwork.connectionState)
			{
				case ConnectionState.Connected:
					
					Fsm.Event(isConnectedEvent);	
					break;

				case ConnectionState.Connecting:

					Fsm.Event(isConnectingEvent);
					break;
				
				case ConnectionState.Disconnected:
					
					Fsm.Event(isDisconnectingEvent);
					break;
				
				case ConnectionState.Disconnecting:
					
					Fsm.Event(isDisconnectingEvent);
					break;
				case ConnectionState.InitializingApplication:
					
					Fsm.Event(isInitializingApplicationEvent);
					break;
			}
		}
	}
}