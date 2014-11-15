using Assets.Sources.components.behaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Sources.model
{
    public class Prefabs : MonoBehaviour
    {
        public GameObject Empty;
        public GameObject Papership;
        public GameObject Steelship;
        public GameObject Island;
        public GameObject Explosion;
        public GameObject Rocket;
        public GameObject Player;
        public GameObject Ai;
        public GameObject SmallExplosion;
        public GameObject Shockwave;
		public GameObject Shield;

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
            //go.AddComponent<RocketMove>();
            //go.transform.localScale = new Vector3(15f, 15f, 15f);
            return go;
        }

        public GameObject GetNewPlayer()
        {
            var go = CreateGameObject(Player);
            AddToPlayers(go);
            return go;
        }

        private void AddToPlayers(GameObject go)
        {
            var player = GameObject.Find("Players");
            if (player == null)
            {
                var empty = CreateGameObject(Empty);
                empty.name = "Players";
                empty.transform.parent = GameObject.Find("Game").transform;
                go.transform.parent = empty.transform;
            }
            else
                go.transform.parent = player.transform;
        }

        public GameObject GetNewAi()
        {
            var go = CreateGameObject(Ai);
            AddToPlayers(go);
            return go;
        }

        public GameObject GetNewShockwave()
        {
            return CreateGameObject(Shockwave);
        }

        public GameObject GetNewSteelShip()
        {
            return CreateGameObject(Steelship);
        }

		public GameObject GetNewShield()
		{
			return CreateGameObject(Shield);
		}
    }
}

