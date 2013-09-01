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
        public Texture friendlistTexture;
        public Texture bannerTexture;
        public Texture backgroundTexture;
        private GUIStyle style;
        private float scale = 0.6f;
        private float yOffset = 300;
        private float xOffset = 50;
        private float btnHeight;
        private GameObject background;
        private GameObject banner;
        private GameObject camera;
        private Material transparencyMaterial;
        public float aspectRatio;

        private void OnGUI()
        {
            switch (_currentState)
            {
                case State.MAIN_MENU:
                    ShowMainMenu();
                    break;
                case State.GAME_SCREEN_SINGLE_PLAYER:
                    ShowGameSinglePlayer();
                    break;
                case State.GAME_SCREEN_1VS1:
                    ShowGame1vs1();
                    break;
                case State.MAIN_MENU_CUSTOM_GAME:
                    ShowCustomGame();
                    break;
            }
        }

        public void Update()
        {

        }

        public void Start()
        {
            _currentState = State.MAIN_MENU;
            camera = GameObject.Find("Main Camera");
            style = new GUIStyle();
            btnHeight = (btnTexture.height - 100);
            aspectRatio = 1280 / 800; // 1.6
        }

        public void addFixedImages()
        {
            /*background = GameObject.CreatePrimitive(PrimitiveType.Plane);
            background.renderer.material.mainTexture = backgroundTexture;
            background.transform.parent = camera.transform;
            background.transform.rotation = Quaternion.AngleAxis(270, new Vector3(1, 0, 0)) * Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
            background.transform.localScale += new Vector3(9, 0, 5.625f);

            banner = GameObject.CreatePrimitive(PrimitiveType.Plane);
            banner.renderer.material.mainTexture = bannerTexture;
            banner.transform.parent = camera.transform;
            banner.transform.rotation = Quaternion.AngleAxis(270, new Vector3(1, 0, 0)) * Quaternion.AngleAxis(180, new Vector3(0, 1, 0));*/
        }

        private void ShowMainMenu()
        {

            GUI.DrawTexture(new Rect(0, 0, 1280, 800), backgroundTexture, ScaleMode.ScaleAndCrop, true, 1.6f);

            GUI.DrawTexture(new Rect(300, 30, bannerTexture.width * scale, bannerTexture.height * scale), bannerTexture, ScaleMode.ScaleAndCrop, true, 1.6f);

            if (GUI.Button(new Rect(xOffset, btnHeight * 0 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.GAME_SCREEN_SINGLE_PLAYER;
            }

            if (GUI.Button(new Rect(xOffset, btnHeight * 1 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.GAME_SCREEN_1VS1;
            }

            if (GUI.Button(new Rect(xOffset, btnHeight * 2 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.MAIN_MENU_CUSTOM_GAME;
            }

            if (GUI.Button(new Rect(930, 100, friendlistTexture.width * scale, friendlistTexture.height * scale), friendlistTexture, style))
            {
                Debug.Log("friendlist clicked");
            }

            // LogInput()
        }

        private void ShowGameSinglePlayer()
        {
            if (GUI.Button(new Rect(xOffset, btnHeight * 0 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.MAIN_MENU;
            }
        }

        private void ShowGame1vs1()
        {
            if (GUI.Button(new Rect(xOffset, btnHeight * 1 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.MAIN_MENU;
            }
        }

        private void ShowCustomGame()
        {
            if (GUI.Button(new Rect(xOffset, btnHeight * 2 * scale + yOffset, btnTexture.width * scale, btnTexture.height * scale), btnTexture, style))
            {
                _currentState = State.MAIN_MENU;
            }
        }

        public void LogInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 screenPos = Input.mousePosition;
                screenPos.z = 30;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                Debug.Log(screenPos + " " + worldPos);
            }
        }
    }
}
