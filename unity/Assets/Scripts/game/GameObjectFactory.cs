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

        public static GameObject CreateShip(int uid, int type, PlayerData.PlayerType playerType)
        {
            GameObject go;
            switch (playerType)
            {
                case PlayerData.PlayerType.Offensive: 
                    go = Prefabs.Instance.GetNewSteelShip();
                    break;
                case PlayerData.PlayerType.Player:
                    go = Prefabs.Instance.GetNewPapership();
                    break;
                default:
                    go = Prefabs.Instance.GetNewPapership();
                    break;
            }
            Registry.Ships.Add(uid, go);
            return go;
        }

        public static GameObject CreatePlayer(string uid)
        {
            var go = Prefabs.Instance.GetNewPlayer();
            var playerData = go.GetComponent<PlayerData>();
            playerData.uid = go.name = uid;
            playerData.playerType = PlayerData.PlayerType.Player;

            Registry.Player.Add(uid, go);

            return go;
        }

        public static GameObject CreateAi(string uid)
        {
            var go = Prefabs.Instance.GetNewAi();
            var playerData = go.GetComponent<PlayerData>();
            playerData.uid = go.name = uid;
            playerData.playerType = PlayerData.PlayerType.Offensive;

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
    }
}
