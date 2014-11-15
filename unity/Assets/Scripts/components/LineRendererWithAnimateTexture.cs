using UnityEngine;

namespace Assets.Sources.components
{
    public class LineRendererWithAnimateTexture : MonoBehaviour
    {
        public bool Animate = true;
        public float ScrollSpeed = -1f;
        public float Tiling;

        public Transform Start;
        public Transform End;

        private LineRenderer _line;

        public void Awake()
        {
            _line = GetComponent<LineRenderer>();
        }

        public void Update()
        {
            if (Start == null || End == null)
                return;

            _line.SetVertexCount(2);
            _line.SetPosition(0, Start.position);
            _line.SetPosition(1, End.position);
//            RendereredLines.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));

            if (Animate)
            {
                var offset = Time.time*ScrollSpeed;
                _line.material.mainTextureOffset = new Vector2(offset, 0);
            }

            Tiling = Vector3.Distance(Start.position, End.position);
            _line.material.mainTextureScale = new Vector2(Tiling, 1);
        }
    }
}
