using UnityEngine;

namespace Assets.Source
{
    public class LinesRenderer  : MonoBehaviour
    {
        private Mesh _mesh;
        public Shader Shader;
        public Material Material;
        public float LineSize = 0.03f;

        public void Awake()
        {
            _mesh = new Mesh();
//            Material = new Material(Shader) {color = new Color(1, 0, 0, 1f)};
        }

        public void AddLine(Vector3 p, Vector3 q)
        {
            _mesh.AddLine(transform.MakeQuad(p, q, LineSize), false);
        }

        public void AddLine(Vector2 p, Vector2 q)
        {
            _mesh.AddLine(transform.MakeQuad(p, q, LineSize), false);
        }

        public void Draw()
        {
            Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, Material, 0);
        }

        private void Update()
        {
            Draw();
        }

        public void Reset()
        {
            _mesh.Clear();
        }
    }
}
