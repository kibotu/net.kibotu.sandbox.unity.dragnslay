using Assets.Sources.components.behaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Sources.model
{
    public class Prefabs : MonoBehaviour
    {
        public GameObject Papership;
        public GameObject Island;
        public GameObject Explosion;
        public GameObject Rocket;
        public GameObject Player;
        public GameObject Ai;
        public GameObject SmallExplosion;

        public void Awake()
        {
            Instance = this;
        }

        public static Prefabs Instance { get; private set; }

        private static GameObject CreateGameObject<T>(T type) where T : Object
        {
            if (type == null) Debug.LogError("Assigned Prefab missing. (Inspector)");
            return (GameObject) Instantiate(type); //, new Vector3(0, 0, 0), Quaternion.identity); 
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

        public GameObject GetNewSmallExplosion()
        {
            return CreateGameObject(Explosion);
        }

        public GameObject GetNewRocket()
        {
            var go = CreateGameObject(Rocket);
            go.AddComponent<RocketMove>();
            return go;
        }

        public GameObject GetNewPlayer()
        {
            return CreateGameObject(Player);
        }

        public GameObject GetNewAi()
        {
            return CreateGameObject(Ai);
        }
    }
}

