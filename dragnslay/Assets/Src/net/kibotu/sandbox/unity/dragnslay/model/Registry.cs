using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    class Registry : MonoBehaviour
    {
        public Dictionary<int, GameObject> GameObjects;
        public Dictionary<int, GameObject> Planes;

        private static Registry _instance;

        private Registry()
        {
            GameObjects = new Dictionary<int, GameObject>();
            Planes = new Dictionary<int, GameObject>();
        }

        public static Registry Instance
        {
            get { return _instance ?? ( _instance = new GameObject("Registry").AddComponent<Registry>()); }
        }
    }
}