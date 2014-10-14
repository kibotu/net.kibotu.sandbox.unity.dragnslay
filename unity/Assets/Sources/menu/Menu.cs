using System;
using System.Collections;
using Assets.Sources.menu.transition;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.menu
{
    public class Menu : MonoBehaviour
    {
        public GameObject MainMenu;
        public GameObject Hud;
        public GameObject Market;
        public GameObject Profile;
        public GameObject WinScreen;
        public GameObject LoseScreen;
        public GameObject LoadingScreen;
        public GameObject Upgrades;

        private static Menu _instance;

        public Menu Shared
        {
            get { return _instance ?? (_instance = new Menu()); }
        }

        public void Remove(GameObject go)
        {
            Destroy(go);
        }

        public void LoadLevel(String level)
        {
            Application.LoadLevelAdditiveAsync(level);
        }

        public void ShowMainMenu()
        {
            var go = Instantiate(MainMenu) as GameObject;
            const float fadeInTime = 3f;
            foreach (var image in go.GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name == "Banner")
                    image.ScaleOverTime(new Vector3(0.35f, 0.35f, 0.35f), new Vector3(0.7f, 0.7f, 0.7f), 5f);

                if (image.gameObject.name == "Profile" || 
                    image.gameObject.name == "Market" || 
                    image.gameObject.name == "Invite Friends" ||
                    image.gameObject.name == "Play Online" ||
                    image.gameObject.name == "Play Saga" ||
                    image.gameObject.name == "Settings")
                    image.ScaleOverTime(new Vector3(0.3f, 0.2f, 0.2f), new Vector3(0.4f, 0.4f, 0.4f), 5f);


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

        public void ShowProfile()
        {
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
        }

        public void PlaySagaScreen()
        {
        }

        public void PlayOnlineScreen()
        {
        }

        public void ShowSettings()
        {
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
            while (Math.Abs(rect.localScale.magnitude - to.magnitude) > Mathf.Epsilon)
            {
                startTime += Time.deltaTime;
                rect.localScale = Vector3.Lerp(from, to, startTime / time);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
