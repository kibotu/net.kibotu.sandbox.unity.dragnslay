using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MainMenu : MonoBehaviour {

        void Start () {
            var mainMenuToolkit = GameObject.Find("main_menu").GetComponent<UIToolkit>();

            var playButton = UIButton.create(mainMenuToolkit, "button.png", "button.png", 0, 0);
            playButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
            playButton.centerize();
            playButton.scaleFromTo(1.0f, Vector3.zero, new Vector3(0.3f, 0.3f, 0), Easing.Quintic.easeOut);
            playButton.onTouchUpInside += OnPlayButtonClicked;
        }

        public void OnPlayButtonClicked(UIButton button)
        {
            var game = new GameObject("Game").AddComponent<Game1vs1>();
            SocketHandler.SharedConnection.OnJSONEvent += game.OnJSONEvent;

            SocketHandler.Connect(1337);
            GameObject.Find("Menu").GetComponent<Menu>().ShowGameHUD();

            // hide connect button
            button.hidden = true;
        }
    }
}
