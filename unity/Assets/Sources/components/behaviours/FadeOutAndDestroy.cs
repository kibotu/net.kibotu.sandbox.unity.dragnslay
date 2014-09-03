using UnityEngine;
using System.Collections;

public class FadeOutAndDestroy : MonoBehaviour {

	public float StartTime = 1f;
	public float Duration = 1f;
	public Color StartColor;

	public void Start() {
		Reset();
	}

	public void Update () 
	{
		StartTime += Time.deltaTime;
		Fade();

		if(StartTime > Duration)
			Destroy(this.gameObject);
	}

	public Color c;

	private void Fade() {
		if(renderer == null || renderer.material == null)
			return;

		c = StartColor;
		c.a = Mathf.Lerp(c.a, 0, Easing.Sinusoidal.easeOut(StartTime / Duration));
		renderer.material.SetColor ("_TintColor", c);
	}

	public void Reset() 
	{
		StartTime = 0;
		StartColor = Color.white;
	}
}
