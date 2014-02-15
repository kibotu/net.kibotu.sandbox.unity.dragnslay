// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

/// <summary>
/// This component is required on gameObjects and prefabs you are planning to instanciate over the Photon network.
/// 
/// It receives the OnPhotonInstantiate message when instanciated and forward it as an Event for Fsm component attached this gameObject and all its childrens.
/// 
/// It also verifies the proper setup for fsm components on that gameObject that have networked synched variables:
/// *It assumes like for the Unity networking that you have a PhotonView observing the fsm.
/// *I insert at runtime a bridge ( PlayMakerPhotonView ) that goes inbetween the fsm and the photonView. This is required because the fsmComponent doesn't implement photon networking nativly
/// ( that is not implementing OnPhotonInstantiate() nor OnPhotonSerializeView
/// It can be set up manually of course, else Iwill do it for the user at runtime when the gameObject is instanciated. 
/// Note: for gameObject sitting in the hierarchy when starting, the check is also happening but it's directly call within the "PlayMaker photon proxy" mandatory prefab
/// </summary>
public class PlayMakerPhotonGameObjectProxy : Photon.MonoBehaviour {
	
	/// <summary>
	/// output in the console activities of the various elements.
	/// Is taken form the debug value of the "PlayMaker Photon Proxy"
	/// </summary>
	bool debug = true;
	bool LogMessageInfo = true;
	/// <summary>
	/// The fsm proxy used to send the "OnPhotonInstantiate" event to Fsm.
	/// </summary>
	PlayMakerFSM fsmProxy;
		
		
	[ContextMenu("Help")]
	public void help ()
	{
	    Application.OpenURL ("https://hutonggames.fogbugz.com/default.asp?W990");
	}
	
	
	// get the Playmaker Photon proxy fsm.
	void Awake () {
	

		Debug.Log("Player awake");
			
		// get the photon proxy for Photon Fsm Proxy to send event.
		GameObject go = GameObject.Find("PlayMaker Photon Proxy");
		
		if (go == null )
		{
			Debug.LogError("Working with photon network require that you add a 'PlayMaker Photon Proxy' component to the gameObject. You can do so from the menu 'PlayMaker Photon/components/Add photon proxy to scene'");
			return;
		}
		
		// get the proxy to set the debug flag.
	 	PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
		if (_proxy!=null)
		{
			debug = _proxy.debug;
			LogMessageInfo = _proxy.LogMessageInfo;
		}
		
		// get the Fsm for reference when sending events.
		fsmProxy = go.GetComponent<PlayMakerFSM>();
		if (fsmProxy==null)
		{
			return;
		}
		
		_proxy.SanitizeGameObject(this.gameObject);
		
	}// Awake
	
	#region Photon Messages
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// TOFIX: The problem is, It's called before Fsm are instanciated, so I had to implement a slight delay...
	/// </summary>
	void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonInstantiate "+info.sender.name);
		}
		
	
		if (fsmProxy==null)
		{
			Debug.LogWarning("FsmProxy is null");
			return;
		}
	
		// TOFIX: if we found a better solution, I am all up for it... How stable this can be if framerate is low for example?
		Invoke("sendPhotonInstantiationFsmEvent",0.1f);

		
	}// OnPhotonInstantiate
	
	
	/// <summary>
	/// Sends the photon instantiation fsm event to ALL Fsm of the instantiated gameObject AND its children.
	/// </summary>
	void sendPhotonInstantiationFsmEvent()
	{
		if (debug) {
			Debug.Log("sending PHOTON INSTANTIATE event to "+this.gameObject.name);
		}
		// set the target to be this gameObject.
		FsmOwnerDefault goTarget = new FsmOwnerDefault();
		goTarget.GameObject = new FsmGameObject();
		goTarget.GameObject.Value = this.gameObject;
		goTarget.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		
       // send the event to this gameObject and all its children
		FsmEventTarget eventTarget = new FsmEventTarget();
		eventTarget.excludeSelf = false;
		eventTarget.target = FsmEventTarget.EventTarget.GameObject;
		eventTarget.gameObject = goTarget;
		eventTarget.sendToChildren = true;
		
		// create the event.
		FsmEvent fsmEvent = new FsmEvent("PHOTON / PHOTON INSTANTIATE");
	
		// send the event
		fsmProxy.Fsm.Event(eventTarget,fsmEvent.Name); // TOFIX: doesn't work if using simply fsmEvent
		
	}// sendPhotonInstantiationFsmEvent
	
	#endregion
	
	
	
	
	#region Photon RPC TARGETS
		
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcastFsmEvent" that use RPC to send information about the event to Send to self over the network
	/// </summary>
	/// <param name='target'>
	/// Photon Target.
	/// </param>
	/// <param name='globalEventName'>
	/// Global Fsm event name to send using the photon target rule.
	/// </param>
	public void PhotonRpcSendFsmEvent(PhotonTargets target,string globalEventName)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send global Fsm Event:"+globalEventName+" to self, with network target:"+target.ToString());	
		}
		
		photonView.RPC("rpc", target, globalEventName);// method name used to be too long : "RPC_PhotonRpcBroadcastFsmEvent"
	}
	
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcastFsmEvent" that use RPC to send information about the event to Send to self over the network
	/// </summary>
	/// <param name='target'>
	/// Photon Target.
	/// </param>
	/// <param name='globalEventName'>
	/// Global Fsm event name to broadcast using the photon target rule.
	/// </param>	
	/// <param name='stringData'>
	/// String data to pass with this event. WARNING: this is not supposed to be (nor efficient) a way to synchronize data. This is simply to comply with
	/// the ability for FsmEvent to include data.
	/// </param>
	public void PhotonRpcSendFsmEventWithString(PhotonTargets target,string globalEventName,string stringData)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send string:"+stringData+"  with global Fsm Event:"+globalEventName+" to self, with network target:"+target.ToString());	
		}
		
		photonView.RPC("rpc_s", target, globalEventName, stringData);// method name used to be too long :  "RPC_FsmPhotonRpcBroadcastFsmEventWithString"
	}
	#endregion
	
	#region Photon RPC TARGETS FUNCTIONS
	//-- TODO: more rpc Events signatures or a more verstatile signature perhaps? can't find a way tho...
	// at least a set of signature to provide support for all eventdata, but that means *A LOT* of signatures for all possible cases 
	// ( string,  string int, string float,  string int float, string int float vector3, etc...) overwhelming.
	//--
	
	/// <summary>
	/// RPC CALL. The paired rpc called triggered by PhotonRpcBroacastFsmEvent ( either by player or target)
	/// this will broadcast to All Fsm a global Fsm Event.
	/// The sender properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='globalEventName'>
	/// Global Fsm event name.
	/// </param>
	/// <param name='info'>
	/// Info.
	/// </param>
	[RPC]
	void rpc(string globalEventName,PhotonMessageInfo info) // method name used to be too long :  RPC_PhotonRpcBroadcastFsmEvent
	{
		if (LogMessageInfo)
		{
			Debug.Log(info.sender);	
		}
		//lastMessagePhotonPlayer = info.sender;
		
		
		// set the target to be this gameObject.
		FsmOwnerDefault goTarget = new FsmOwnerDefault();
		goTarget.GameObject = new FsmGameObject();
		goTarget.GameObject.Value = this.gameObject;
		goTarget.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		
       // send the event to this gameObject and all its children
		FsmEventTarget eventTarget = new FsmEventTarget();
		eventTarget.excludeSelf = false;
		eventTarget.target = FsmEventTarget.EventTarget.GameObject;
		eventTarget.gameObject = goTarget;
		eventTarget.sendToChildren = true;
		
		// create the event.
		FsmEvent fsmEvent = new FsmEvent(globalEventName);
	
		// send the event
		fsmProxy.Fsm.Event(eventTarget,fsmEvent.Name); // TOFIX: doesn't work if using simply fsmEvent
	}
	
	/// <summary>
	/// RPC CALL. The paired rpc called triggered by PhotonRpcBroacastFsmEventWithString ( either by player or target)
	/// this will broadcast to All Fsm a global Fsm Event.
	/// The sender properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='globalEventName'>
	/// Global Fsm event name.
	/// </param>
	/// <param name='info'>
	/// Info.
	/// </param>
	[RPC]
	void rpc_s(string globalEventName,string stringData,PhotonMessageInfo info)// method name used to be too long : RPC_FsmPhotonRpcBroadcastFsmEventWithString
	{
		if (LogMessageInfo)
		{
			Debug.Log(info.sender.name+" sent RPC string:"+stringData+" from Fsm Event:"+globalEventName);	
		}
		
		//lastMessagePhotonPlayer = info.sender;
		
		Fsm.EventData.StringData = stringData;

		// set the target to be this gameObject.
		FsmOwnerDefault goTarget = new FsmOwnerDefault();
		goTarget.GameObject = new FsmGameObject();
		goTarget.GameObject.Value = this.gameObject;
		goTarget.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		
       // send the event to this gameObject and all its children
		FsmEventTarget eventTarget = new FsmEventTarget();
		eventTarget.excludeSelf = false;
		eventTarget.target = FsmEventTarget.EventTarget.GameObject;
		eventTarget.gameObject = goTarget;
		eventTarget.sendToChildren = true;
		
		// create the event.
		FsmEvent fsmEvent = new FsmEvent(globalEventName);
	
		// send the event
		fsmProxy.Fsm.Event(eventTarget,fsmEvent.Name); // TOFIX: doesn't work if using simply fsmEvent
	}
	
	#endregion
	
	
}
