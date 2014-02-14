using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    /*
    // DestructibleObject.cs
    //
    // Written by: Eli (demonpants), modified slightly by Ben
    //
    // Functionality: Explode apart a mesh into individual GameObjects
    //
    // How it is done: Add to an object with a mesh renderer.
    // It reorganizes the vertices, splitting, and sending along the normal.
    //
    // Prospective Uses: 1. Spin as they move, rather than only moving linearly
    // 2. Be affected by gravity
    // 3. Bounce off other objects (with the help of, say, a box collider approximating the shape of the triangle)
    // 4. Make a sound when they bounce
    // 5. Cause damage when they hit other game entities
    //
    // Discussion/Source: http://forum.unity3d.com/threads/11112-Splitting-a-Mesh
    //
    /////////////////////////*/

    //DestructibleObject - provides functionality to destroy whatever GameObject the script is attached to
    //the attached object must have a MeshFilter in its children, and its shrapnel objects must have a renderer in the root.
    //by Eli

    public class DestructibleObject : MonoBehaviour
    {
        public GameObject shrapnelPrefab;
        //public AudioClip breakSound;
        public float lifespan = 10.0f;
        public float fadeTime = 0.5f;

        protected bool destroyed = false;
        protected float lifetime;
        protected GameObject[] pieces;

        public virtual void Update()
        {
            if (destroyed)
            {
                lifetime -= Time.deltaTime;

                //out of time, destroy this object
                if (lifetime <= 0.0f)
                {
                    Object.Destroy(gameObject);
                }
                //fade out before destroying
                else if (lifetime <= fadeTime)
                {
                    for (int i = 0; i < pieces.Length; i++)
                    {
                        Color c = pieces[i].renderer.material.color;
                        c.a = 1.0f - ((fadeTime - lifetime) / fadeTime);
                        pieces[i].renderer.material.color = c;
                    }
                }
            }
        }

        public virtual void Explode()
        {
            if (destroyed)
            {
                return;
            }
            destroyed = true;
            lifetime = lifespan + fadeTime;

            //construct all the individual destructible pieces from our mesh
            MeshFilter filter = GetComponentInChildren(typeof(MeshFilter)) as MeshFilter;
            Mesh mesh = filter.mesh;
            pieces = new GameObject[mesh.triangles.Length / 3];
            //a sneaky easy way to get the children to be sized correctly is to have a unit scale when spawning them, then restore it later
            Vector3 oldScale = transform.lossyScale;
            transform.localScale = new Vector3(1, 1, 1);

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                GameObject go = GameObject.Instantiate(shrapnelPrefab) as GameObject;
                Mesh newMesh = (go.GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
                newMesh.vertices = new Vector3[]
    {
    mesh.vertices[mesh.triangles[i+0]],
    mesh.vertices[mesh.triangles[i+1]],
    mesh.vertices[mesh.triangles[i+2]],
    mesh.vertices[mesh.triangles[i+0]] - mesh.normals[mesh.triangles[i+0]] * 0.15f, //need to turn this plane 3D
    mesh.vertices[mesh.triangles[i+1]] - mesh.normals[mesh.triangles[i+1]] * 0.15f,
    mesh.vertices[mesh.triangles[i+2]] - mesh.normals[mesh.triangles[i+2]] * 0.15f
    };
                newMesh.uv = new Vector2[]
    {
    mesh.uv[mesh.triangles[i+0]],
    mesh.uv[mesh.triangles[i+1]],
    mesh.uv[mesh.triangles[i+2]],
    mesh.uv[mesh.triangles[i+0]],
    mesh.uv[mesh.triangles[i+1]],
    mesh.uv[mesh.triangles[i+2]]
    };
                newMesh.triangles = new int[]
    {
    0, 2, 3,
    2, 5, 3,
    0, 3, 1,
    1, 3, 4,
    1, 4, 2,
    2, 4, 5,
    2, 0, 1,
    5, 4, 3
    };
                newMesh.RecalculateNormals();
                (go.collider as MeshCollider).sharedMesh = newMesh;
                go.transform.parent = filter.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                pieces[i / 3] = go;
            }
            mesh.triangles = new int[0];
            mesh.vertices = new Vector3[0];
            mesh.uv = new Vector2[0];
            mesh.normals = new Vector3[0];
            transform.localScale = oldScale;
            Object.Destroy(collider);
            //audio.PlayOneShot(breakSound);
        }

        public GameObject[] GetPieces()
        {
            return pieces;
        }
    }
}
