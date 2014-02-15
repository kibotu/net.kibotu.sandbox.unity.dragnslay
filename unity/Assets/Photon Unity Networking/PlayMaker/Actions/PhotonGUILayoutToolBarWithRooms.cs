// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("GUILayout Toolbar listing Photon rooms.\n" +
	 	"The selection event int data contains the room index, and the event string data contains the selected room name")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W901")]
	public class PhotonGUILayoutToolbarWithRooms : GUILayoutAction
	{
		
		[Tooltip("If True, append to the room name the number of users against the maximum ( '--- 1/3' )")]
		public FsmBool displayRoomDetails;
		
		[Tooltip("The selected room index")]
		[UIHint(UIHint.Variable)]
		public FsmInt selectedRoomIndex;
		
		[Tooltip("The selected room name")]
		[UIHint(UIHint.Variable)]
		public FsmString selectedRoomName;
		
		[Tooltip("Event sent when user select a room from the toolbar")]
		public FsmEvent selectionEvent;
		
		
		public FsmString style;
		
		
		private string[] roomNames;
		
		public override void Reset()
		{
			base.Reset();
			displayRoomDetails = true;
			selectedRoomIndex = null;
			selectedRoomName = null;
			
			selectionEvent = null;
			
			style = "Button";
		}
		
		public override void OnGUI()
		{
			var guiChanged = GUI.changed;
			GUI.changed = false;
			
			
			RoomInfo[] _rooms = PhotonNetwork.GetRoomList();
			
			if (_rooms.Length==0)
			{
				GUIUtility.ExitGUI();
				return;
			}
			
			
			roomNames = new string[_rooms.Length];
			
			bool _details = displayRoomDetails.Value == true;
			
			
			int i=0;
			
			foreach (RoomInfo _room in _rooms)
            {
				roomNames[i] = _room.name;
				if (_details)
				{
					roomNames[i] += " ("+_room.playerCount+"/"+_room.maxPlayers+")";
				}
				i++;
			}
			
			int _selection = GUILayout.Toolbar(selectedRoomIndex.Value, roomNames, style.Value, LayoutOptions);
			
			selectedRoomIndex.Value = _selection;
			selectedRoomName.Value = _rooms[_selection].name;
			
			if (GUI.changed)
			{
				Fsm.EventData.IntData = _selection;
				Fsm.EventData.StringData = _rooms[_selection].name;
				Fsm.Event(selectionEvent);
				GUIUtility.ExitGUI();
			
				
			}else{
				GUI.changed = guiChanged;
			}
		}
		
	}
}