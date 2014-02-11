using UnityEngine;

public class init : MonoBehaviour
{
    public GameObject source;
    public GameObject target;

    void OnGUI()
    {
        if (GUILayout.Button("Move already, biatch!"))
        {
            var papership = source.transform.FindChild("Papership");
            var rotation = papership.GetComponent<RotationTest>();
            var move = papership.gameObject.AddComponent<MoveToTarget>();
            move.target = target;
            Destroy(rotation);
        }
    }
}
