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
            lineRenderer.SetWidth(3F, 3F);
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
                if (debug) Debug.Log("send " + selected[i] + " to " + selected[selected.Count - 1]);
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
            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(screenPoint));
            lineRenderer.SetPosition(1, transform.position);
            if (debug) Debug.Log(Camera.main.ScreenToWorldPoint(screenPoint) + " to " + transform.position);

            if (selected.Count > 1)
                Debug.Log("selected more than one");
            //  Drawing.DrawLine(Registry.Instance.Orbs[selected[0]].go.transform.position, Input.mousePosition, Color.magenta, 10, true);

        }
    }
}
