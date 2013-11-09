using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MainMenu : MonoBehaviour {

        public UIToolkit buttonToolkit;
        private float _startTime = 0;

        // Use this for initialization
        public void Start ()
        {
            var playButton = UIButton.create("button.png", "button.png", 0, 0);
            playButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
            playButton.scale = new Vector3(0.3f, 0.3f, 0);
             #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJNI.AttachCurrentThread();
            AndroidJNIHelper.debug = false;
            #endif
            var world = GameObject.Find("World");
            world.AddComponent<WorldData>();
            world.AddComponent<Game1vs1>();
        }
	
        public void Update () {
            _startTime += Time.deltaTime;
            if (_startTime > 7f)
            {
                _startTime = 0;
                //SocketHandler.Instance.Emit("send", SocketHandler.Instance.CreateHelloWorldMessage());
            }
        }
    }
}
