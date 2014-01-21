using Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view;
using UnityEngine;
using NetworkView = Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view.NetworkView;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu
{
    class Menu : MonoBehaviour
    {
        private GameObject _miniMap;
        private GameObject _miniMapCamera;

        public void Start()
        {
            var uiToolkit = GameObject.Find("UIToolkit").GetComponent<UI>();

            //uiToolkit.maxWidthOrHeightForHD = 800; //  DisplayMetricsAndroid.WidthPixels;
            //uiToolkit.maxWidthOrHeightForSD = 800; // DisplayMetricsAndroid.WidthPixels;

            ShowMainMenu();

            _miniMap = GameObject.Find("MiniMap");
            _miniMap.SetActive(false); 
            _miniMapCamera = GameObject.Find("MiniMapCanera");
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

            _miniMap.SetActive(true);
            _miniMapCamera.SetActive(true);
        }

        public void ShowShop()
        {
            Destroy(gameObject.GetComponent<MainMenu>());

            gameObject.AddComponent<ShopView>();
        }
    }
}
