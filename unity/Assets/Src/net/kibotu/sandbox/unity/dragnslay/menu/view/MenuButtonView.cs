using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MenuButtonView : MonoBehaviour
    {
        public void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var sprite = UIButton.create(in_game_hud, "menu.png", "menu.png", 0, 0);
            sprite.positionFromTopLeft(0, 0);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}