using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.network.googleplayservice
{
    public sealed class GooglePlayServiceHelper
    {
        #region singleton

        private static volatile GooglePlayServiceHelper _instance;
        private static object syncRoot = new Object();
        private GooglePlayServiceHelper() { }

        public static GooglePlayServiceHelper Shared
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new GooglePlayServiceHelper();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Connection Type

        public enum ConnectionType
        {
            TCP, UDP
        }

        public ConnectionType Type { get; set; }

        public bool UseReliableMessages()
        {
            return Type == ConnectionType.UDP;
        }

        #endregion

        public void Login()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (Social.localUser.authenticated) return;

            PlayGamesPlatform.Activate();

            Debug.Log("Authenticating...");
            Social.localUser.Authenticate(success =>
            {
                Debug.Log(success ? "Successfully authenticated" : "Authentication failed.");
                PlayGamesPlatform.Instance.RegisterInvitationDelegate(OnInvitationReceived);
            });
        }

        public void Logout()
        {
            if (!Social.localUser.authenticated) return;

            Debug.Log("Signing out.");
            ((PlayGamesPlatform)Social.Active).SignOut();
        }

        public void ShowAchievements()
        {
            Login();
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
        public void ShowLeaderboard()
        {
            Login();
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             

        public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
        {
            Debug.Log("OnInvitationReceived invitation from " + invitation.Inviter.DisplayName);
            if (shouldAutoAccept)
            {
                Debug.Log("Accepting.");
                PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, new InvitationListener());
            }
            else
            {
                ShowInbox();
            }
        }

        public void ShowInvitationScreen()
        {
            Login();
            const int MinOpponents = 1, MaxOpponents = 1;
            const int GameVariant = 0;
            RealTimeMultiplayerListener listener = new InvitationListener();
            PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents, GameVariant, listener);
        }

        public void LeaveRoom()
        {
            Debug.Log("Leave Room.");
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        }

        public void StartQuickMatchRT()
        {
            Debug.Log("StartQuickMatchRT");
            const int MinOpponents = 1, MaxOpponents = 1;
            const int GameVariant = 0;
            PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents, GameVariant, new InvitationListener());
        }

        public void StartQuickMatchTurnBased()
        {
            Debug.Log("StartQuickMatchTurnBased");
            const int MinOpponents = 1;
            const int MaxOpponents = 1;
            const int Variant = 0;  // default
            PlayGamesPlatform.Instance.TurnBased.CreateQuickMatch(MinOpponents, MaxOpponents, Variant,  OnMatchStarted);
        }

        public void OnMatchStarted(bool success, TurnBasedMatch match) {
            if (success) {
                Debug.Log("Match Started.");
                foreach (var participiant in match.Participants)
                {
                    Debug.Log(participiant + " connected to room.");
                }

                string s = "Hello World.";
                bool reliably = Type == ConnectionType.TCP;
                Debug.Log("Send message reliably: " + reliably);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliably,GetBytes(s));
                // go to the game screen and play!
            } else {
                Debug.Log("Match failed.");
                // show error message
            }
        }

        public void ShowInbox()
        {
            Debug.Log("Show inbox.");
            PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(new InvitationListener());
        }

        public void BroadcastMessage(string message)
        {

            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(UseReliableMessages(), GetBytes(message));
        }
        public void BroadcastMessage(JObject message)
        {
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(UseReliableMessages(), ToBytes(message));
        }

        public static byte[] ToBytes(JObject json)
        {
            return GetBytes(json.ToString());
        }
        public static JObject ToJObject(byte[] data)
        {
            return JObject.Parse(GetString(data)); 
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
