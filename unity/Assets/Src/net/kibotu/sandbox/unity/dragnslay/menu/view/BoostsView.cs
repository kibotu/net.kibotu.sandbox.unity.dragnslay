using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class BoostsView : MonoBehaviour
    {
        public void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var skills = UIButton.create(in_game_hud, "skills.png", "skills.png",0,0);
            skills.positionFromBottomRight(0,0);
            skills.scale = new Vector3(0.6f,0.6f,0.6f);
        }
    }
}