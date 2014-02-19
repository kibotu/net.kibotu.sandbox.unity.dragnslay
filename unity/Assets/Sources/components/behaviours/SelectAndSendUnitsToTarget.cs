using System.Collections.Generic;
using System.Linq;
using Assets.Sources.components.data;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SelectAndSendUnitsToTarget : MonoBehaviour {

        public static readonly List<int> Selected = new List<int>();
        private int _uid = -1;
        private Color _oldColor;
        private readonly Color _colorHighlight = new Color(0.5f, 0.5f, 0.5f);

        public int Uid
        {
            get { return _uid == -1 ? _uid = GetComponent<IslandData>().uid : _uid; }
        }

        public void Start()
        {
            InitLineRender();
            _oldColor = renderer.material.color;
        }

        private void DyeSelected()
        {
            renderer.material.color += _colorHighlight;
        }

        private void RestoreColor()
        {
            renderer.material.color = _oldColor;
        }

        public void Select()
        {
            DyeSelected();
            if (!Selected.Contains(Uid)) Selected.Add(Uid);
        }

        public void Deselect()
        {
            RestoreColor();
            Selected.Remove(Uid);
        }

        public void SendUnits()
        {
            if (Selected.IsEmpty()) return;

            var target = Registry.Islands[Selected.Last()];
            
            // 1) already there
            if (target == null || target == gameObject) return;

            // 3) send own units to destination
            for (var i = 0; i < transform.childCount; ++i)
            {
                var papership = transform.GetChild(i);
                if (papership.name.StartsWith("Papership"))
                {
                    if (papership.GetComponent<PlayMakerFSM>().ActiveStateName != "Moving")
                    {
                        var move = papership.gameObject.AddComponent<MoveToTarget>();
                        move.target = target;
                        Deselect();
                    }
                }
            }
        }

        private void InitLineRender()
        {
            var c1 = Color.yellow;
            var c2 = Color.red;
            const int lengthOfLineRenderer = 2;
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            // @see http://answers.unity3d.com/questions/57303/changing-replacement-shaders-at-runtime.html
            lineRenderer.material = new Material(Resources.Load("Shader/Mobile Particles Additive Culled", typeof(Shader)) as Shader);
            lineRenderer.SetColors(c1, c2);
            lineRenderer.SetWidth(0.3f, 0.3f);
            lineRenderer.castShadows = false;
            lineRenderer.receiveShadows = false;
            lineRenderer.SetVertexCount(lengthOfLineRenderer);
        }

        public void Update()
        {
            // line rendering
            var lineRenderer = GetComponent<LineRenderer>();
            if (!Selected.IsEmpty() && Selected.Contains(Uid))
            {
                lineRenderer.SetVertexCount(3);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, Registry.Islands[Selected.Last()].transform.position);
                lineRenderer.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
            }
            else
            {
                lineRenderer.SetVertexCount(0);
            }
        }
    }
}