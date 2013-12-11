using Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu
{
    class Menu : MonoBehaviour
    {
        public void Start()
        {
            var uiToolkit = GameObject.Find("UIToolkit").GetComponent<UI>();

            uiToolkit.maxWidthOrHeightForHD = DisplayHelper.GetDisplayWidth();
            uiToolkit.maxWidthOrHeightForSD = DisplayHelper.GetDisplayWidth();

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            gameObject.AddComponent<MainMenu>();
        }
    }
}
