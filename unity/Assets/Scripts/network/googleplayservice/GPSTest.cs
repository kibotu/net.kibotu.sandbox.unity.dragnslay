using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.network.googleplayservice
{
    public class GPSTest : MonoBehaviour
    {
        #region Log

        public Text Log;

        public void OnEnable()
        {
            Application.RegisterLogCallback((message, stacktrace, type) => Log.text += "\n" + message);
        }
        public void Disable()
        {
            Application.RegisterLogCallback(null);
        }

        #endregion

        #region GPS

        public void InviteById()
        {
            GooglePlayServiceHelper.Shared.InviteFriends("p_CKPdgbXW7-recRAB");
        }

        public void Login()
        {
            GooglePlayServiceHelper.Shared.Login();
        }

        public void Logout()
        {
            GooglePlayServiceHelper.Shared.Logout();
        }
        public void ShowAchievements()
        {
            GooglePlayServiceHelper.Shared.ShowAchievements();
        }
        public void ShowLeaderboard()
        {
            GooglePlayServiceHelper.Shared.ShowLeaderboard();
        }
        public void SwapToTCP()
        {
            Debug.Log("Using TCP");
            GooglePlayServiceHelper.Shared.Type = GooglePlayServiceHelper.ConnectionType.TCP;

        }
        public void SwapToUDP()
        {
            Debug.Log("Using UDP");
            GooglePlayServiceHelper.Shared.Type = GooglePlayServiceHelper.ConnectionType.UDP;
        }

        public void ShowInvitationScreen()
        {
            GooglePlayServiceHelper.Shared.ShowInvitationScreen(1,1);
        }
        public void ShowInbox()
        {
            Debug.Log("Show inbox.");
            GooglePlayServiceHelper.Shared.ShowInbox();
        }

        public void QuickMatchRT()
        {
            Debug.Log("StartQuickMatchRT");
            GooglePlayServiceHelper.Shared.StartQuickMatchRT(1, 1);
        }

        public void QuickMatchTB()
        {
            Debug.Log("StartQuickMatchTurnBased");
            GooglePlayServiceHelper.Shared.StartQuickMatchTurnBased(1,1);
        }

        public void LeaveRoom()
        {
            Debug.Log("Leave Room.");
            GooglePlayServiceHelper.Shared.LeaveRoom();
        }

        #endregion

        #region Lifecycle

        public void GameStart()
        {
            Debug.Log("Start");

            JObject json = new JObject{
                {"message",     "start-game"},
                {"packageId",   0},
                {"scheduleId",  0},
                {"ack",         true}
            };

            BroadcastGpsMessage(json);
        }

        public void GamePause()
        {
            Debug.Log("Pause");

            JObject json = new JObject{
                {"message",     "pause-game"},
                {"packageId",   0},
                {"scheduleId",  0},
                {"ack",         true}
            };

            BroadcastGpsMessage(json);
        }

        public void GameResume()
        {
            Debug.Log("Resume");

            JObject json = new JObject{
                {"message",     "resume-game"},
                {"packageId",   0},
                {"scheduleId",  0},
                {"ack",         true}
            };

            BroadcastGpsMessage(json);
        }

        public void GameStop()
        {
            Debug.Log("Stop");

            JObject json = new JObject{
                {"message",     "stop-game"},
                {"packageId",   0},
                {"scheduleId",  0},
                {"ack",         true}
            };

            BroadcastGpsMessage(json);
        }
        #endregion

        #region Game Events
    
        public void SendUnits()
        {
            Debug.Log("SendUnits");
        }
        public void SendRockets()
        {
            Debug.Log("SendRockets");
        }
        public void RocketsArrive()
        {
            Debug.Log("RocketsArrive");
        }
        public void ActivateSpecial()
        {
            Debug.Log("ActivateSpecial");
        }
        #endregion

        public void BroadcastGpsMessage(string message)
        {
            GooglePlayServiceHelper.Shared.BroadcastMessage(message);
        }

        public void BroadcastGpsMessage(JObject message)
        {
            GooglePlayServiceHelper.Shared.BroadcastMessage(message);
        }
    }
}
