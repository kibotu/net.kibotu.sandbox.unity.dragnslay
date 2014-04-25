using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class BoostsView : MonoBehaviour
    {

        public void Start()
        {
            var atlas = GameObject.Find("in_game_hud_atlas").GetComponent<UIToolkit>();

            var skills = UIButton.create(atlas, "skills.png", "skills.png", 0, 0);
            skills.positionFromBottomRight(0, 0);
            skills.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}