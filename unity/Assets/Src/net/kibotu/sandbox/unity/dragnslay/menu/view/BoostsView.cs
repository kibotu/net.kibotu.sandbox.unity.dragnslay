using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class BoostsView : MonoBehaviour
    {
        void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var skills = UIButton.create(in_game_hud, "skills.png", "skills.png", Screen.width-239,Screen.height-150);
            skills.positionCenter();
            skills.scale = new Vector3(0.6f,0.6f,0.6f);
            skills.position = new Vector2(571,-340);
        }
    }
}