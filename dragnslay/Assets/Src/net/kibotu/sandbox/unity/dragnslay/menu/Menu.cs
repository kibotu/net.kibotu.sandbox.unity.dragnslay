using System.Collections;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view;
using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu
{
    class Menu : MonoBehaviour
    {
        public void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            gameObject.AddComponent<MainMenu>();
        }
    }
}
