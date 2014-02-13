using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using UnityEngine;

public class init : MonoBehaviour
{
    public GameObject source;
    public GameObject target;

    public void OnGUI()
    {
        if (GUILayout.Button( "Move already, biatch!"))
        {

            for (int i = 0; i < source.transform.childCount; ++i)
            {
                var papership = source.transform.GetChild(i);
                Debug.Log(papership.name);
                if (papership.name == "Papership")
                {
                    var rotation = papership.GetComponent<Orbiting>();
                    var move = papership.gameObject.AddComponent<MoveToTarget>();
                    move.target = target;
                    Destroy(rotation);

                    var tmp = source;
                    source = target;
                    target = tmp;
                }
            }
        }
    }
}
