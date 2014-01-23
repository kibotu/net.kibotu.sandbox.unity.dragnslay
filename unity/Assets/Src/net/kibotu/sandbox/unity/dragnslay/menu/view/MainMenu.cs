using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MainMenu : MonoBehaviour
    {
        private UIButton banner;
        private UIButton queue1vs1Btn;
        private UIButton queue2vs2Btn;
        private UIButton customGameBtn;
        private UIButton settingsBtn;
        private UIButton shopBtn;
        private UIButton profileBtn;
        public UIText text;
        
        public void Start () 
        {
            var atlas = GameObject.Find("main_menu_atlas").GetComponent<UIToolkit>();
            
            const float padding = 0.12f;
            const float bannerHeight = 0.1f;

            banner = UIButton.create(atlas, "banner.png", "banner.png", 0, 0);
            banner.positionFromCenter(0, 0);
            banner.positionFromTop(0, 0);
            banner.highlightedTouchOffsets = new UIEdgeOffsets(30);
            banner.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);

            queue1vs1Btn = UIButton.create(atlas, "button.png", "button.png", 0, 0);
            queue1vs1Btn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            queue1vs1Btn.positionFromCenter(0, 0);
            queue1vs1Btn.positionFromTop(bannerHeight+padding, 0);
            queue1vs1Btn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            queue1vs1Btn.onTouchUpInside += OnQueue1vs1Clicked;

            queue2vs2Btn = UIButton.create(atlas, "button.png", "button.png", 0, 0);
            queue2vs2Btn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            queue2vs2Btn.positionFromCenter(0, 0);
            queue2vs2Btn.positionFromTop(bannerHeight + padding * 2, 0);
            queue2vs2Btn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            queue2vs2Btn.onTouchUpInside += OnQueue2vs2Clicked;

            customGameBtn = UIButton.create(atlas, "button.png", "button.png", 0, 0);
            customGameBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            customGameBtn.positionFromCenter(0, 0);
            customGameBtn.positionFromTop(bannerHeight + padding * 3, 0);
            customGameBtn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            customGameBtn.onTouchUpInside += OnCustomGameClicked;

            settingsBtn = UIButton.create(atlas, "button.png", "button.png", 0, 0);
            settingsBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            settingsBtn.positionFromCenter(0, 0);
            settingsBtn.positionFromTop(bannerHeight + padding * 4, 0);
            settingsBtn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            settingsBtn.onTouchUpInside += OnSettingsClicked;

            profileBtn = UIButton.create(atlas, "button.png", "button.png", 0, 0);
            profileBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            profileBtn.positionFromCenter(0, 0);
            profileBtn.positionFromTop(bannerHeight + padding * 5, 0);
            profileBtn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            profileBtn.onTouchUpInside += OnProfileClicked;

            shopBtn = UIButton.create(atlas, "shop.png", "shop.png", 0, 0);
            shopBtn.highlightedTouchOffsets = new UIEdgeOffsets(30);
            shopBtn.positionFromCenter(0, 0);
            shopBtn.positionFromTop(bannerHeight + padding * 6, 0);
            shopBtn.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            shopBtn.onTouchUpInside += OnShopClicked;
            
            /*var layout = new UIVerticalLayout(45);
            layout.beginUpdates();
            layout.verticalAlignMode = UIAbsoluteLayout.UIContainerVerticalAlignMode.Top;
            layout.addChild(queue1vs1Btn, queue2vs2Btn, customGameBtn, settings, shopBtn);
            layout.positionFromTop(0.2f, 0);
            layout.positionFromCenter(0, 0);
            layout.endUpdates();
            layout.matchSizeToContentSize();*/
        }

        public void OnQueue1vs1Clicked(UIButton button)
        {
            var game = new GameObject("Game").AddComponent<Game1vs1>();
            SocketHandler.SharedConnection.OnJSONEvent += game.OnJSONEvent;

            SocketHandler.Connect(1337);
            GameObject.Find("Menu").GetComponent<Menu>().ShowGameHud();

            // hide connect button
            //button.hidden = true;
        }

        public void OnQueue2vs2Clicked(UIButton button)
        {
            
        }

        public void OnCustomGameClicked(UIButton button)
        {

        }

        public void OnSettingsClicked(UIButton button)
        {

        }

        public void OnProfileClicked(UIButton button)
        {
            GameObject.Find("Menu").GetComponent<Menu>().ShowProfile();
        }

        public void OnShopClicked(UIButton button)
        {
            GameObject.Find("Menu").GetComponent<Menu>().ShowShop();
        }

        public void OnDestroy()
        {
            banner.destroy();
            queue1vs1Btn.destroy();
            queue2vs2Btn.destroy();
            customGameBtn.destroy();
            settingsBtn.destroy();
            profileBtn.destroy();
            shopBtn.destroy();
        }
    }
}
