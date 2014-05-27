using System.Collections.Generic;
using System.Linq;
using Assets.Sources.components.behaviours.camera;
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
        public IslandData Island;
        private LineRenderer _lineRenderer;
        private MoveCamera cameraMovement;

        public int Uid
        {
            get { return _uid == -1 ? _uid = GetComponent<IslandData>().Uid : _uid; }
        }

        public void Start()
        {
            InitLineRender();
            Island = GetComponent<IslandData>();
            cameraMovement = GameObject.Find("Main Camera").GetComponent<MoveCamera>();
        }

        private void DyeSelected()
        {
            renderer.material.color += _colorHighlight;
        }

        private void RestoreColor()
        {
            renderer.material.color = Island.PlayerData.color;
        }

        public void Select()
        {
            DyeSelected();
            if (!Selected.Contains(Uid)) Selected.Add(Uid);
            cameraMovement.enabled = false;
        }

        public void Deselect()
        {
            RestoreColor();
            Selected.Remove(Uid);
            cameraMovement.enabled = true;
        }

        public void SendUnits()
        {
            if (Selected.IsEmpty()) return;

            var target = Registry.Islands[Selected.Last()];
            
            // 1) already there
            if (target == null || target == gameObject) return;

            Send(target);
        }

        protected virtual void Send(GameObject target)
        {
            // 2) send own units to destination
            for (var i = 0; i < transform.childCount; ++i)
            {
                var ship = transform.GetChild(i);
                if (ship.name.StartsWith("Papership") || ship.name.StartsWith("Steelship"))
                {
                    if (ship.GetComponent<ShipData>().PlayerData.playerType == PlayerData.PlayerType.Player && ship.GetComponent<PlayMakerFSM>().ActiveStateName != "Moving")
                    {
                        var move = ship.gameObject.AddComponent<MoveToTarget>();
                        move.Target = target;
                        Deselect();
                    }
                }
            }
        }

        private void InitLineRender()
        {
            var c1 = Color.green;
            var c2 = Color.red;
            const int lengthOfLineRenderer = 2;
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            // @see http://answers.unity3d.com/questions/57303/changing-replacement-shaders-at-runtime.html
            _lineRenderer.material = new Material(Resources.Load("Shader/Mobile Particles Additive Culled", typeof(Shader)) as Shader);
            _lineRenderer.SetColors(c1, c2);
            _lineRenderer.SetWidth(0.4f, 0.4f);
            _lineRenderer.castShadows = false;
            _lineRenderer.receiveShadows = false;
            _lineRenderer.SetVertexCount(lengthOfLineRenderer);
        }

        public void Update()
        {
            if (!Selected.IsEmpty() && Selected.Contains(Uid))
            {
                _lineRenderer.SetVertexCount(3);
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, Registry.Islands[Selected.Last()].transform.position);
                _lineRenderer.SetPosition(2, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
            }
            else
            {
                _lineRenderer.SetVertexCount(0);
            }
        }
    }
}