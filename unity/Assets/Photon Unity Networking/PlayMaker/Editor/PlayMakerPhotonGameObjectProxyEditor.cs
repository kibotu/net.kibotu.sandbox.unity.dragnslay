using UnityEngine;
using UnityEditor;
using System.Collections;

#if PHOTON
[CustomEditor(typeof(PlayMakerPhotonGameObjectProxy))]
public class PlayMakerPhotonGameObjectProxyEditor : Editor {


	public override void  OnInspectorGUI()
	{
		
		PlayMakerPhotonGameObjectProxy _target = target as PlayMakerPhotonGameObjectProxy;
		
		if (GUILayout.Button("Help"))
		{
			_target.help();
		}
	}
}
#endif