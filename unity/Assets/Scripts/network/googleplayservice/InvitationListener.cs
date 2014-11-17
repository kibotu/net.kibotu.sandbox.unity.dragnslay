using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.menu;
using Assets.Sources.utility;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

namespace Assets.Scripts.network.googleplayservice
{
    public class InvitationListener : RealTimeMultiplayerListener
    {


        public void OnRoomSetupProgress(float percent)
        {
            Debug.Log("OnRoomSetupProgress: " + percent);
        }

        public void OnRoomConnected(bool success)
        {
            Debug.Log("OnRoomConnected: " + success);

            if (success)
            {
                List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
                foreach (var participiant in participants)
                {
                    Debug.Log(participiant + " connected to room.");
                }

                Application.LoadLevel(Menu.Level.Level01.Name());
                Application.LoadLevelAdditiveAsync(Menu.Level.Hud.Name());
            }
        }

        public void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }

        public void OnPeersConnected(string[] participantIds)
        {
            Debug.Log("OnRoomConnected: " + participantIds.Count());
            foreach (var participiant in participantIds)
            {
                Debug.Log(participiant + " has joined the room.");
            }
        }

        public void OnPeersDisconnected(string[] participantIds)
        {
            Debug.Log("OnPeersDisconnected: " + participantIds.Count());
            foreach (var participiant in participantIds)
            {
                Debug.Log(participiant + " has left the room.");
            }
        }

        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
        {
            Debug.Log("OnRealTimeMessageReceived: reliably: " + isReliable + " senderId: " + senderId + " bytes: " + data.Count() + " msg: " + GooglePlayServiceHelper.ToJObject(data));
        }
    }
}
