using System;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.components.behaviours.depricated
{
    [Obsolete("Not used anymore", false)]
    public class init : MonoBehaviour
    {
        public GameObject Source;
        public GameObject Target;

        public void OnGUI()
        {
            if (GUILayout.Button( "Move already, biatch!"))
            {

                for (var i = 0; i < Source.transform.childCount; ++i)
                {
                    var papership = Source.transform.GetChild(i);
                    if (papership.name == "Papership")
                    {
                        StartCoroutine(MovePlane(papership, Target));
                    }
                }

                var tmp = Source;
                Source = Target;
                Target = tmp;
            }
        }

        private static IEnumerator MovePlane(Component plane, GameObject target)
        {
            if (plane == null) throw new ArgumentNullException("plane");

            var rotation = plane.GetComponent<Orbiting>();
            Destroy(rotation);
            var move = plane.gameObject.AddComponent<MoveToTarget>();
            move.Target = target;
            yield return 0;
        }
    }
}
