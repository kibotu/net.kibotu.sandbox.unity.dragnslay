using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public void InviteFriends(string player)
        {
			throw new System.NotImplementedException ();

            // todo experimental invite player by player id, however id changes arbitary 
            IList<string> list = new []{player};

			PlayGamesPlatform.Instance.RealTime.CreateQuickGame(1,1,list,0,RtsHandler);
        }
    }
}
