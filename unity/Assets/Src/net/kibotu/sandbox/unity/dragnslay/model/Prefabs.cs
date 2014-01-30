using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    public class Prefabs : MonoBehaviour
    {
        public GameObject Papership;
        public GameObject Island;
        public GameObject Explosion;
        public GameObject Rocket;

        public void Start()
        {
            Instance = this;
        }

        public static Prefabs Instance { get; private set; }

        private static GameObject CreateGameObject<T>(T type) where T : Object
        {
            return (GameObject) Instantiate(type, new Vector3(0, 0, 0), Quaternion.identity); 
        }

        public GameObject GetNewPapership()
        {
            return CreateGameObject(Papership);
        }

        public GameObject GetNewIsland()
        {
            return CreateGameObject(Island);
        }

        public GameObject GetNewExplosion()
        {
            return CreateGameObject(Explosion);
        }

        public GameObject GetNewRocket()
        {
            return CreateGameObject(Rocket);
        }
    }
}

