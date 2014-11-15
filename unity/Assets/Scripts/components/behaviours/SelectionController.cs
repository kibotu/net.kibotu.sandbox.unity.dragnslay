using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SelectionController : MonoBehaviour {

        public readonly List<GameObject> Selected = new List<GameObject>();
        public GameObject AnimatedTiledLine;
        private readonly List<GameObject> _lines = new List<GameObject>();

        public void ToggleAddToList(GameObject selected)
        {
            if (!Selected.Contains(selected))
            {
                Selected.Add(selected);
                selected.renderer.material.color = Color.red;
            }
            else
            {
                Selected.Remove(selected);
                selected.renderer.material.color = Color.white;
            }

            TargetLast();
        }

        private void TargetLast()
        {
            if (Selected.Count > 1)
            {
                ClearLines();

                for (int i = 1; i < Selected.Count; ++i)
                    AddLine(Selected[i - 1].transform, Selected.Last().transform);
            }
        }

        private void AddLine(Transform start, Transform end)
        {
            var go = Instantiate(AnimatedTiledLine) as GameObject;
            go.transform.parent = transform;
            var lr = go.GetComponent<LineRendererWithAnimateTexture>();
            lr.Start = start;
            lr.End = end;
            _lines.Add(go);
        }

        public void FinalTarget(GameObject lastSelected)
        {
            foreach (var go in Selected)
            {
                Debug.Log(go.name + " => " + lastSelected.name);
            }
        }

        public void DeselectAll()
        {
            foreach (var go in Selected)
            {
                go.renderer.material.color = Color.white;
            }
            Selected.Clear();

            ClearLines();
        }

        private void ClearLines()
        {
            foreach (var line in _lines)
            {
                Destroy(line);
            }
            _lines.Clear();
        }
    }
}
