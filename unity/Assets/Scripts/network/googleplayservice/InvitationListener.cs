using System.Linq;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

namespace Assets.Sources.network.googleplayservice
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
                Debug.Log(participantIds + " has joined the room.");
            }
        }

        public void OnPeersDisconnected(string[] participantIds)
        {
            Debug.Log("OnPeersDisconnected: " + participantIds.Count());
            foreach (var participiant in participantIds)
            {
                Debug.Log(participantIds + " has left the room.");
            }
        }

        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
        {
            Debug.Log("OnRealTimeMessageReceived: reliably: " + isReliable + " senderId: " + senderId + " bytes: " + data.Count());
        }
    }
}
