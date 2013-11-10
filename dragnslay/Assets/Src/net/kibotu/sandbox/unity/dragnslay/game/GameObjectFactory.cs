using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    class GameObjectFactory
    {
        // static factory class
        private GameObjectFactory()
        {
        }

        public static void AddMeshToGameObject(GameObject go, string meshUrl, string textureUrl)
        {
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load(meshUrl, typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load(textureUrl, typeof(Texture)) as Texture;
        }

        public static GameObject CreateIsland()
        {
            var uid = UidGenerator.GetNewUid();
            var go = new GameObject("Island_" + uid);
            AddMeshToGameObject(go, "meshes/iland", "meshes/iland");
            go.AddComponent<MetaData>().uid = uid;
            go.AddComponent<SphereCollider>();
            go.AddComponent<Rotating>();
            go.AddComponent<SpawnUnits>();
            go.AddComponent<SendUnits>();
            go.AddComponent<LifeData>();

            Registry.Instance.GameObjects.Add(go.GetInstanceID(), go);

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(4,4,4);
            sphere.transform.parent = go.transform; 
            sphere.renderer.enabled = false; 
            sphere.layer = 2; // Raycast ignore

            return go;
        }

        public static GameObject CreatePlane()
        {
            var uid = UidGenerator.GetNewUid();
            var go = new GameObject("Papership_" + uid);
            AddMeshToGameObject(go, "meshes/papership", "meshes/paperplant");
            go.AddComponent<MetaData>().uid = uid;
            go.AddComponent<SphereCollider>();
            go.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // no physic reactions 
            go.AddComponent<LifeData>();
            
            go.transform.position = new Vector3(60, 0, 0);

            return go;
        }
    }
}
