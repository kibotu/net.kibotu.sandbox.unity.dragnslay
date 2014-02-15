// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.
/// !!!!!!!!!!!!!!! OBSOLETE !!!!!!!!!!!!!
/// 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;

/// <summary>
/// --------------------------------- OBSOLETE !!!!!!!!!!!!! 
/// --------------------------------- LOOK FOR "PlayMakerPhotonView" for the current implementation of Fsm variable synch
/// This script is not in used, it's only here as a back up if the current implementation is turned down.
/// The benefit from this implement is the fact that we could synchronize global variables across the network. which would be something cool to have.
/// ---------------------------------
/// PlayMaker photon synch proxy.
/// This behavior implements the underlying data serialization necessary to synchronize data.
/// It is paired with the action "PhotonViewSyncData" that lets the user define which variables should be serialized across the network.
/// 
/// This is totally transparent for the user. By simply declaring which Fsm variables to synch using "PhotonViewSyncData", the user effectivly access serialization
/// 
/// the current implementation uses a Look up table of all variables to serialize and use this table to maintain order within the photon stream.
/// 
///  TODO: maybe this is can be improved by accessing the "network sync" check box of Fsm variable instead of having to use the "PhotonViewSyncData" action.
/// 
///  TODO: maybe implementing getters and setters would provide cleaner code.
/// </summary>
public class PlayMakerPhotonSynchProxy : MonoBehaviour {
	
	
	/// <summary>
	/// Holds all the variables references to read from and write to during serialization.
	/// </summary>
	private Hashtable variableLOT;
	


	/// <summary>
	/// set up the variable look up table
	/// </summary>
	void Awake()
    {
		variableLOT = new Hashtable();
	}


	
	#region WRITING
	/// <summary>
	/// Synchs the bool variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchBoolVariable(string name,bool val)
	{
		variableLOT[name] = val;
	}
	
	/// <summary>
	/// Synchs the int variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchIntVariable(string name,int val)
	{
		variableLOT[name] = val;
	}
	
	/// <summary>
	/// Synchs the float variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchFloatVariable(string name,float val)
	{
		variableLOT[name] = val;
	}
	
	/// <summary>
	/// Synchs the vector3 variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchVector3Variable(string name,Vector3 val)
	{
		variableLOT[name] = val;
	}
	
	/// <summary>
	/// Synchs the quaternion variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchQuaternionVariable(string name,Quaternion val)
	{
		variableLOT[name] = val;
	}
	
	/// <summary>
	/// Synchs the string variable. This effectivly writes to the stream.
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='val'>
	/// Value.
	/// </param>
	public void synchStringVariable(string name,string val)
	{
		variableLOT[name] = val;
	}
	
	#endregion
	
	#region READING
	
	/// <summary>
	/// Gets the bool variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The bool variable.
	/// </returns>
	/// <param name='name'>
	/// the variable name
	/// </param>
	public bool getBoolVariable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("bool variable '"+name+"' missing");
			return false;
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("bool variable '"+name+"' null");
			return false;		
		}
		
		return (bool)variableLOT[name];
	}
	
	/// <summary>
	/// Gets the int variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The int variable.
	/// </returns>
	/// <param name='name'>
	/// the variable name
	/// </param>
	public int getIntVariable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("int variable '"+name+"' missing");
			return 0;
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("int variable '"+name+"' null");
			return 0;		
		}
		
		return (int)variableLOT[name];
	}
	
	/// <summary>
	/// Gets the float variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The float variable.
	/// </returns>
	/// <param name='name'>
	/// The variable name
	/// </param>
	public float getFloatVariable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("Float variable '"+name+"' missing");
			return 0f;
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("Float variable '"+name+"' null");
			return 0f;
		}
		
		return (float)variableLOT[name];
	}
	
	/// <summary>
	/// Gets the vector3 variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The vector3 variable.
	/// </returns>
	/// <param name='name'>
	/// The variable name
	/// </param>
	public Vector3 getVector3Variable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("Vector3 variable '"+name+"' missing");
			return Vector3.zero;
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("Vector3 variable '"+name+"' null");
			return Vector3.zero;
		}
		
		return (Vector3)variableLOT[name];
	}
	
	/// <summary>
	/// Gets the quaternion variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The quaternion variable.
	/// </returns>
	/// <param name='name'>
	/// The variable name
	/// </param>
	public Quaternion getQuaternionVariable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("Quaternion variable '"+name+"' missing");
			return Quaternion.identity;
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("Quaternion variable '"+name+"' null");
			return Quaternion.identity;		
		}
		
		return (Quaternion)variableLOT[name];
	}
	
	/// <summary>
	/// Gets the string variable. This effectivly read from the stream.
	/// </summary>
	/// <returns>
	/// The string variable.
	/// </returns>
	/// <param name='name'>
	/// The variable name
	/// </param>
	public string getStringVariable(string name)
	{
		if (!variableLOT.ContainsKey(name))
		{
			Debug.LogWarning("String variable '"+name+"' missing");
			return "";
		}
		
		if (variableLOT[name] ==null){
			Debug.LogWarning("String variable '"+name+"' null");
			return "";		
		}
		
		return (string)variableLOT[name];
	}
	
	#endregion

	#region serialization
	
	/// <summary>
	/// The Only serialization required for playmaker integration. This is transparent for the user.
	/// Use the action "PhotonViewSyncData" to define which Fsm variable will be serialized here.
	/// 
	/// TODO: this might very well need improvment or reconsideration.
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
			foreach( DictionaryEntry variable in variableLOT )
       		{
            	stream.SendNext(variable.Value); 	
       		}
          
        }else{	
			
			// I have to clone it else, I get mismatches ( I think due to threading. While It iterate, variables can be set and that cause problems.)
		 	Hashtable _temp = (Hashtable)variableLOT.Clone();
			
			// go through the Look up table and receive in order.
			foreach( DictionaryEntry variable in _temp  )
       		{
				string key = (string)variable.Key;

				if (variableLOT.ContainsKey(key) && variableLOT[key]!=null){
					
					if(typeof(bool) == variableLOT[key].GetType())
					{
						variableLOT[key] = (bool)stream.ReceiveNext();
					}
					else if(typeof(int) == variableLOT[key].GetType())
					{
						variableLOT[key] = (int)stream.ReceiveNext();
					}
					else if(typeof(float) == variableLOT[key].GetType())
					{
						variableLOT[key] = (float)stream.ReceiveNext();
					}
					else if(typeof(Vector3) == variableLOT[key].GetType())
					{
						variableLOT[key] = (Vector3)stream.ReceiveNext();
					}
					else if(typeof(Quaternion) == variableLOT[key].GetType())
					{
						variableLOT[key] = (Quaternion)stream.ReceiveNext();
					}
				}
				
       		}// foreach variables
            
        }// reading or writing
    }
	
	#endregion
	
}
