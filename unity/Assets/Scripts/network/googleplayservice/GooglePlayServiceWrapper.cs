using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

namespace Assets.Sources.network.googleplayservice
{
    public class GooglePlayServiceWrapper
    {
        private void Login()
        {
            if (!Social.localUser.authenticated)
            {
                Debug.Log("Authenticating...");
                Social.localUser.Authenticate((bool success) =>
                {
                    Debug.Log(success ? "Successfully authenticated" : "Authentication failed.");
                    PlayGamesPlatform.Instance.RegisterInvitationDelegate(OnInvitationReceived);
                });
            }

        }

        public void ToggleLogin()
        {
            GooglePlayGames.PlayGamesPlatform.Activate();

            if (!Social.localUser.authenticated)
            {
                Debug.Log("Authenticating...");
                Social.localUser.Authenticate((bool success) =>
                {
                    Debug.Log(success ? "Successfully authenticated" : "Authentication failed.");
                });
            }
            else
            {
                // Sign out!
                Debug.Log("Signing out.");
                ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
            }
        }

        private void Logout()
        {
            if (Social.localUser.authenticated)
            {
                Debug.Log("Signing out.");
                ((PlayGamesPlatform)Social.Active).SignOut();
            }
        }

        public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
        {
            Debug.Log("OnInvitationReceived invitation from " + invitation.Inviter.DisplayName + " shouldAutoAccept: " + shouldAutoAccept);
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, new InvitationListener());
        }

        public void ShowInvitationScreen()
        {
            Login();
            const int MinOpponents = 1, MaxOpponents = 3;
            const int GameVariant = 0;
            RealTimeMultiplayerListener listener = new InvitationListener();
            PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents, GameVariant, listener);
        }
    }
}
