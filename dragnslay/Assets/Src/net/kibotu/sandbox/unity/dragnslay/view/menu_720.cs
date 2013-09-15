using UnityEngine;
using System.Collections;

public class menu_720 : MonoBehaviour {
	
	/*private enum State
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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void ShowMainMenu()
        {

            GUI.DrawTexture(new Rect(0, 0, 1280, 800), backgroundTexture, ScaleMode.ScaleAndCrop, true, 1.6f);

            GUI.DrawTexture(new Rect(300, 30, bannerTexture.width * scale, bannerTexture.height * scale), bannerTexture, ScaleMode.ScaleAndCrop, true, 1.6f);

            GUIContent buttonContent = new GUIContent("Single Player", btnTexture);
            style.alignment = TextAnchor.MiddleCenter;

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
        }*/
}
