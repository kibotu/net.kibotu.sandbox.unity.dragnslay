// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("GUILayout Toolbar listing Photon players.\n" +
	 	"The selection event int data contains the player index, and the event string data contains the selected player name")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W900")]
	public class PhotonGUILayoutToolbarwithPlayers : GUILayoutAction
	{
		
		[Tooltip("If true, list only other players.")]
		public FsmBool otherPLayerOnly;
		
		[Tooltip("The selected player")]
		[UIHint(UIHint.Variable)]
		public FsmInt selectedPlayer;
		
		[Tooltip("The selected player name")]
		[UIHint(UIHint.Variable)]
		public FsmString selectedPlayerName;
		
		[Tooltip("Event sent when user select a player from the toolbar")]
		public FsmEvent selectionEvent;
		
		[Tooltip("The gui style of the elements in that toolbar")]
		public FsmString style;
		
		private PhotonPlayer[] players;
		private string[] playerNames;
		
		
		
		public override void Reset()
		{
			base.Reset();
			
			otherPLayerOnly = true;
			
			selectionEvent = null;
			selectedPlayerName = null;
			style = "Button";
		}
		
		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;
			
			
			if (otherPLayerOnly.Value){
				players = PhotonNetwork.otherPlayers;
			}else{
				players = PhotonNetwork.playerList;
			}
			
			if (players.Length==0)
			{
				GUIUtility.ExitGUI();
				return;
			}
			
			
			playerNames = new string[players.Length];
			
			int i=0;
			
			foreach (PhotonPlayer player in players)
            {
				playerNames[i] = player.name;
				i++;
			}
		
			
			int _selection = GUILayout.Toolbar(selectedPlayer.Value, playerNames, style.Value, LayoutOptions);
			
			selectedPlayer.Value = _selection;
			selectedPlayerName.Value = playerNames[_selection];
			
			if (GUI.changed)
			{
				Fsm.EventData.IntData = _selection;
				Fsm.EventData.StringData = playerNames[_selection];
				Fsm.Event(selectionEvent);
				GUIUtility.ExitGUI();
			
				
			}
			else
			{
				GUI.changed = guiChanged;
			}
		}
		
	}
}