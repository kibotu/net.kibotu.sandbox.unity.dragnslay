// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Updates and synchronizes the named custom property of the local player. New properties are added, existing values are updated. CustomProperties can be set before entering a room and will be synced as well.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1113")]
	public class PhotonNetworkSetPlayerCustomProperty : FsmStateAction
	{
		[Tooltip("The Custom Property to set or update")]
		public FsmString customPropertyKey;
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar customPropertyValue;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			customPropertyValue = null;
		}
		
		public override void OnEnter()
		{
			SetPlayerProperty();
			
			Finish();
		}
		
		void SetPlayerProperty()
		{
			if (customPropertyValue==null)
			{
				LogError("customPropertyValue is null ");
				return;
			}
			
			PhotonPlayer _player = PhotonNetwork.player;
			
			Hashtable _prop = new Hashtable();
			Log(" set key "+customPropertyKey.Value+"="+ PlayMakerPhotonProxy.GetValueFromFsmVar(this.Fsm,customPropertyValue));
			
			_prop[customPropertyKey.Value] = PlayMakerPhotonProxy.GetValueFromFsmVar(this.Fsm,customPropertyValue);
			_player.SetCustomProperties(_prop);
		}

	}
}