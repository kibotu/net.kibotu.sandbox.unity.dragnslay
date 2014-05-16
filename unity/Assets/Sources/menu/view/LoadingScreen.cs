using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class LoadingScreen : MonoBehaviour
    {

        public void Start()
        {
            var atlas = GameObject.Find("main_menu_atlas").GetComponent<UIToolkit>();

            var main = UIButton.create(atlas, "loadingscreenMain.png", "loadingscreenMain.png", 0, 0);
            main.positionCenter();
            main.scale = new Vector3(0.52083f, 0.520833f, 0);

            var pendel = UIButton.create(atlas, "loadingscreenPendel.png", "loadingscreenPendel.png", 0, 0);
            pendel.positionFromCenter(0.3f, 0);
            pendel.scale = new Vector3(0.52083f, 0.520833f, 0);

            var hand = UIButton.create(atlas, "LoadingscreenZeiger.png", "LoadingscreenZeiger.png", 0, 0);
            hand.positionCenter();
            hand.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}