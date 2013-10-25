using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.scripts
{
    // @ see http://answers.unity3d.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html
    public class SendUnits : MonoBehaviour
    {
        private static string LOGGING_TAG = "SendUnits";
        private static bool debug = false;
        private static bool isDragging;
        private static bool isOver;
        private static List<int> selected;
        private int id;

        public static float getY(float x)
        {
            return - ( x * x ) + 2 * x;
        }

        public static Vector3 move(Vector3 end, Vector3 start, float dt, float maxTime)
        {
            // (b - a) * t + a
            return Vector3.MoveTowards(end, start, Vector3.Distance(end, start) * dt/maxTime);
        }

        public void Start()
        {
            initLineRender();

            isDragging = false;
            isOver = false;
            id = gameObject.GetInstanceID();
            if(selected == null) selected = new List<int>();
        }

        private void initLineRender()
        {
            Color c1 = Color.yellow;
            Color c2 = Color.red;
            const int lengthOfLineRenderer = 2;
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.SetColors(c1, c2);
            lineRenderer.SetWidth(10F, 10F);
            lineRenderer.castShadows = false;
            lineRenderer.receiveShadows = false;
            lineRenderer.SetVertexCount(lengthOfLineRenderer);
        }

        public void OnMouseDown()
        {
            isDragging = true;
            isOver = true;

            if (isDragging && !selected.Contains(id))
            {
                selected.Add(id);
                if (debug) Debug.Log("select " + id);
            }
        }

        public void OnMouseEnter()
        {
            isOver = true;
            if (isDragging && !selected.Contains(id))
            {
                selected.Add(id);
                if (debug) Debug.Log("select " + id);
            }
            else if(selected.Contains(id))
            {
                selected.Remove(id);
                if (debug) Debug.Log("deselect " + id);
            }
        }

        public void OnGUI()
        {
        }

        public void OnMouseExit()
        {
            isOver = false;
        }

        public void OnMouseUp()
        {
            isDragging = false;

            if (isOver && selected.Count > 1)
            {
                Send();
            }

            DeselectAll();
        }

        private void DeselectAll()
        {
            for (int i = 0; i < selected.Count; ++i)
            {
                if (debug) Debug.Log("deselect " + selected[i]);
            }
            selected.Clear();
        }

        private void Send()
        {
            for (int i = 0; i < selected.Count - 1; ++i)
            {
                if (debug || true ) Debug.Log("send " + selected[i] + " to " + selected[selected.Count - 1]);

                Orb source = Registry.Instance.Orbs[selected[i]];
                Orb destination = Registry.Instance.Orbs[selected[selected.Count - 1]];


                //TrabantPrototype plane = Registry.Instance.Planes[source.go.transform.GetChild(0).GetInstanceID()];
                //Debug.Log("child id: " + source.go.transform.GetChild(0).GetInstanceID());

                foreach (KeyValuePair<int, TrabantPrototype> pair in Registry.Instance.Planes)
                {
                    //Debug.Log("child " + i + " " + pair.Key);

                }

                    //TrabantPrototype plane = Registry.Instance.Planes[destination.go.transform.GetChild(0).GetInstanceID()];
                    //plane.go.transform.position = move(destination.go.transform.position + plane.go.transform.position, destination.go.transform.position + plane.go.transform.position, 0.5f, 1f);
            }

            /*
            if (Registry.Instance.Orbs.ContainsKey(id))
            {
                Orb parent = Registry.Instance.Orbs[id];
                //Debug.Log(parent.id);
            }
            else
            {

            }*/

            // SocketHandler.Instance.Emit("send", SocketHandler.Instance.createSendUnitsMessage());
        }

        public void Update()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            if (selected.Count > 1 && selected.Contains(id))
            {
                lineRenderer.SetVertexCount(3);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, Registry.Instance.Orbs[selected[selected.Count - 1]].go.transform.position);
                lineRenderer.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
            }
            else
            {
                lineRenderer.SetVertexCount(0);
            }
        }
    }
}
