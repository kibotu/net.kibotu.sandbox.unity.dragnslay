// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Play maker photon variable types. Simply for better code and being able to reference conveniently the variables types to stream.
/// Used in PlayMakerPhotonFsmVariableDefinition
/// TODO: expand to support all possible types.
/// </summary>
using HutongGames.PlayMaker;

	public enum PlayMakerPhotonVarTypes
	{
		Bool,
		Int,
		Float,
		String,
		Vector2,
		Vector3,
		Quaternion,
		Rect,
		Color,
	}


/// <summary>
/// PlayMaker photon synch proxy.
/// This behavior implements the underlying data serialization necessary to synchronize data for a given Fsm Component having some of its variables checked for network synching.
/// This behavior is also observed by a PhotonView. This is mandatory for the serialization over the photon network to happen.
/// the required set up described above is NOT YET checked. 
/// TODO: implement runtime or editor time verification that the set up is right. The problem is I can't find a way to get notified about instanciation so I can't run a check on
/// a gameObject created by an instanciating from the network.
/// </summary>
public class PlayMakerPhotonView : MonoBehaviour {
	
	
	
	/// <summary>
	/// The fsm Component being observed. 
	/// We implement the setter to set up FsmVariable as soon as possible, 
	/// else the photonView will miss it and create errors as we start streaming the fsm vars too late ( we have to do it before the Start() )
	/// </summary>
	public PlayMakerFSM observed
	{
		set{
			_observed = value;
			SetUpFsmVariableToSynch();
		}
		
		get{
			return _observed;
		}
	}
	
	private PlayMakerFSM _observed;
	
	/// <summary>
	/// Holds all the variables references to read from and write to during serialization.
	/// </summary>
	private ArrayList variableLOT;
	
	
	/// <summary>
	/// call base
	/// </summary>	
	private void Awake()
    {
		variableLOT = new ArrayList();

    }// Awake

	/// <summary>
	/// Sets up fsm variable caching for synch.
	/// It scans the observed Fsm for all variable checked for network synching, and store the required info about them using PlayMakerPhotonFsmVariableDefinition
 	/// It store all these variables in  variableLOT to be iterated during stream read and write.
 	/// TODO: implement all possible variables to synch.
	/// </summary>
	private void SetUpFsmVariableToSynch()
	{
		
		// fill the variableLOT with all the networksynched Fsmvariables.
	
		// bool
		foreach(FsmBool fsmBool in  observed.FsmVariables.BoolVariables)
		{
			if (fsmBool.NetworkSync){
				Debug.Log ("network synched FsmBool: '"+fsmBool.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmBool.Name;
				varDef.type = PlayMakerPhotonVarTypes.Bool;
				varDef.FsmBoolPointer = fsmBool;
				variableLOT.Add(varDef);
			}
		}
	
		
		// int
		foreach(FsmInt fsmInt in  observed.FsmVariables.IntVariables)
		{
			if (fsmInt.NetworkSync){
				Debug.Log ("network synched fsmInt: '"+fsmInt.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmInt.Name;
				varDef.type = PlayMakerPhotonVarTypes.Int;
				varDef.FsmIntPointer = fsmInt;
				variableLOT.Add(varDef);
			}
		}
	
		// float
		foreach(FsmFloat fsmFloat in  observed.FsmVariables.FloatVariables)
		{
			if (fsmFloat.NetworkSync){
				Debug.Log ("network synched FsmFloat: '"+fsmFloat.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmFloat.Name;
				varDef.type = PlayMakerPhotonVarTypes.Float;
				varDef.FsmFloatPointer = fsmFloat;
				variableLOT.Add(varDef);
			}
		}

		// string
		foreach(FsmString fsmString in  observed.FsmVariables.StringVariables)
		{
			if (fsmString.NetworkSync){
				Debug.Log ("network synched FsmString: '"+fsmString.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmString.Name;
				varDef.type = PlayMakerPhotonVarTypes.String;
				varDef.FsmStringPointer = fsmString;
				variableLOT.Add(varDef);
			}
		}
	
		// vector2
		foreach(FsmVector2 fsmVector2 in  observed.FsmVariables.Vector2Variables)
		{
			if (fsmVector2.NetworkSync){
				Debug.Log ("network synched fsmVector2: '"+fsmVector2.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmVector2.Name;
				varDef.type = PlayMakerPhotonVarTypes.Vector2;
				varDef.FsmVector2Pointer = fsmVector2;
				variableLOT.Add(varDef);
			}
		}
		
		// vector3
		foreach(FsmVector3 fsmVector3 in  observed.FsmVariables.Vector3Variables)
		{
			if (fsmVector3.NetworkSync){
				Debug.Log ("network synched fsmVector3: '"+fsmVector3.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmVector3.Name;
				varDef.type = PlayMakerPhotonVarTypes.Vector3;
				varDef.FsmVector3Pointer = fsmVector3;
				variableLOT.Add(varDef);
			}
		}

		// quaternion
		foreach(FsmQuaternion fsmQuaternion in  observed.FsmVariables.QuaternionVariables)
		{
			if (fsmQuaternion.NetworkSync){
				Debug.Log ("network synched fsmQuaternion: '"+fsmQuaternion.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmQuaternion.Name;
				varDef.type = PlayMakerPhotonVarTypes.Quaternion;
				varDef.FsmQuaternionPointer = fsmQuaternion;
				variableLOT.Add(varDef);
			}
		}

		// rect
		foreach(FsmRect fsmRect in  observed.FsmVariables.RectVariables)
		{
			if (fsmRect.NetworkSync){
				Debug.Log ("network synched fsmRect: '"+fsmRect.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmRect.Name;
				varDef.type = PlayMakerPhotonVarTypes.Rect;
				varDef.FsmRectPointer = fsmRect;
				variableLOT.Add(varDef);
			}
		}
		
		// color
		foreach(FsmColor fsmColor in  observed.FsmVariables.ColorVariables)
		{
			if (fsmColor.NetworkSync){
				Debug.Log ("network synched fsmColor: '"+fsmColor.Name +"' in fsm:'" +observed.Fsm.Name+"' in gameObject:'"+observed.gameObject.name+"'");
			
				PlayMakerPhotonFsmVariableDefinition varDef = new PlayMakerPhotonFsmVariableDefinition();
				varDef.name = fsmColor.Name;
				varDef.type = PlayMakerPhotonVarTypes.Color;
				varDef.FsmColorPointer = fsmColor;
				variableLOT.Add(varDef);
			}
		}
		
	}// SetUpFsmVariableToSynch
	 
	
	#region serialization
	
	/// <summary>
	/// The serialization required for playmaker integration. This is transparent for the user.
	/// 1: Add the "PlaymakerPhotonView" component to observe this fsm,
    /// 2: Add a "photonView" component to observe that "PlaymakerPhotonView" component
    /// 3: Check "network synch" in the Fsm variables you want to synch over the network, 
    /// 
    /// This setup is required For each Fsm. So if on one GameObject , several Fsm wants to sync variables, 
    /// you need a "PlaymakerPhotonView" and "PhotonView" setup for each.
    /// 
	/// TODO: this might very well need improvment or reconsideration. AT least some editor or runtime check and helpers.
	/// I am thinking of letting the user only add a "photonView" observing the fsm, and at runtime insert the "PlaymakerPhotonView" in between.
	/// But I can't run this check when an instanciation occurs as I have no notifications of this.
	/// 
	/// </summary>
	/// <param name='stream'>
	/// Stream of data
	/// </param>
	/// <param name='info'>
	/// Info.
	/// </param>
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
			
			// go through the Look up table and send in order.
			foreach( PlayMakerPhotonFsmVariableDefinition varDef in variableLOT )
       		{
				
				switch (varDef.type)
				{
					case PlayMakerPhotonVarTypes.Bool:
						stream.SendNext(varDef.FsmBoolPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Int:
						stream.SendNext(varDef.FsmIntPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Float:
						stream.SendNext(varDef.FsmFloatPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.String:
						stream.SendNext(varDef.FsmStringPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Vector2:
						stream.SendNext(varDef.FsmVector2Pointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Vector3:
						stream.SendNext(varDef.FsmVector3Pointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Quaternion:
						stream.SendNext(varDef.FsmQuaternionPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Rect:
						stream.SendNext(varDef.FsmRectPointer.Value); 
						break;
					case PlayMakerPhotonVarTypes.Color:
						stream.SendNext(varDef.FsmColorPointer.Value); 
						break;
				}	
       		}

        }else{	
			
			
			// go through the Look up table and read in order.
			foreach( PlayMakerPhotonFsmVariableDefinition varDef in variableLOT )
       		{
				switch (varDef.type)
				{
					case PlayMakerPhotonVarTypes.Bool:			
						varDef.FsmBoolPointer.Value = (bool)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Int:
						varDef.FsmIntPointer.Value = (int)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Float:
						varDef.FsmFloatPointer.Value = (float)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.String:
						varDef.FsmStringPointer.Value = (string)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Vector2:
						varDef.FsmVector2Pointer.Value = (Vector2)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Vector3:
						varDef.FsmVector3Pointer.Value = (Vector3)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Quaternion:
						varDef.FsmQuaternionPointer.Value = (Quaternion)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Rect:
						varDef.FsmRectPointer.Value = (Rect)stream.ReceiveNext();
						break;
					case PlayMakerPhotonVarTypes.Color:
						varDef.FsmColorPointer.Value = (Color)stream.ReceiveNext();
						break;
				}	
       		}

        }// reading or writing
    }
	
	#endregion
	
}

/// <summary>
/// Allow a convenient description of the Fsm variable that needs streaming.  
/// Also let the reference be cached instead of accessed everytime. Make the stream function easier to script and potenitaly help performances hopefully.
/// </summary>
public class PlayMakerPhotonFsmVariableDefinition
{

	/// <summary>
	/// The name of the Fsm Variable. Within a given Fsm, variables names have to be unique, so we are ok.
	/// </summary>
	public string name;
	
	/// <summary>
	/// Store the type conviniently instead of messing with type during the streaming.
	/// </summary>
	public PlayMakerPhotonVarTypes type;
	
	/// <summary>
	/// The fsm bool pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmBool FsmBoolPointer;
	
	/// <summary>
	/// The fsm int pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmInt FsmIntPointer;
	
	/// <summary>
	/// The fsm float pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmFloat FsmFloatPointer;
	
	/// <summary>
	/// The fsm string pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmString FsmStringPointer;

	/// <summary>
	/// The fsm vector3 pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmVector2 FsmVector2Pointer;
	
	/// <summary>
	/// The fsm vector3 pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmVector3 FsmVector3Pointer;

	/// <summary>
	/// The fsm quaternion pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmQuaternion FsmQuaternionPointer;
	
	/// <summary>
	/// The fsm rect pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmRect FsmRectPointer;

	/// <summary>
	/// The fsm color pointer. Set Only if type correspond. This is for convenient caching without loosing the type.
	/// </summary>
	public FsmColor FsmColorPointer;
}
