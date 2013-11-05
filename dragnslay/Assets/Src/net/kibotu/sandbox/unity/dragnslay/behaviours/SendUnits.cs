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

        /**
         * a += i1
         * b += i2
         * 
         * (b-a) * t + a
         * t = [0...1]
         */
        private void Send()
        {
            for (int i = 0; i < selected.Count - 1; ++i)
            {
                if (debug) Debug.Log("send " + selected[i] + " to " + selected[selected.Count - 1]);

                Orb source = Registry.Instance.Orbs[selected[i]];
                Orb destination = Registry.Instance.Orbs[selected[selected.Count - 1]];

                foreach (KeyValuePair<int, TrabantPrototype> pair in Registry.Instance.Planes)
                {
                    if (debug) Debug.Log("bla: " + (source.go.transform == Registry.Instance.Planes[pair.Key].go.transform.parent));
                    if (source.go.transform == Registry.Instance.Planes[pair.Key].go.transform.parent)
                    {
                        GameObject plane = Registry.Instance.Planes[pair.Key].go;
                        plane.AddComponent<Move>();
                        Move move = plane.GetComponent<Move>();
                        move.speed = 50;
                        move.destination = destination.go.transform.FindChild("Sphere");
                    }
                }
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
            // line rendering
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

            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                    RaycastHit hit;   

                    if (collider && collider.Raycast(ray, out hit, 100.0f))
                    {
                        OnMouseDown();
                    }
                }
            }

        }
    }
}
