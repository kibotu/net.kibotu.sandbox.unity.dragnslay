using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.network.googleplayservice
{
    public sealed class GooglePlayServiceHelper
    {
        #region Singleton

        private static volatile GooglePlayServiceHelper _instance;
        private static readonly object SyncRoot = new Object();
        private GooglePlayServiceHelper() { }

        public static GooglePlayServiceHelper Shared
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
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

		#region rts handler

		public volatile MultiplayerListenerRTS RtsHandler = new MultiplayerListenerRTS(); 

		#endregion

        #region Authentication

        public void Login(System.Action callback)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (Social.localUser.authenticated)
            {
                if(callback != null) 
                    callback();

                return;
            }

            PlayGamesPlatform.Activate();

            Debug.Log("Authenticating...");
            Social.localUser.Authenticate(success =>
            {
                Debug.Log(success ? "Successfully authenticated" : "Authentication failed.");
                
                if(callback != null) 
                    callback();
            });
        }

        public void Login()
        {
            Login(null);
        }

        public void Logout()
        {
            if (!Social.localUser.authenticated) return;

            Debug.Log("Signing out.");
            ((PlayGamesPlatform)Social.Active).SignOut();
        }

        #endregion

        #region build in screens

        public void ShowAchievements()
        {
            Login(() => PlayGamesPlatform.Instance.ShowAchievementsUI());
        }
        public void ShowLeaderboard()
        {
            Login(() => PlayGamesPlatform.Instance.ShowLeaderboardUI());
        }
        public void ShowInvitationScreen(int minOpponents, int maxOpponents, int variant = 0)
        {
            Login(() =>
            {
				PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minOpponents, maxOpponents, variant, RtsHandler);
            });
        }

        public void ShowInbox()
        {
            Login(()=> PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(RtsHandler));
        }

        #endregion

        public void StartQuickMatchRT(int minOpponents, int maxOpponents, int variant = 0)
        {
            Login(()=>PlayGamesPlatform.Instance.RealTime.CreateQuickGame(minOpponents, maxOpponents, variant, RtsHandler));
        }

        public void StartQuickMatchTurnBased(int minOpponents, int maxOpponents, int variant = 0)
        {
            Login(()=>PlayGamesPlatform.Instance.TurnBased.CreateQuickMatch(minOpponents, maxOpponents, variant, OnMatchStarted)); 
        }

        public void AcceptInvitation(string invitationId) 
        {
            if (!Social.localUser.authenticated)
                return;

			PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, RtsHandler);
        }

        public void LeaveRoom()
        {
            if (!Social.localUser.authenticated)
                return;

            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        }

        private void OnMatchStarted(bool success, TurnBasedMatch match) {

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

        #region send message

        public void BroadcastMessage(string message)
        {
            if (!Social.localUser.authenticated) 
                return;

            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(UseReliableMessages(), GetBytes(message));
        }

        public void BroadcastMessage(JObject message)
        {
            if (!Social.localUser.authenticated) 
                return;

            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(UseReliableMessages(), ToBytes(message));
        }

        #endregion

        #region Serialization

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

        #endregion

        public void InviteFriends(string player)
        {
			throw new System.NotImplementedException ();

            // todo experimental invite player by player id, however id changes arbitary 
            IList<string> list = new []{player};

			PlayGamesPlatform.Instance.RealTime.CreateQuickGame(1,1,list,0,RtsHandler);
        }
    }
}
