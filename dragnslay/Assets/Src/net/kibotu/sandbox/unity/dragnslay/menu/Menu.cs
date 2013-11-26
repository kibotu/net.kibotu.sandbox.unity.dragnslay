using Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view;
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
