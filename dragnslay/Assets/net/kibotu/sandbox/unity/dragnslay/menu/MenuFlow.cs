using Assets.net.kibotu.sandbox.unity.dragnslay.pattern;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.menu
{
    class MenuFlow : Singleton<MonoBehaviour>
    {
        private enum State
        {
            MAIN_MENU, MAIN_MENU_CUSTOM_GAME, GAME_SCREEN_1VS1, GAME_SCREEN_SINGLE_PLAYER 
        }

        private State _currentState;
        public Texture btnTexture;
        private GUIStyle style;
        private float scale = 0.5f;
        private float yOffset = 200;
        private float xOffset = 50;
        private float btnHeight;

        public void Start()
        {
            _currentState = State.MAIN_MENU;
            style = new GUIStyle();
            btnHeight = (btnTexture.height - 100);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(xOffset, btnHeight * 0 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                Debug.Log("single clicked");
            }

            if (GUI.Button(new Rect(xOffset, btnHeight * 1 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                Debug.Log("1vs1 clicked");
            }

            if (GUI.Button(new Rect(xOffset, btnHeight * 2 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                Debug.Log("Custom clicked");
            }
        }

        public void Update()
        {
          
        }
    }
}
