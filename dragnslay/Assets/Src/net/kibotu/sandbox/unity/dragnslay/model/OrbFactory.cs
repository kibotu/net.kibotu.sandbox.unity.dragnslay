using Assets.Src.net.kibotu.sandbox.unity.dragnslay.scripts;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    class OrbFactory
    {

        private OrbFactory()
        {
        }

        public static Orb createIsland()
        {
            GameObject go = new GameObject("Island_" + UidGenerator.GetNewUid());
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/island", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/island", typeof(Texture)) as Texture;

            Orb orb = new Orb(go.GetInstanceID(), 0, null, null, 0, 1, 0,
                new Life(10,10,0,0,0,0,0), 
                new PhysicalProperty(new Vector3(0,0,0),new Vector3(1,1,1),new Quaternion(0,0,0,0),0,0,0,0 ));

            orb.go = go;
            go.transform.position = orb.physicalProperty.position;

            go.AddComponent<Rotating>();
            go.AddComponent<SendUnits>();
            go.AddComponent<SpawnUnits>();
            go.AddComponent<SphereCollider>();

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(10, 10, 10);
            sphere.transform.parent = go.transform;
            sphere.renderer.enabled = false;
            sphere.layer = 2; // Raycast ignore

            Registry.Instance.Orbs.Add(go.GetInstanceID(), orb);

            return orb;
        }

        public static TrabantPrototype createPlane()
        {
            TrabantPrototype plane = new TrabantPrototype(0,0,null,
                new Life(10,10,0,0,0,0,0), 
                new PhysicalProperty(new Vector3(60,0,0), new Vector3(1f,1f,1f), new Quaternion(0,0,0,0), 0,0,0,0));

            GameObject go = new GameObject("Papership_" + UidGenerator.GetNewUid());
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/papership", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/paperplant", typeof(Texture)) as Texture;
            
            go.transform.position = plane.physicalProperty.position;
            go.transform.localRotation = plane.physicalProperty.rotation;
            go.transform.localScale = plane.physicalProperty.scalling;

            plane.go = go;
            go.AddComponent<SphereCollider>();
            go.AddComponent<Rigidbody>();
            go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            return plane;
        }
    }
}
