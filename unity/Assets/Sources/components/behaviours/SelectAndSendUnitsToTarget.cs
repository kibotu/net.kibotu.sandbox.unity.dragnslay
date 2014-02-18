using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SelectAndSendUnitsToTarget : MonoBehaviour {

        public static readonly List<int> Selected = new List<int>();
        private int _uid = -1;

        public int Uid
        {
            get { return _uid == -1 ? _uid = GetComponent<IslandData>().uid : _uid; }   
        }

        public void Select()
        {
            if (!Selected.Contains(Uid)) Selected.Add(Uid);
        }

        public void Deselect()
        {
            Selected.Remove(Uid);
        }

        public void SendUnits()
        {
            var target = Registry.Islands[Selected[Selected.Count - 1]];

            // 1) already there
            if (target == gameObject) return;

            // 3) send own units to destination
            for (var i = 0; i < transform.childCount; ++i)
            {
                var papership = transform.GetChild(i);
                if (papership.name.StartsWith("Papership"))
                {
                    StartCoroutine(MovePlane(papership, target));
                }
            }

           // GameObject.Find("GConsole").guiText.text = "Send (" + Selected.Count + ") " + Uid + " to " + target.GetComponent<IslandData>().uid;
            // Debug.Log("Send ("+Selected.Count+")" + Uid + " to " + target.GetComponent<IslandData>().uid);
        }

        private static IEnumerator MovePlane(Component plane, GameObject target)
        {
            Debug.Log("sending plane");
            if (plane == null) throw new ArgumentNullException("plane");
            var move = plane.gameObject.AddComponent<MoveToTarget>();
            move.target = target;
            yield return 0;
        }
    }
}