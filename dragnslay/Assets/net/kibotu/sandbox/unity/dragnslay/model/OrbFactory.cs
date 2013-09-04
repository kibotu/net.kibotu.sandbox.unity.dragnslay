using Assets.net.kibotu.sandbox.unity.dragnslay.scripts;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.model
{
    class OrbFactory
    {

        private OrbFactory()
        {
        }

        public static Orb createIsland()
        {
            Orb orb = new Orb("island",0,null, null, 0,1,0,
                new Life(10,10,0,0,0,0,0), 
                new PhysicalProperty(new Vector3(0,0,0),new Vector3(1,1,1),new Quaternion(0,0,0,0),0,0,0,0 ));

            GameObject go = new GameObject("island");
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/island", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/island", typeof(Texture)) as Texture;

            orb.go = go;
            go.transform.position = orb.physicalProperty.position;

            go.AddComponent<Orbitting>();
            go.AddComponent<SendUnits>();
            go.AddComponent<SpawnUnits>();
            go.AddComponent("SphereCollider"); 

            return orb;
        }

        public static TrabantPrototype createPlane()
        {
            TrabantPrototype plane = new TrabantPrototype(0,0,null,
                new Life(10,10,0,0,0,0,0), 
                new PhysicalProperty(new Vector3(0,0,0), new Vector3(0.3f,0.3f,0.3f), new Quaternion(0,0,0,0), 0,0,0,0));

            GameObject go = new GameObject("papership");
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            filter.mesh = Resources.Load("meshes/papership", typeof(Mesh)) as Mesh;
            renderer.material.mainTexture = Resources.Load("meshes/paperplant", typeof(Texture)) as Texture;
            
            plane.go = go;
            go.transform.position = plane.physicalProperty.position;
            go.transform.localRotation = plane.physicalProperty.rotation;
            go.transform.localScale = plane.physicalProperty.scalling;

            return plane;
        }
    }
}
