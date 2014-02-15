// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System;
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to the photon server by address, port, appID and game(client) version." +
		"This method is used by ConnectUsingSettings which applies values from the settings file.)")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1107")]
	public class PhotonNetworkConnectManually : FsmStateAction
	{
		[Tooltip("The master server's address (either your own or Photon Cloud address).")]
		public FsmString serverAddress;
		
		[Tooltip("The master server's port to connect to.")]
		public FsmInt port;
		
		[Tooltip("Your application ID (Photon Cloud provides you with a GUID for your game).")]
		public FsmString applicationID;
		
		[Tooltip("This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).")]
		public FsmString clientGameVersion;
		
		public override void Reset()
		{
			serverAddress  = "app-eu.exitgamescloud.com";
			port = 5055;
			applicationID = "YOUR APP ID";
			clientGameVersion = "1.0";
		}

		public override void OnEnter()
		{
			PhotonNetwork.Connect(serverAddress.Value,port.Value,applicationID.Value,clientGameVersion.Value);	
			Finish();
		}

	}
}