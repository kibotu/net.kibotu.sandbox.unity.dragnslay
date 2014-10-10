using UnityEngine;

namespace Assets.Source
{
    static class MeshUtils
    {
        public static void AddLine(this Mesh m, Vector3[] quad, bool tmp)
        {
            int vl = m.vertices.Length;

            Vector3[] vs = m.vertices;
            if (!tmp || vl == 0) vs = ResizeVertices(vs, 4);
            else vl -= 4;

            vs[vl] = quad[0];
            vs[vl + 1] = quad[1];
            vs[vl + 2] = quad[2];
            vs[vl + 3] = quad[3];

            int tl = m.triangles.Length;

            int[] ts = m.triangles;
            if (!tmp || tl == 0) ts = ResizeTraingles(ts, 6);
            else tl -= 6;
            ts[tl] = vl;
            ts[tl + 1] = vl + 1;
            ts[tl + 2] = vl + 2;
            ts[tl + 3] = vl + 1;
            ts[tl + 4] = vl + 3;
            ts[tl + 5] = vl + 2;

            m.vertices = vs;
            m.triangles = ts;
            m.RecalculateBounds();
        }

        public static Vector3[] ResizeVertices(Vector3[] ovs, int ns)
        {
            Vector3[] nvs = new Vector3[ovs.Length + ns];
            for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
            return nvs;
        }

        public static int[] ResizeTraingles(int[] ovs, int ns)
        {
            int[] nvs = new int[ovs.Length + ns];
            for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
            return nvs;
        }

        public static Vector3[] MakeQuad(this Transform transform, Vector3 p, Vector3 q, float width)
        {
            width = width / 2;
            Vector3[] quad = new Vector3[4];

            Vector3 n = Vector3.Cross(p, q);
            Vector3 l = Vector3.Cross(n, q - p);
            l.Normalize();

            quad[0] = transform.InverseTransformPoint(p + l * width);
            quad[1] = transform.InverseTransformPoint(p + l * -width);
            quad[2] = transform.InverseTransformPoint(q + l * width);
            quad[3] = transform.InverseTransformPoint(q + l * -width);

            return quad;
        }
    }
}
