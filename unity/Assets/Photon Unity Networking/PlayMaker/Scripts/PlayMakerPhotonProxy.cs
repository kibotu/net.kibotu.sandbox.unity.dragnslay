// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

/// <summary>
/// Part of "PlayMaker Photon Proxy" prefab.
/// This behavior implements *All* messages from Photon, and broadcast associated global events.
/// note: the instantiate call is featured in the PlayMakerPhotonGameObjectProxy component
/// 
/// The playmaker events corresponding to each Photon messages are declared in the Fsm named "Photon messages interface" in the "PlayMaker Photon Proxy" prefab.
/// 
/// Example: the photon message OnPhotonPlayerConnected (PhotonPlayer player) is translated as a global event "PHOTON / PHOTON PLAYER CONNECTED"
/// the PhotonPlayer passed in these messages is stored in lastMessagePhotonPlayer and can be retrieved using the action "PhotonViewGetLastMessagePLayerProperties"
/// 
/// This behavior also watch the connection state and broadcast associated global events.
/// example: PhotonNetwork.connectionState.Connecting is translated as a global event named "PHOTON / STATE : CONNECTING"
/// 
/// 
///
/// This behavior also watch for FsmVariables being synched over the network ( FsmVariables with the check box "network sync" on), and verify that
/// the correct setup is implemented for the network synchronization to work, that is:
/// First  : a PlaymakerPhotonView needs to observe a fsm component with at least onefsm variable with network sync on. 
/// Second : this PlaymakerPhotonView is in turn observed by a PhotonView.
/// 
/// 
/// TODO: To implement The "network sync" check box properly:
/// 
///  -- FSMString not supported, and other types --> Photon supports any objects. 
///  -- Fsm Global vars not supported.
///  
///  -- playmaker api enhancment?: 
///  -- flag for a fsm if it contains at least one fsmVariable to synch over netork, or better the list of variable that synch.
///  -- some kind of common class to work with fsmvars instead of duplicate 12 times code for each type of fsmvariable ( or a c# trick)
/// 
/// </summary>
public class PlayMakerPhotonProxy : Photon.MonoBehaviour
{

	/// <summary>
	/// output in the console activities of the various elements.
	/// TODO: should be set to false for release
	/// </summary>
	public bool debug = true;
	
	/// <summary>
	/// The last state of the connection. This is used to watch connection state changes and broadcast related Events.
	/// So you can receive an event when the connection is "disconnecting" or "Connecting", something not available as messages.
	/// TOREVIEW: this is a goodie, but very useful within playmaker environment, it's easier and more adequate then watching for the connection state within a fsm.
	/// </summary>
	private ConnectionState lastConnectionState;	
	
	/// <summary>
	/// The photon player sent with a message like OnPhotonPlayerConnected, OnPhotonPlayerDisconnected or OnMasterClientSwitched
	/// Only the last instance is stored. Use PhotonNetworkGetMessagePlayerProperties Action to retrieve it within PlayMaker.
	/// This also store the player from the photonMessageinfo of the RPC calls implemented in this script.
	/// </summary>
	public PhotonPlayer lastMessagePhotonPlayer;
	
	/// <summary>
	/// The last disonnection or connection failure cause
	/// </summary>
	public DisconnectCause lastDisconnectCause;
	
	
	
	#region Photon network synch
	

	/// <summary>
	/// Used to check for fsm that required a photon view, and make sure it is set up properly: fsm<->playmakerPhotonView<->PhotonView
	/// </summary>
	void Start()
	{
		ArrayList FsmToObserveList = GetFsmsWithNetworkSynchedVariables();
		
		//now for each of these Fsm check that a playmaker Photon gameObject proxy is attached to it, else will complain
		foreach(PlayMakerFSM fsm in FsmToObserveList)
		{
			PlayMakerPhotonGameObjectProxy goProxy = fsm.gameObject.GetComponent<PlayMakerPhotonGameObjectProxy>();
			
			if (goProxy==null)
			{
				Debug.LogError("Missing PlayMakerPhotonGameObjectProxy on GameObject '"+fsm.gameObject.name + "' with Fsm '"+fsm.FsmName+"' containing variables supposed to be synched over the network");
			}
			
		}
		
	}// start
	
	
	/// <summary>
	/// pre flight check on game object. Making sure it's set up properly to connect playmaker and PUN together.
	/// </summary>
	/// <returns>
	/// The pre flight check on game object.
	/// </returns>
	/// <param name='go'>
	/// true if pre flight check went ok: else something went wrong...
	/// </param>
	public bool ValidatePreFlightCheckOnGameObject(GameObject go)
	{
		if (go==null)
		{
			return false;
		}
		
		PlayMakerPhotonGameObjectProxy[] proxies = go.GetComponents<PlayMakerPhotonGameObjectProxy>();
		if (proxies.Length ==0)
		{
			Debug.LogError("Instanciating a GameObject with photon network require that you add a 'PlayMakerPhotonGameObjectProxy' component to the gameObject");
		}
		
		return true;
	}// ValidatePreFlightCheckOnGameObject
	
	/// <summary>
	/// Sanitizes the game object and check photonView observing fsm, else complain. 
	/// If photonView there, will insert PlayMakerPhotonView inbetween. 
	/// </summary>
	/// <param name='go'>
	/// Go.
	/// </param>
	public void SanitizeGameObject(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		
		PhotonView[] allPhotonViews = go.GetComponentsInChildren<PhotonView>();
		
		// now make sure all fsm with network synchronized variable have a photonView attached
		ArrayList fsmsToObserve = GetFsmsWithNetworkSynchedVariables(go);
		Debug.Log("found fsm to observe : "+fsmsToObserve.Count);
		foreach(PlayMakerFSM fsm in fsmsToObserve)
		{		
			bool ok = false;
			foreach(PhotonView photonView in allPhotonViews)
			{
				if 	(photonView.observed == fsm)
				{
					ok = true;
					break;
				}
			}
			
			if (!ok){
				Debug.LogError(
					string.Format(
						"Fsm component '{0}' on gameObject '{1}' has variable checked for network synching, but no PhotonView component is observing this fsm",
						fsm.name,
						fsm.gameObject.name)
					);
			}
			
		}
	
		// now inject PlayMakerPhotonView where required.
		foreach(PhotonView photonView in allPhotonViews)
		{	
			Debug.Log(" photon view observing : "+photonView.observed+" "+photonView.viewID);
			
			if ( photonView.observed is PlayMakerFSM)
			{
				PlayMakerFSM fsm =  (PlayMakerFSM)photonView.observed;
				PlayMakerPhotonView synchProxy = photonView.gameObject.AddComponent<PlayMakerPhotonView>();
				Debug.Log("switching observed");
				synchProxy.observed = fsm;
				
				photonView.observed = synchProxy;
			}
		}
	
	}// SanitizeGameObject

	
	/// <summary>
	/// Gets the list of fsm from a given gameObject having network fsm synched variables.
	/// </summary>
	/// <returns>
	/// The list of fsm with network fsm synched variables on that gameObject.
	/// </returns>
	private ArrayList GetFsmsWithNetworkSynchedVariables(GameObject go)
	{
		
		ArrayList FsmToObserveList = new ArrayList();
		
		PlayMakerFSM[] allFsms = go.GetComponentsInChildren<PlayMakerFSM>();
		
		
		foreach(PlayMakerFSM fsm in allFsms)
		{	
			if (! FsmToObserveList.Contains(fsm))
			{
				if (HasFsmNetworkingSynchVariables(fsm))
				{
					FsmToObserveList.Add(fsm);
				}
			}
		}
		
		return FsmToObserveList;
	}// GetFsmsWithNetworkSynchedVariables
	
	
	
	/// <summary>
	/// Gets the list of fsm having network fsm synched variables.
	/// </summary>
	/// <returns>
	/// The list of fsm with network fsm synched variables.
	/// </returns>
	private ArrayList GetFsmsWithNetworkSynchedVariables()
	{
		
		ArrayList FsmToObserveList = new ArrayList();
		
		foreach(PlayMakerFSM fsm in PlayMakerFSM.FsmList)
		{	
			if (! FsmToObserveList.Contains(fsm))
			{
				if (HasFsmNetworkingSynchVariables(fsm))
				{
					FsmToObserveList.Add(fsm);
				}
			}
		}
		
		return FsmToObserveList;
	}// GetFsmsWithNetworkSynchedVariables
	
	
	/// <summary>
	/// Determines whether a given fsm has network synch variables.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this fsm features network synch variables; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='fsm'>
	/// If set to <c>true</c> fsm.
	/// </param>
	private bool HasFsmNetworkingSynchVariables(PlayMakerFSM fsm)
	{
		foreach(FsmFloat fsmFloat in  fsm.FsmVariables.FloatVariables)
		{
			if (fsmFloat.NetworkSync){
					return true;
			}
		}
		
		foreach(FsmInt fsmInt in  fsm.FsmVariables.IntVariables)
		{
			if (fsmInt.NetworkSync){
					return true;
			}
		}
		
		foreach(FsmVector2 fsmVector2 in  fsm.FsmVariables.Vector2Variables)
		{
			if (fsmVector2.NetworkSync){
					return true;
					
			}
		}
		
		foreach(FsmVector3 fsmVector3 in  fsm.FsmVariables.Vector3Variables)
		{
			if (fsmVector3.NetworkSync){
					return true;
					
			}
		}
		
		foreach(FsmQuaternion fsmQuaternion in  fsm.FsmVariables.QuaternionVariables)
		{
			if (fsmQuaternion.NetworkSync){
					return true;
					
			}
		}
		
		foreach(FsmColor fsmColor in  fsm.FsmVariables.ColorVariables)
		{
			if (fsmColor.NetworkSync){
					return true;
					
			}
		}

		foreach(FsmBool fsmBool in  fsm.FsmVariables.BoolVariables)
		{
			if (fsmBool.NetworkSync){
					return true;
			}
		}
		
		foreach(FsmString fsmString in  fsm.FsmVariables.StringVariables)
		{
			if (fsmString.NetworkSync){
					return true;
			}
		}
			
			
			// TODO: duplicate for all Fsm variable types... yeekkk... could we not have a FsmVariableRoot extending the commong stuff or a c# trick?
		return false;
	}// HasFsmNetworkingSynchVariables
	
	
	
	/// <summary>
	/// Determines whether a PhotonView is observing the specified fsm.
	/// </summary>
	/// <returns>
	/// <c>true</c> if a PhotonView is observing the specified fsm; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='fsm'>
	/// the fsm component
	/// </param>
	/// <param name='thePhotonView'>
	/// the photonView component IF it is observing fsm.
	/// </param>
	private PhotonView GetPhotonViewObservingFsm(PlayMakerFSM fsm)
	{
		Debug.Log("GetPhotonViewObservingFsm "+fsm.FsmName  );
		
	 	PhotonView[] photonViews = fsm.GetComponents<PhotonView>();
		foreach(PhotonView photonview in  photonViews)
		{
			
			//PlayMakerFSM observedFsm = (PlayMakerFSM)photonview.observed;
			//if (observedFsm!=null)
			//{
			//	Debug.Log(photonview.observed.GetType().ToString());
			//}
			if (photonview.observed == fsm){
				return photonView;
			}
		}
		return null;
	}// GetPhotonViewObservingFsm

	
	
	
	
	#endregion
	
	
	#region Photon RPC PLAYER
	/// <summary>
	/// output in the console photon message activity.
	/// TODO: should be set to false for release
	/// </summary>
	public bool LogMessageInfo = true;
	
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
	/// </summary>
	/// <param name='target'>
	/// Photon player Target.
	/// </param>
	/// <param name='globalEventName'>
	/// Global Fsm event name to broadcast to the player target
	/// </param>
	public void PhotonRpcBroadcastFsmEvent(PhotonPlayer target,string globalEventName)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send global Fsm Event:"+globalEventName+" to player:"+target.ToString());	
		}
		
		photonView.RPC("rpc", target, globalEventName);
	}
	
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcasFsmEventToPlayer" that use RPC to send information about the event to broadcast
	/// </summary>
	/// <param name='target'>
	/// Photon player Target.
	/// </param>
	/// <param name='globalEventName'>
	/// Global Fsm event name to broadcast to the player target
	/// </param>
	/// <param name='stringData'>
	/// String data to pass with this event. WARNING: this is not supposed to be (nor efficient) a way to synchronize data. This is simply to comply with
	/// the ability for FsmEvent to include data.
	/// </param>
	public void PhotonRpcFsmBroadcastEventWithString(PhotonPlayer target,string globalEventName,string stringData)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send string:"+stringData+" with global Fsm Event:"+globalEventName+" to player:"+target.ToString());	
		}
		
		photonView.RPC("rpc_s", target, globalEventName, stringData);
	}

	#endregion
	
	#region Photon RPC TARGETS
		
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
	/// </summary>
	/// <param name='target'>
	/// Photon Target.
	/// </param>
	/// <param name='globalEventName'>
	/// Global Fsm event name to broadcast using the photon target rule.
	/// </param>
	public void PhotonRpcBroacastFsmEvent(PhotonTargets target,string globalEventName)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send global Fsm Event:"+globalEventName+" to target:"+target.ToString());	
		}
		
		photonView.RPC("rpc", target, globalEventName);// method name used to be too long : "RPC_PhotonRpcBroadcastFsmEvent"
	}
	
	/// <summary>
	/// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
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
	public void PhotonRpcBroacastFsmEventWithString(PhotonTargets target,string globalEventName,string stringData)
	{
		if (LogMessageInfo)
		{
			Debug.Log("RPC to send string:"+stringData+"  with global Fsm Event:"+globalEventName+" to target:"+target.ToString());	
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
		lastMessagePhotonPlayer = info.sender;
		
		PlayMakerFSM.BroadcastEvent(globalEventName);
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
		
		lastMessagePhotonPlayer = info.sender;
		
		Fsm.EventData.StringData = stringData;

		PlayMakerFSM.BroadcastEvent(globalEventName);
	}

	#endregion
	
	/// <summary>
	/// Watch connection state
	/// </summary>
	void Update ()
	{		
		Update_connectionStateWatcher ();
	}
	
	#region connection state watcher
	
	/// <summary>
	/// Watch connection state and broadcast associated FsmEvent.
	/// </summary>
	private void Update_connectionStateWatcher ()
	{

		if (lastConnectionState != PhotonNetwork.connectionState) {
			if (debug) {
				Debug.Log ("PhotonNetwork.connectionState changed from '" + lastConnectionState + "' to '" + PhotonNetwork.connectionState + "'");
			}
			
			lastConnectionState = PhotonNetwork.connectionState;
			
			switch (PhotonNetwork.connectionState) {
				case ConnectionState.Connected:
					
					PlayMakerFSM.BroadcastEvent ("PHOTON / STATE : CONNECTED");	
					break;

				case ConnectionState.Connecting:

					PlayMakerFSM.BroadcastEvent ("PHOTON / STATE : CONNECTING");
					break;
				
				case ConnectionState.Disconnected:
						
					PlayMakerFSM.BroadcastEvent ("PHOTON / STATE : DISCONNECTED");
					break;
					
				case ConnectionState.Disconnecting:
						
					PlayMakerFSM.BroadcastEvent ("PHOTON / STATE : DISCONNECTING");
					break;
					
				case ConnectionState.InitializingApplication:
						
					PlayMakerFSM.BroadcastEvent ("PHOTON / STATE : INITIALIZING APPLICATION");
					break;
			}
		}
	}// Update_connectionStateWatcher
	
	#endregion

	#region Photon Messages
	
	
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='player'>
	/// Player.
	/// </param>
	void OnPhotonPlayerConnected (PhotonPlayer player)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy: OnPhotonPlayerConnected: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON PLAYER CONNECTED");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='player'>
	/// Player.
	/// </param>
	void OnPhotonPlayerDisconnected (PhotonPlayer player)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPlayerDisconneced: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON PLAYER DISCONNECTED");
	}
	

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnJoinedRoom ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnJoinedRoom: "+Time.timeSinceLevelLoad);
		}
		
		/*
		//------- THAT WORKS ---------//
		var list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);
			
		foreach (PlayMakerFSM fsm in list){
			
			if (true)
			{
				fsm.SendEvent("PHOTON / JOINED ROOM"); // fine
			}else{	
				fsm.Fsm.ProcessEvent(FsmEvent.GetFsmEvent("PHOTON / JOINED ROOM")); // too late. same as PlayMakerFSM.BroadcastEvent()
			}
		}
		//-------
		
		// PlayMakerFSM.BroadcastEvent ("PHOTON / JOINED ROOM");  DO NOT WORK, too late.
		*/
		
	 	PlayMakerPhotonProxy.BroadCastToAll("PHOTON / JOINED ROOM");
			
		Debug.Log("post PHOTON / JOINED ROOM broadcasting");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnCreatedRoom ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnCreatedRoom: ");
		}
		PlayMakerFSM.BroadcastEvent ("PHOTON / CREATED ROOM");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonCreateRoomFailed ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonCreateRoomFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON CREATE ROOM FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonJoinRoomFailed ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonJoinRoomFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON JOIN ROOM FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonRandomJoinFailed ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonRandomJoinFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON RANDOM JOIN FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnLeftRoom ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnLeftRoom (local)");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / LEFT ROOM");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnReceivedRoomList ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnReceivedRoomList");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / RECEIVED ROOM LIST");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnReceivedRoomListUpdate ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnReceivedRoomListUpdate");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / RECEIVED ROOM LIST UPDATE");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='player'>
	/// Player.
	/// </param>
	void OnMasterClientSwitched (PhotonPlayer player)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnMasterClientSwitched: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / MASTER CLIENT SWITCHED");    
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnConnectedToPhoton ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectedToPhoton");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTED TO PHOTON");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnConnectedToMaster ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectedToMaster");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTED TO MASTER");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnDisconnectedFromPhoton ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnDisconnectedFromPhoton");
		}
        
		PlayMakerFSM.BroadcastEvent ("PHOTON / DISCONNECTED FROM PHOTON");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	/// <param name='parameters'>
	/// Parameters. TODO: NOT IMPLEMENTED. not sure what to expect from this object.
	/// </param>
	void OnConnectionFail (DisconnectCause cause)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectionFail " + cause);
		}
		
		lastDisconnectCause = cause;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTION FAIL");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	/// <param name='parameters'>
	/// Parameters. TODO: NOT IMPLEMENTED. not sure what to expect from this object.
	/// </param>
	void OnFailedToConnectToPhoton (DisconnectCause cause)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnFailedToConnectToPhoton " + cause);
		}
		
		lastDisconnectCause = cause;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / FAILED TO CONNECT TO PHOTON");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnJoinedLobby ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnJoinedLobby");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / JOINED LOBBY");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnLeftLobby ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnLeftLobby");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / LEFT LOBBY");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonMaxCccuReached()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonMaxCccuReached");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / MAX CCCU REACHED");
	}
	
	
	void OnPhotonCustomRoomPropertiesChanged()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonCustomRoomPropertiesChanged");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / CUSTOM ROOM PROPERTIES CHANGED");
	}
	
	void OnPhotonPlayerPropertiesChanged(PhotonPlayer player)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonPlayerPropertiesChanged:"+ player);
		}
		
		lastMessagePhotonPlayer = player;
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / PLAYER PROPERTIES CHANGED");
	}
	

	#endregion
	
	
	#region utils
	
	// this is a fix to the regular BroadCast that for some reason delivers events too late when player joins a room
	
	public static void BroadCastToAll(string fsmEvent)
	{
	
		var list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);
			
		foreach (PlayMakerFSM fsm in list){
			
			//if (true)
			//{
				fsm.SendEvent(fsmEvent); // fine
			//}else{	
			//	fsm.Fsm.ProcessEvent(FsmEvent.GetFsmEvent(fsmEvent)); // too late. This is the PlayMakerFsm.BroadcastEvent() way as far as I can tell.
			//}
		}
		
	}
	
	public static bool ApplyValueToFsmVar(Fsm fromFsm,FsmVar fsmVar, object value)
	{

			System.Type valueType = value.GetType();
			System.Type storageType = null;
			switch (fsmVar.Type)
			{
			case VariableType.Int:
				storageType = typeof(int);
				break;
			case VariableType.Float:
				storageType = typeof(float);
				break;
			case VariableType.Bool:
				storageType = typeof(bool);
				break;
			case VariableType.Color:
				storageType = typeof(Color);
				break;
			case VariableType.GameObject:
				storageType = typeof(GameObject);
				break;
			case VariableType.Quaternion:
				storageType = typeof(Quaternion);
				break;
			case VariableType.Rect:
				storageType = typeof(Rect);
				break;
			case VariableType.String:
				storageType = typeof(string);
				break;
			case VariableType.Texture:
				storageType = typeof(Texture2D);
				break;
			case VariableType.Vector2:
				storageType = typeof(Vector2);
				break;
			case VariableType.Vector3:
				storageType = typeof(Vector3);
				break;
			case VariableType.Object:
				storageType = typeof(Object);
				break;
			case VariableType.Material:
				storageType = typeof(Material);
				break;
			}
			
			
			if (! storageType.Equals(valueType))
			{
				if (valueType.Equals(typeof(ProceduralMaterial)))
				{
					// we are still ok.
				}else{
					Debug.LogError("The fsmVar value <"+storageType+"> doesn't match the value <"+valueType+">");
					return false;
				}
			}
			
	
			if (valueType == typeof(bool) ){
				FsmBool _target= fromFsm.Variables.GetFsmBool(fsmVar.variableName);
				_target.Value = (bool)value;
			
			}else if(valueType == typeof(Color) ){
				FsmColor _target= fromFsm.Variables.GetFsmColor(fsmVar.variableName);
				_target.Value = (Color)value;
			
			}else if(valueType == typeof(int)){
				FsmInt _target= fromFsm.Variables.GetFsmInt(fsmVar.variableName);
				_target.Value = (int)value;
			
			}else if(valueType == typeof(float) ){
				FsmFloat _target= fromFsm.Variables.GetFsmFloat(fsmVar.variableName);
				_target.Value = (float)value;
			
			}else if(valueType == typeof(GameObject) ){	
				FsmGameObject _target= fromFsm.Variables.GetFsmGameObject(fsmVar.variableName);
				_target.Value = (GameObject)value;
			
			}else if(valueType == typeof(Material) ){
			 	FsmMaterial _target= fromFsm.Variables.GetFsmMaterial(fsmVar.variableName);
				_target.Value = (Material)value;
			
			}else if(valueType == typeof(ProceduralMaterial) ){
			 	FsmMaterial _target= fromFsm.Variables.GetFsmMaterial(fsmVar.variableName);
				_target.Value = (ProceduralMaterial)value;
			
			}else if(valueType == typeof(Object) ){
				FsmObject _target= fromFsm.Variables.GetFsmObject(fsmVar.variableName);
				_target.Value = (Object)value;
			
			}else if(valueType == typeof(Quaternion) ){
				FsmQuaternion _target= fromFsm.Variables.GetFsmQuaternion(fsmVar.variableName);
				_target.Value = (Quaternion)value;
			
			}else if(valueType == typeof(Rect) ){
				FsmRect _target= fromFsm.Variables.GetFsmRect(fsmVar.variableName);
				_target.Value = (Rect)value;
			
			}else if(valueType == typeof(string) ){
				FsmString _target= fromFsm.Variables.GetFsmString(fsmVar.variableName);
				_target.Value = (string)value;
			
			}else if(valueType == typeof(Texture2D) ){
				FsmTexture _target= fromFsm.Variables.GetFsmTexture(fsmVar.variableName);
				_target.Value = (Texture2D)value;
			
			}else if(valueType == typeof(Vector2) ){
				FsmVector2 _target= fromFsm.Variables.GetFsmVector2(fsmVar.variableName);
				_target.Value = (Vector2)value;
			
			}else if(valueType == typeof(Vector3) ){
				FsmVector3 _target= fromFsm.Variables.GetFsmVector3(fsmVar.variableName);
				_target.Value = (Vector3)value;
			
			}else{
				Debug.LogWarning("?!?!"+valueType);
				//  don't know, should I put in FsmObject?	
			}
		
		return true;
	}
	
	
	public static object GetValueFromFsmVar(Fsm fromFsm,FsmVar fsmVar)
	{
		if (fsmVar.useVariable)
		{
			string _name = fsmVar.variableName;
			
			switch (fsmVar.Type){
				case VariableType.Int:
					return fromFsm.Variables.GetFsmInt(_name).Value;
				case VariableType.Float:
					return fromFsm.Variables.GetFsmFloat(_name).Value;
				case VariableType.Bool:
					return fromFsm.Variables.GetFsmBool(_name).Value;
				case VariableType.Color:
					return fromFsm.Variables.GetFsmColor(_name).Value;
				case VariableType.Quaternion:
					return fromFsm.Variables.GetFsmQuaternion(_name).Value;
				case VariableType.Rect:
					return fromFsm.Variables.GetFsmRect(_name).Value;
				case VariableType.Vector2:
					return fromFsm.Variables.GetFsmVector2(_name).Value;
				case VariableType.Vector3:
					return fromFsm.Variables.GetFsmVector3(_name).Value;
				case VariableType.Texture:
					return fromFsm.Variables.GetFsmTexture(_name).Value;
				case VariableType.String:
					return fromFsm.Variables.GetFsmString(_name).Value;
				case VariableType.GameObject:
					return fromFsm.Variables.GetFsmGameObject(_name).Value;
				case VariableType.Object:
					return fromFsm.Variables.GetFsmObject(_name).Value;
			}
			
		}else{
			
			switch (fsmVar.Type){
				case VariableType.Int:
					return fsmVar.intValue;
				case VariableType.Float:
					return fsmVar.floatValue;
				case VariableType.Bool:
					return fsmVar.boolValue;
				case VariableType.Color:
					return fsmVar.colorValue;
				case VariableType.Quaternion:
					return fsmVar.quaternionValue;
				case VariableType.Rect:
					return fsmVar.rectValue;
				case VariableType.Vector2:
					return fsmVar.vector2Value;
				case VariableType.Vector3:
					return fsmVar.vector3Value;
				case VariableType.Texture:
					return fsmVar.textureValue;
				case VariableType.String:
					return fsmVar.stringValue;
				case VariableType.GameObject:
					return fsmVar.gameObjectValue;
				case VariableType.Object:
					return fsmVar.objectReference;
			}
		}
		
	
		return null;
	}
	
	#endregion
}