using Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu
{
    class Menu : MonoBehaviour
    {
        public void Start()
        {
            var uiToolkit = GameObject.Find("UIToolkit").GetComponent<UI>();

            uiToolkit.maxWidthOrHeightForHD = 800; //  DisplayMetricsAndroid.WidthPixels;
            uiToolkit.maxWidthOrHeightForSD = 800; // DisplayMetricsAndroid.WidthPixels;

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            gameObject.AddComponent<GameMenu>();
        }

        public void ShowGameHUD()
        {
            gameObject.AddComponent<GameHUD>();
        }
    }
}
