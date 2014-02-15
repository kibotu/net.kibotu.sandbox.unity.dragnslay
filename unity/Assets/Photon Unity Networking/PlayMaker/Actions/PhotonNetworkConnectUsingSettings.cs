// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
		"Uses: Connect(string serverAddress, int port, string uniqueGameID)")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W902")]
	public class PhotonNetworkConnectUsingSettings : FsmStateAction
	{
		[Tooltip("The gameVersion")]
		public FsmString gameVersion;
		
		public override void Reset()
		{
			gameVersion  = "1.0";
		}

		public override void OnEnter()
		{
			PhotonNetwork.ConnectUsingSettings(gameVersion.Value);
			
			Finish();
		}

	}
}