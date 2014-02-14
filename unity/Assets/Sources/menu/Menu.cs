using Assets.Sources.menu.view;
using UnityEngine;
using NetworkView = Assets.Sources.menu.view.NetworkView;

namespace Assets.Sources.menu
{
    class Menu : MonoBehaviour
    {
        private GameObject _miniMapCamera;

        public void Start()
        {
            ShowMainMenu();

            _miniMapCamera = GameObject.Find("MiniMapCamera");
            _miniMapCamera.SetActive(false);
        }

        public void ShowMainMenu()
        {
            gameObject.AddComponent<MainMenu>();
        }

        public void ShowGameHud()
        {
            Destroy(gameObject.GetComponent<MainMenu>());
                
            gameObject.AddComponent<BoostsView>();
            gameObject.AddComponent<NetworkView>();
            gameObject.AddComponent<MapView>();
            gameObject.AddComponent<MenuButtonView>();
            gameObject.AddComponent<ResourcesView>();
            gameObject.AddComponent<CornerView>();

            _miniMapCamera.SetActive(true);
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
    }
}
