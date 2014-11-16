using System.Collections;
using Assets.Scripts.network.googleplayservice;
using Assets.Sources.utility;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.menu
{
    public class Menu : MonoBehaviour
    {
        public enum Level
        {
            MainMenu, LoadingScreen, Hud, LoseScreen, WinScreen, Level01, Windmill, CombatDemo
        }

        public GameObject MainMenu;
        public GameObject Hud;
        public GameObject Market;
        public GameObject Profile;
        public GameObject WinScreen;
        public GameObject LoseScreen;
        public GameObject LoadingScreen;
        public GameObject Upgrades;
        public InvitationListener InvitationListener;

        #region Singleton

        private static volatile Menu _instance;
        private static readonly object SyncRoot = new Object();
        private Menu() { }

        public static Menu Shared
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Menu();
                    }
                }

                return _instance;
            }
        }

        #endregion

        public void Awake()
        {
            DontDestroyOnLoad(this);
            InvitationListener = new InvitationListener();

            // random background scene
            Application.LoadLevelAdditiveAsync(Random.Range(0, 2) == 0 ? Level.Windmill.Name() : Level.CombatDemo.Name());
        }

        public void Start()
        {
            var inbox = GetComponent<InvitationInbox>();

            GooglePlayServiceHelper.Shared.Login(()=> PlayGamesPlatform.Instance.RegisterInvitationDelegate(
                (Invitation invitation, bool shouldAutoAccept) =>
                {
                    Debug.Log("OnInvitationReceived invitation from " + invitation.Inviter.DisplayName);

                    if (shouldAutoAccept)
                    {
                        Debug.Log("Should auto accept.");
                    }
                    else
                    {
                        Debug.Log("Should not auto accept.");
                    }

                    // show invitation button
                    inbox.FriendInviteBtn1.GetComponent<Image>().enabled = true;
                    inbox.FriendInviteBtn1.GetComponentInChildren<Text>().enabled = true;

                    // set inviter name
                    var text = inbox.FriendInviteBtn1.GetComponentInChildren<Text>();
                    text.text = invitation.Inviter.DisplayName;

                    // clean button
                    inbox.FriendInviteBtn1.onClick.RemoveAllListeners();

                    // accept invite on click
                    inbox.FriendInviteBtn1.onClick.AddListener(() =>
                    {
                        GooglePlayServiceHelper.Shared.AcceptInvitation(invitation.InvitationId, InvitationListener);

                        inbox.FriendInviteBtn1.GetComponent<Image>().enabled = false;
                        inbox.FriendInviteBtn1.GetComponentInChildren<Text>().enabled = false;
                    });
                }));
        }

        public void Remove(GameObject go)
        {
            Destroy(go);
        }

        public void LoadLevel(string level)
        {
            Application.LoadLevel(level);
        }

        public void ShowMainMenu()
        {
            var go = Instantiate(MainMenu) as GameObject;
            const float fadeInTime = 1.5f;
            foreach (var image in go.GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name == "Banner")
                    image.ScaleOverTime(new Vector3(0.35f, 0.35f, 0.35f), new Vector3(0.7f, 0.7f, 0.7f), 0.7f);

                if (image.gameObject.name == "Profile" || 
                    image.gameObject.name == "Market" || 
                    image.gameObject.name == "Invite Friends" ||
                    image.gameObject.name == "Play Online" ||
                    image.gameObject.name == "Play Saga" ||
                    image.gameObject.name == "Settings")
                    image.ScaleOverTime(new Vector3(0.25f, 0.2f, 0.2f), new Vector3(0.4f, 0.4f, 0.4f), 0.7f);


                image.CrossFadeAlpha(0.0f,0,false);
                image.CrossFadeAlpha(1f, fadeInTime, false);
            }

            foreach (var text in go.GetComponentsInChildren<Text>())
            {
                text.CrossFadeAlpha(0.0f, 0, false);
                text.CrossFadeAlpha(1f, fadeInTime, false);
            }
        }

        public void ShowGameHud()
        {
            
        }

        public void ShowMarket()
        {
        }

        public void ShowAchievements()
        {
            GooglePlayServiceHelper.Shared.ShowAchievements();
        }
        public void ShowLeaderboard()
        {
            GooglePlayServiceHelper.Shared.ShowLeaderboard();
        }

        public void ShowUpgrades()
        {
        }

        public void ShowWinScreen()
        {
        }

        public void ShowLoseScreen()
        {
        }

        public void ShowLoadingScreen()
        {
        }

        public void ShowInviteFriendsScreen()
        {
            GooglePlayServiceHelper.Shared.ShowInvitationScreen(1,1);
        }

        public void PlaySagaScreen()
        {
        }

        public void PlayOnlineScreen()
        {
            Application.LoadLevel(Level.Level01.Name());
            GooglePlayServiceHelper.Shared.StartQuickMatchRT(InvitationListener, 1, 1);
        }

        public void ShowSettings()
        {
           
        }

        public void ShowInbox()
        {
            GooglePlayServiceHelper.Shared.ShowInbox();
        }
    }

    public static partial class TransitionExtentions
    {
        public static void ScaleOverTime(this Image image, Vector3 from, Vector3 to, float time)
        {
            image.StartCoroutine(image.rectTransform.ScaleOverTime(from, to, time));
        }

        public static IEnumerator ScaleOverTime(this RectTransform rect, Vector3 from, Vector3 to, float time)
        {
            float startTime = 0;
            rect.localScale = from;
            while (Mathf.Abs(rect.localScale.magnitude - to.magnitude) > Mathf.Epsilon)
            {
                startTime += Time.deltaTime;
                rect.localScale = Vector3.Lerp(from, to, startTime / time);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
