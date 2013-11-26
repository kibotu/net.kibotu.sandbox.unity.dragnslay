using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.States;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
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
        private Color oldColor;

        public void Start()
        {
            initLineRender();

            isDragging = false;
            isOver = false;
            id = gameObject.GetInstanceID();
            if(selected == null) selected = new List<int>();
            oldColor = renderer.material.color;
        }

        private void initLineRender()
        {
            Color c1 = Color.yellow;
            Color c2 = Color.red;
            const int lengthOfLineRenderer = 2;
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            // @see http://answers.unity3d.com/questions/57303/changing-replacement-shaders-at-runtime.html
            lineRenderer.material = new Material(Resources.Load("Shaders/Mobile Particles Additive Culled", typeof(Shader)) as Shader);
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
                renderer.material.color = new Color(1.5f,2.0f,1.5f,1.0f);
                if (debug) Debug.Log("select " + id);
            }
        }

        public void OnMouseEnter()
        {
            isOver = true;
            if (isDragging && !selected.Contains(id))
            {
                selected.Add(id);
                renderer.material.color = new Color(1.5f, 2.0f, 1.5f, 1.0f);
                if (debug) Debug.Log("select " + id);
            }
            else if(selected.Contains(id))
            {
                selected.Remove(id);
                renderer.material.color = oldColor;
                if(debug) Debug.Log("deselect " + id);
            }
        }

        public void OnMouseDrag()
        {
            isDragging = true;
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
                Registry.Instance.GameObjects[selected[i]].renderer.material.color = oldColor;
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
            for (var i = 0; i < selected.Count - 1; ++i)
            {
                if (debug) Debug.Log("send " + selected[i] + " to " + selected[selected.Count - 1]);

                var source = Registry.Instance.GameObjects[selected[i]];
                var destination = Registry.Instance.GameObjects[selected[selected.Count - 1]];

                var toMovePlanes = new List<int>();

                foreach (var pair in Registry.Instance.Planes)
                {
                    if (debug) Debug.Log("bla: " + (source.transform == Registry.Instance.Planes[pair.Key].transform.parent));
                    if (source.transform == Registry.Instance.Planes[pair.Key].transform.parent)
                    {
                        var plane = Registry.Instance.Planes[pair.Key];
                        plane.AddComponent<Move>();
                        var move = plane.GetComponent<Move>();
                        move.speed = 25;
                        move.destination = destination.transform.FindChild("Sphere");
                        toMovePlanes.Add(plane.GetComponent<MetaData>().uid);
                    }
                }
                if (toMovePlanes.Count > 0) SocketHandler.Instance.Emit("send", PackageFactory.CreateSendUnitsMessage(destination.GetInstanceID(), toMovePlanes.ToArray()));
            }
        }

        public void Update()
        {

            // line rendering
            var lineRenderer = GetComponent<LineRenderer>();
            if (selected.Count > 1 && selected.Contains(id))
            {
                lineRenderer.SetVertexCount(3);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, Registry.Instance.GameObjects[selected[selected.Count - 1]].transform.position);
                lineRenderer.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
                
            }
            else
            {
                lineRenderer.SetVertexCount(0);
            }
        }
    }
}
