using UnityEngine;
using System.Collections;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{

   public class GameHUD : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var skills = UIButton.create(in_game_hud, "skills.png", "skills.png", Screen.width-239,Screen.height-150);
            skills.centerize();
            skills.scale = new Vector3(0.6f,0.6f,0.6f);
            skills.position = new Vector2(571,-340);

            
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}