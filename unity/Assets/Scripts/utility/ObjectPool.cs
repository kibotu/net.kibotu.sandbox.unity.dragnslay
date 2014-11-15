using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.utility
{
    // @see https://github.com/UnityPatterns/ObjectPool
    public sealed class ObjectPool : MonoBehaviour
    {
        static ObjectPool _instance;

        readonly Dictionary<Component, List<Component>> _objectLookup = new Dictionary<Component, List<Component>>();
        readonly Dictionary<Component, Component> _prefabLookup = new Dictionary<Component, Component>();

        public static void Clear()
        {
            Instance._objectLookup.Clear();
            Instance._prefabLookup.Clear();
        }

        public static void CreatePool<T>(T prefab) where T : Component
        {
            if (!Instance._objectLookup.ContainsKey(prefab))
                Instance._objectLookup.Add(prefab, new List<Component>());
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            if (!Instance._objectLookup.ContainsKey(prefab)) return (T) Instantiate(prefab, position, rotation);
            T obj = null;
            var list = Instance._objectLookup[prefab];
            if (list.Count > 0)
            {
                while (obj == null && list.Count > 0)
                {
                    obj = list[0] as T;
                    list.RemoveAt(0);
                }
                if (obj != null)
                {
                    obj.transform.parent = null;
                    obj.transform.localPosition = position;
                    obj.transform.localRotation = rotation;
                    obj.gameObject.SetActive(true);
                    Instance._prefabLookup.Add(obj, prefab);
                    return obj;
                }
            }
            obj = (T)Instantiate(prefab, position, rotation);
            Instance._prefabLookup.Add(obj, prefab);
            return obj;
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab, position, Quaternion.identity);
        }
        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            if (Instance._prefabLookup.ContainsKey(obj))
            {
                Instance._objectLookup[Instance._prefabLookup[obj]].Add(obj);
                Instance._prefabLookup.Remove(obj);
                obj.transform.parent = Instance.transform;
                obj.gameObject.SetActive(false);
            }
            else
                Destroy(obj.gameObject);
        }

        public static int Count<T>(T prefab) where T : Component
        {
            return Instance._objectLookup.ContainsKey(prefab) ? Instance._objectLookup[prefab].Count : 0;
        }

        public static ObjectPool Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                var obj = new GameObject("_ObjectPool");
                obj.transform.localPosition = Vector3.zero;
                _instance = obj.AddComponent<ObjectPool>();
                return _instance;
            }
        }
    }

    public static class ObjectPoolExtensions
    {
        public static void CreatePool<T>(this T prefab) where T : Component
        {
            ObjectPool.CreatePool(prefab);
        }

        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, position, rotation);
        }
        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, position, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPool.Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            ObjectPool.Recycle(obj);
        }

        public static int Count<T>(T prefab) where T : Component
        {
            return ObjectPool.Count(prefab);
        }
    }
}