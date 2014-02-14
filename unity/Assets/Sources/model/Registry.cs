using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.model
{
    class Registry : MonoBehaviour
    {
        public Dictionary<int, GameObject> Islands;
        public Dictionary<int, GameObject> Ships;

        private static Registry _instance;

        private Registry()
        {
            Islands = new Dictionary<int, GameObject>();
            Ships = new Dictionary<int, GameObject>();
        }

        public static Registry Instance { get { return _instance ?? ( _instance = new GameObject("Registry").AddComponent<Registry>()); }}
    }
}