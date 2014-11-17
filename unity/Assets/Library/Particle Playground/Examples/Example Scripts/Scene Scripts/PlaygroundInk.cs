using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundInk : MonoBehaviour {

	public Transform controlTransform;
	PlaygroundParticlesC particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<PlaygroundParticlesC>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f)) {
				particles.Emit (
					Mathf.RoundToInt (4000*Time.deltaTime), 
					hit.point-new Vector3(.2f,.2f,.2f), 
					hit.point+new Vector3(.2f,.2f,.2f), 
					new Vector3(-1f,-1f,-1f), 
					new Vector3(1f,1f,1f), 
					Color.white
				);
				if (controlTransform!=null)
					controlTransform.position = hit.point;
			}
		}
	}
}