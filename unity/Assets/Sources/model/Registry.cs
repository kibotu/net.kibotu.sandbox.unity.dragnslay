using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.model
{
    public class Registry : MonoBehaviour
    {
        public enum Levels
        {
            IntroWithMainMenu, Training
        }

        public static readonly Dictionary<string, GameObject> Player = new Dictionary<string, GameObject>();
        public static readonly Dictionary<int, GameObject> Islands = new Dictionary<int, GameObject>();
        public static readonly Dictionary<int, GameObject> Ships = new Dictionary<int, GameObject>();

        public static void Init()
        {
            Player.Clear();
            Islands.Clear();
            Ships.Clear();
        }
    }
}