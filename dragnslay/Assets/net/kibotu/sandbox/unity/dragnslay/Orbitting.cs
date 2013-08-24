using UnityEngine;
using System.Collections;

public class Orbitting : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 100, Space.World);
	}
}
