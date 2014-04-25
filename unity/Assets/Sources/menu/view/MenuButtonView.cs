using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class MenuButtonView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("in_game_hud_atlas").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "menu.png", "menu.png", 0, 0);
            sprite.positionFromTopLeft(0, 0);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
            sprite.onTouchUp += button => Application.LoadLevel((int) Registry.Levels.IntroWithMainMenu);
        }
    }
}