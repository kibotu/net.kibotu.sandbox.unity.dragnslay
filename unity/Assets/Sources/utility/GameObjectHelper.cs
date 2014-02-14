using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

namespace Assets.Sources.utility
{
    public static class GameObjectHelper
    {
        /*public static Component GetComponentInChild(GameObject parent, string name, Component type)
        {
            return parent.transform.FindChild(name).GetComponent<type>();
        }*/
		
		public static float Range2(this Random random, float min, float max, float excludeMin, float excludeMax) {
			return 0f;
		}
    }
}
