using UnityEngine;
using System.Collections;

public class IdleCamera : MonoBehaviour
{

    public Vector3 origin;
    public Vector3 target;
    public float ratio = 0.01f;

    void Start()
    {
        origin = transform.position;
        InvokeRepeating("ChangeTarget", 0.01f, 2f);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, ratio);
    }

    void ChangeTarget()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);
        target = new Vector3(origin.x + x, origin.y, origin.z+z);
    }
}