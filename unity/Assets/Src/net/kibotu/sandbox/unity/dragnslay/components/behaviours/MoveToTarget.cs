using Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class MoveToTarget : MonoBehaviour
{

/*
	void Start ()
	{
	    localPostion = transform.localPosition;
	    transform.parent = null;

	}
	
	void Update ()
	{
	    time += Time.deltaTime;
	    transform.position = Vector3.Lerp(transform.position, target.transform.position + localPostion, time);

	    if (time > 1)
	    {
	        gameObject.AddComponent<RotationTest>();
	        transform.parent = target.transform;
	        Destroy(this);
	    }
	}*/

    public GameObject target;
    public float velocity = 5f;
    public float rotationVelocity = 1f;
    private float startTime;
    private float journeyLength;
    public float smooth = 5.0F;
    public float time;

    private Vector3 localPostion;

    void Start()
    {
        localPostion = transform.localPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, target.transform.position);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + localPostion, Time.deltaTime * velocity);

        Vector3 targetDir = transform.position.Direction(target.transform.position + localPostion);
        var step = rotationVelocity * Time.deltaTime;
        var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);

        if (Vector3.Distance(transform.position, target.transform.position + localPostion) < 0.1f)
        {
            transform.parent = target.transform;
            var rotation = gameObject.AddComponent<RotationTest>();
            rotation.enabled = true;
            Destroy(this);
        }
    }

    /*
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, target.transform.position, fracJourney);

        Debug.Log(Vector3.Distance(transform.position, target.transform.position + localPostion));

        if (fracJourney > 1f)
        {
            gameObject.AddComponent<RotationTest>();
            transform.parent = target.transform;
            Destroy(this);
        }
    }*/
}
