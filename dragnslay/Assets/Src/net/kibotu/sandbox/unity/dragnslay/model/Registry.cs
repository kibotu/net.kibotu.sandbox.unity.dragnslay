using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    class Registry : MonoBehaviour
    {
        public Dictionary<int, Orb> Orbs;

        private static Registry _instance;

        private Registry()
        {
            Orbs = new Dictionary<int, Orb>();
        }

        public static Registry Instance
        {
            get { return _instance ?? ( _instance = new GameObject("Registry").AddComponent<Registry>()); }
        }
    }
}