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

            return orb;
        }
    }
}
