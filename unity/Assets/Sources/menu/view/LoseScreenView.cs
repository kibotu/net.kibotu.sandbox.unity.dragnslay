using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class LoseScreenView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("in_game_atlas").GetComponent<UIToolkit>();
            var header = UIButton.create(atlas, "lose_screen_header.png", "lose_screen_header.png", 0, 0);
            header.positionFromCenter(-0.2f, 0f);
            header.scale = new Vector3(0.52083f, 0.520833f, 0);

            var confirm = UIButton.create(atlas, "lose_screen_confirm.png", "lose_screen_confirm.png", 0, 0);
            confirm.positionFromCenter(0.2f, 0);
            confirm.scale = new Vector3(0.52083f, 0.520833f, 0);
            confirm.onTouchUpInside += button => Application.LoadLevel((int)Registry.Levels.IntroWithMainMenu);
        }
    }
}