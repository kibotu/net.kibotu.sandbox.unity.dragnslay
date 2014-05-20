using System.Collections;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class LoadingScreen : MonoBehaviour
    {
        private GameObject _pendulum;
        private GameObject _clockhand;
        public float Speed = 2f;
        private float _startTime;
        private float Delay = 4f;

        public void Start()
        {
//            var atlas = GameObject.Find("main_menu_atlas").GetComponent<UIToolkit>();

//            var main = UIButton.create(atlas, "loadingscreenMain.png", "loadingscreenMain.png", 0, 0);
//            main.positionCenter();
//            main.scale = new Vector3(0.52083f, 0.520833f, 0);

//            var pendel = UIButton.create(atlas, "loadingscreenPendel.png", "loadingscreenPendel.png", 0, 0);
//            pendel.positionFromCenter(0.3f, 0);
//            pendel.scale = new Vector3(0.52083f, 0.520833f, 0);
//
//            var hand = UIButton.create(atlas, "LoadingscreenZeiger.png", "LoadingscreenZeiger.png", 0, 0);
//            hand.positionCenter();
//            hand.scale = new Vector3(0.52083f, 0.520833f, 0);

            _pendulum = GameObject.Find("loadingscreenPendel");
            _clockhand = GameObject.Find("LoadingscreenZeiger");
        }

        private bool isLoading = false;
        private float clockhandTime;
        private float progress;

        public void Update()
        {
            var deflection = 40f;
            _pendulum.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(-deflection, deflection, (Mathf.Sin(_startTime * Speed - Mathf.PI / 2.0f) + 1.0f) / 2.0f));
            _startTime += Time.deltaTime;

            deflection = 40f;
            _clockhand.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(deflection, -deflection, /*progress)); */ _startTime / Delay));

            StartCoroutine(LoadLevel(Registry.Levels.MainMenuAndIntro));
        }

        private IEnumerator LoadLevel(string screenToLoad)
        {
            yield return new WaitForSeconds(Delay);           // Wait until we did the fade out before we load.
            Debug.Log("Loading " + screenToLoad);
            var async = Application.LoadLevelAsync(screenToLoad);
            while(!async.isDone) // Level finished loading.
            {
                progress = async.progress;
                yield return async.progress;
            }

            Destroy(gameObject, 0.0f);
            Resources.UnloadUnusedAssets();
        }
    }
}