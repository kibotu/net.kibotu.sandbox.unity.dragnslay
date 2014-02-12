using UnityEngine;
using System.Collections;

public class IdleIsland : MonoBehaviour {

	private float time;
	public Vector2 xRot;
	public Vector2 yRot;
	public Vector2 zRot;
	public Vector3 rotSpeed;
	public Vector2 minRotation;
	public Vector2 minRotSpeed;
	public Vector3 rotation;
	private Quaternion originalRotation;

	public float xTrans;
	public float yTrans;
	public float zTrans;
	public float transSpeed;

	void Start () {
		originalRotation = transform.rotation;
		xRot = new Vector2 (-5f,5f);
		yRot = new Vector2 (-5f,5f);
		zRot = new Vector2 (-5f,5f);
		minRotSpeed = new Vector2 (-0.8f,0.8f);
		minRotation = new Vector2 (-5,5);
		float exclude = 0.3f;
		rotation.x = MathUtilityHelper.Range(xRot.x,xRot.y, minRotation.x, minRotation.y);	
		rotation.y = MathUtilityHelper.Range(yRot.x,yRot.y, minRotation.x, minRotation.y);
		rotation.z = MathUtilityHelper.Range(zRot.x,zRot.y, minRotation.x, minRotation.y);
		rotSpeed = new Vector3 (
			MathUtilityHelper.Range(minRotSpeed.x,minRotSpeed.y,-exclude,exclude),
			MathUtilityHelper.Range(minRotSpeed.x,minRotSpeed.y,-exclude,exclude),
			MathUtilityHelper.Range(minRotSpeed.x,minRotSpeed.y,-exclude,exclude)
		);
	}
	
	void Update () {

		time += Time.deltaTime;

		transform.localRotation = originalRotation * Quaternion.Euler(
			rotation.x * Mathf.Sin(Time.time * rotSpeed.x), 
			rotation.y * Mathf.Sin(Time.time * rotSpeed.y),
			rotation.z * Mathf.Sin(Time.time * rotSpeed.z)
		);
	}

	void OnDestroy() {
		transform.rotation = originalRotation;
	}
}
