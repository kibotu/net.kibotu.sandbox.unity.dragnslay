// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections.Generic;


/// <summary>
/// Implements a convenient menu to add the necessary prefab to bind Photon and Playmaker together.
/// WARNING: without this prefab, nothing will work.
/// Only one of these prefab is needed.
/// 
/// TODO: maybe we could implement a singleton system and check for the uniqueness and provide a more robust and secure implementation.
/// </summary>
public class PlayMakerPhotonEditorUtility : Editor
{
    const string PlayMakerPhotonMenuRoot = "PlayMaker/Addons/Photon Networking/";
	
    public const float supportedPUNVersion = 1.21f;

	static string PlayMakerPhotonProxyName = "PlayMaker Photon Proxy";
	
	public static Color lightBlue = new Color(0.3f,0.7f,1f);
	public static Color lightOrange = new Color(1f,0.7f,0f);
	
	
	
	/*
	// simply test to launch the wizard remotly.
	[MenuItem("PlayMaker/Photon/launch")]
  	public static void DoLaunchWizard()
    {
		PlayMakerPhotonWizard.Init();
	}
	*/
	
	/// <summary>
	/// Menu item function that checks and adds if necessary the "PlayMaker Photon Proxy" prefab.
	/// </summary>
	[MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon proxy to scene")]
  	public static void DoCreateRpcProxy()
    {
		if ( IsSceneSetup() )
		{
				UnityEngine.Debug.LogWarning("Only one PlayMaker Photon Proxy GameObject with a PlayMakerPhotonProxy component attached is needed per scene");	
		}else{
			
			// get the prefab and instanciate it	
			PrefabUtility.InstantiatePrefab(Resources.Load(PlayMakerPhotonProxyName, typeof(GameObject)));
		}
	}//DoCreateRpcProxy
	
	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon proxy to scene", true)]
    static bool ValidateAddPhotonProxyToScene () {
        return !IsSceneSetup();
    }
		
	
	
	/// <summary>
	/// Menu item function that adds a photon view AND if necessary the "PlayMaker Photon Proxy" prefab.
	/// </summary>
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon View to selected Objects")]
    public static void DoAddPhotonView()
    {
	
		
		if (Selection.activeGameObject != null)
		{	
			
			if (!IsSceneSetup())
			{
				DoCreateRpcProxy();
			}
			
			Selection.activeGameObject.AddComponent<PhotonView>();
			
			GameObject _go = Selection.activeGameObject;
			if (_go.GetComponent<PlayMakerPhotonGameObjectProxy>() ==null)
			{
				_go.AddComponent<PlayMakerPhotonGameObjectProxy>();
			}
		}
	}// DoCreateGameObjectProxy
	
	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon View to selected Objects", true)]
    static bool ValidateAddPhotonViewToSelectedObjects () {
        return Selection.activeGameObject != null;
    }
	
	/// <summary>
	/// Menu item function that adds if necessary the "PlayMaker Photon Proxy" prefab.
	/// </summary>
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon GameObject proxy to selected Object")]
    public static void DoCreateGameObjectProxy()
    {
	
		if (Selection.activeGameObject != null)
		{	
			if (!IsSceneSetup())
			{
				DoCreateRpcProxy();
			}
			
			GameObject _go = Selection.activeGameObject;
			if (_go.GetComponent<PlayMakerPhotonGameObjectProxy>() ==null)
			{
				_go.AddComponent<PlayMakerPhotonGameObjectProxy>();
			}
		}
	}// DoCreateGameObjectProxy
	
	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon GameObject proxy to selected Object", true)]
    static bool ValidateAddPhotonGameObjectProxyToSelectedObject () {
       return Selection.activeGameObject != null;
    }
	
	
	/// <summary>
	/// Menu item function that create a new fsm with the required photon view.
	/// </summary>
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon-ready Fsm to selected Object", false)]	
	public static void DoCreateFsmWithPhotonView()
	{
		if (Selection.activeGameObject != null)
		{
			DoCreateGameObjectProxy();
			
			// create a new fsm component
			GameObject _go = Selection.activeGameObject;
			PlayMakerFSM fsm = _go.AddComponent<PlayMakerFSM>();
			fsm.FsmName  = "Photon Network Synch FSM";
			
			fsm.FsmDescription = "This FSM is ready to accept variables that are synched across the network.";

			PhotonView photonView = _go.AddComponent<PhotonView>();
			
			photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
			
			photonView.observed = fsm;
			
			if (_go.GetComponent<PlayMakerPhotonGameObjectProxy>() ==null)
			{
				_go.AddComponent<PlayMakerPhotonGameObjectProxy>();
			}
			
		}
	}
	
	
	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
    [MenuItem(PlayMakerPhotonMenuRoot + "Components/Add Photon-ready Fsm to selected Object", true)]
    static bool ValidateAddPhotonPhotonReadyFsmToSelectedObject () {
       return Selection.activeGameObject != null;
    }
	
	
	public static bool IsSceneSetup()
	{
				
		GameObject go = GameObject.Find(PlayMakerPhotonProxyName);
		
		return go !=null;
	}
	
	
	public static bool IsPunVersionSupported()
	{
		string _versionPUN = PhotonNetwork.versionPUN;
		
		float versionNumber = 0f;
		if (float.TryParse(_versionPUN,out versionNumber))
		{
			if(versionNumber<=supportedPUNVersion)
			{
				return true;
			}
		}
		
		return false;
	}
	

}