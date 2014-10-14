using System.Collections.Generic;
using UnityEngine;

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

        private readonly List<GameObject> _active = new List<GameObject>();

        private static Menu _instance;

        public Menu Shared
        {
            get { return _instance ?? (_instance = new Menu()); }
        }

        private void ClearAll()
        {
            foreach (var o in _active)
            {
                Destroy(o);
            }
            _active.Clear();
        }

        public void ShowMainMenu()
        {
            ClearAll();
            _active.Add(Instantiate(MainMenu) as GameObject);
        }

        public void ShowGameHud()
        {
            ClearAll();
            _active.Add(Instantiate(Hud) as GameObject);
        }

        public void ShowMarket()
        {
            ClearAll();
            _active.Add(Instantiate(Market) as GameObject);
        }

        public void ShowProfile()
        {
            ClearAll();
            _active.Add(Instantiate(Profile) as GameObject);
        }

        public void ShowUpgrades()
        {
            ClearAll();
            _active.Add(Instantiate(Upgrades) as GameObject);
        }

        public void ShowWinScreen()
        {
            _active.Add(Instantiate(WinScreen) as GameObject);
        }

        public void ShowLoseScreen()
        {
            _active.Add(Instantiate(LoseScreen) as GameObject);
        }

        public void ShowLoadingScreen()
        {
            ClearAll();
            _active.Add(Instantiate(LoadingScreen) as GameObject);
        }
    }
}
