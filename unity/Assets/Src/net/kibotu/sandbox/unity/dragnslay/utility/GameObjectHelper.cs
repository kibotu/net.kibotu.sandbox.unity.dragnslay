using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    public static class GameObjectHelper
    {

        public static Vector3 Direction(this Vector3 source, Vector3 target) 
        {
            return (target - source).normalized;
        }

        /*public static Component GetComponentInChild(GameObject parent, string name, Component type)
        {
            return parent.transform.FindChild(name).GetComponent<type>();
        }*/
    }
}
