using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    // @ see http://answers.unity3d.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html
    public class SendUnits : MonoBehaviour
    {
        private static string LOGGING_TAG = "SendUnits";
        private const bool Debug = false;
        private static bool _isDragging;
        private static bool _isOver;
        private static List<int> _selected;
        private int _id;
        private Color _oldColor;

        public void Start()
        {
            InitLineRender();

            _isDragging = false;
            _isOver = false;
            _id = gameObject.GetComponent<IslandData>().uid;
            if(_selected == null) _selected = new List<int>();
            _oldColor = renderer.material.color;
        }

        private void InitLineRender()
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
            _isDragging = true;
            _isOver = true;

            if (!_isDragging || _selected.Contains(_id)) return;

            _selected.Add(_id);
            renderer.material.color = new Color(1.5f,2.0f,1.5f,1.0f);
            if (Debug) UnityEngine.Debug.Log("select " + _id);
        }

        public void OnMouseEnter()
        {
            _isOver = true;
            if (_isDragging && !_selected.Contains(_id))
            {
                _selected.Add(_id);
                renderer.material.color = new Color(1.5f, 2.0f, 1.5f, 1.0f);
                if (Debug) UnityEngine.Debug.Log("select " + _id);
            }
            else if(_selected.Contains(_id))
            {
                _selected.Remove(_id);
                renderer.material.color = _oldColor;
                if(Debug) UnityEngine.Debug.Log("deselect " + _id);
            }
        }

        public void OnMouseDrag()
        {
            _isDragging = true;
        }

        public void OnMouseExit()
        {
            _isOver = false;
        }

        public void OnMouseUp()
        {
            _isDragging = false;

            if (_isOver && _selected.Count > 1)
            {
                Send();
            }
            else
            {
            }

            DeselectAll();
        }

        private void DeselectAll()
        {
            for (int i = 0; i < _selected.Count; ++i)
            {
                if (Debug) UnityEngine.Debug.Log("deselect " + _selected[i]);
                Registry.Instance.Islands[_selected[i]].renderer.material.color = _oldColor;
            }
            _selected.Clear();
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
            for (var i = 0; i < _selected.Count - 1; ++i)
            {
                if (Debug) UnityEngine.Debug.Log("send " + _selected[i] + " to " + _selected[_selected.Count - 1]);

                var source = Registry.Instance.Islands[_selected[i]];
                var destination = Registry.Instance.Islands[_selected[_selected.Count - 1]];

                var toMovePlanes = new List<int>();

                foreach (var pair in Registry.Instance.Ships)
                {
                    if (Debug) UnityEngine.Debug.Log("bla: " + (source.transform == Registry.Instance.Ships[pair.Key].transform.parent));
                    if (source.transform == Registry.Instance.Ships[pair.Key].transform.parent)
                    {
                        var plane = Registry.Instance.Ships[pair.Key];

                        // only move if you own it
                        if (plane.GetComponent<ShipData>().playerUid != Game.ClientUid) continue;
                        
                        plane.AddComponent<Move>();
                        var move = plane.GetComponent<Move>();
                        move.speed = 25;
                        move.destination = destination.transform.FindChild("Sphere");
                    }
                }
                if (toMovePlanes.Count > 0) SocketHandler.SharedConnection.Emit("move-units", PackageFactory.CreateSendUnitsMessage(destination.GetInstanceID(), toMovePlanes.ToArray()));
            }
        }

        public void Update()
        {
            // line rendering
            var lineRenderer = GetComponent<LineRenderer>();
            if (_selected.Count >= 1 && _selected.Contains(_id))
            {
                lineRenderer.SetVertexCount(3);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, Registry.Instance.Islands[_selected[_selected.Count - 1]].transform.position);
                lineRenderer.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
            }
            else
            {
                lineRenderer.SetVertexCount(0);
            }
        }
    }
}
