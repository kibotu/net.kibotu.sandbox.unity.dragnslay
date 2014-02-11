using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.LookAt(target.transform);
	
	}
}
