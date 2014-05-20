using Assets.Sources.menu.view;
using UnityEngine;
using NetworkView = Assets.Sources.menu.view.NetworkView;

namespace Assets.Sources.menu
{
    public class Menu : MonoBehaviour
    {
        public void Start()
        {
//            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            gameObject.AddComponent<MainMenu>();
        }

        public void ShowGameHud()
        {
            GameObject.Find("MiniMapCamera").camera.enabled = false;
                
            gameObject.AddComponent<BoostsView>();
            gameObject.AddComponent<NetworkView>();
//            gameObject.AddComponent<MapView>();
            gameObject.AddComponent<MenuButtonView>();
            gameObject.AddComponent<ResourcesView>();
            gameObject.AddComponent<CornerView>();
        }

        public void ShowShop()
        {
            Destroy(gameObject.GetComponent<MainMenu>());
            gameObject.AddComponent<ShopView>();
        }

        public void ShowProfile()
        {
            Destroy(gameObject.GetComponent<MainMenu>());
            gameObject.AddComponent<ProfileView>();
        }

        public void ShowUpgrades()
        {
            Destroy(gameObject.GetComponent<ProfileView>());
            gameObject.AddComponent<UpgradesView>();
        }

        public void ShowWinScreen()
        {
            gameObject.AddComponent<WinScreenView>();
        }

        public void ShowLoseScreen()
        {
            gameObject.AddComponent<LoseScreenView>();
        }

        public void ShowLoadingScreen()
        {
            gameObject.AddComponent<LoadingScreen>();
        }
    }
}
