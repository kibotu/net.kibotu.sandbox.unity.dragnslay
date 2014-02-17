using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.model
{
    public class Registry : MonoBehaviour
    {
        public static readonly Dictionary<int, GameObject> Islands = new Dictionary<int, GameObject>();
        public static readonly Dictionary<int, GameObject> Ships = new Dictionary<int, GameObject>();
    }
}