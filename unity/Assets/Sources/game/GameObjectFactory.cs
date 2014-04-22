using Assets.Sources.components.behaviours;
using Assets.Sources.components.behaviours.depricated;
using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.game
{
    public static class GameObjectFactory
    {
        public static void AddMeshToGameObject(GameObject go, string meshUrl, string textureUrl)
        {
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load(meshUrl, typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load(textureUrl, typeof(Texture)) as Texture;
        }

        public static GameObject CreateIsland(int uid, int type)
        {
            return CreateIsland(uid);
        }

        public static GameObject CreateShip(int uid, int type)
        {
            return CreateShip(uid);
        }

        public static GameObject CreatePlayer(string uid)
        {
            var go = Prefabs.Instance.GetNewPlayer();
            go.GetComponent<PlayerData>().uid = go.name = uid;

            Registry.Player.Add(uid, go);

            return go;
        }

        public static GameObject CreateAi(string uid)
        {
            var go = Prefabs.Instance.GetNewAi();
            go.GetComponent<PlayerData>().uid = uid;

            Registry.Player.Add(uid, go);

            return go;
        }

        private static GameObject CreateIsland(int uid)
        {
            //var go = new GameObject("Island_" + uid);
            //AddMeshToGameObject(go, "meshes/iland", "meshes/iland");
            var go = Prefabs.Instance.GetNewIsland();
            go.name = "Island_" + uid;
            go.AddComponent<SphereCollider>().radius += 1f;

            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(4,4,4);
            sphere.transform.parent = go.transform; 
            sphere.renderer.enabled = false; 
            sphere.layer = 2; // Raycast ignore

            // add island to registry
            Registry.Islands.Add(uid, go);

            return go;
        }

        public static GameObject CreateShip(int uid)
        {
            // var go = new GameObject("Papership_" + uid);
            //AddMeshToGameObject(go, "meshes/papership", "meshes/paperplant");
            var go = Prefabs.Instance.GetNewPapership();

            //go.transform.position = new Vector3(70, 0, 0);

            // add island to registry
            Registry.Ships.Add(uid, go);

            return go;
        }
    }
}
