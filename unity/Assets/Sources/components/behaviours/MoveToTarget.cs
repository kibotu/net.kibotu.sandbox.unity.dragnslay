using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public GameObject target;
    public float velocity = 7f;
    public float rotationVelocity = 60f;
    private float startTime;
    private float journeyLength;

    private Orbiting orbiting;
    private Vector3 finalDestination;

    public void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, target.transform.position);

        orbiting = gameObject.AddComponent<Orbiting>();
        orbiting.center = target.transform;
        finalDestination = orbiting.GetFinalDestination();
        orbiting.enabled = false;
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalDestination, Time.deltaTime * velocity);

        Vector3 targetDir = transform.position.Direction(finalDestination);
        var step = rotationVelocity * Time.deltaTime;
        var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        if (Vector3.Distance(transform.position, finalDestination) < 0.01f)
        {
            transform.parent = target.transform;
            orbiting.enabled = true;
            Destroy(this);
        }
    }
}
