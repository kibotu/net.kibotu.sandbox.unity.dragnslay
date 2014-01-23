using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    public class Prefabs : MonoBehaviour
    {
        public GameObject Papership;
        public GameObject Island;

        public void Start()
        {
            Instance = this;
        }

        public static Prefabs Instance { get; private set; }

        public GameObject GetNewPapership()
        {
            return (GameObject) Instantiate(Papership, new Vector3(0, 0, 0), Quaternion.identity);
        }

        public GameObject GetNewIsland()
        {
            return (GameObject)Instantiate(Island, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}

