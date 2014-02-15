// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Creates a Game Object on all clients in a Photon network game. \n A PlayMakerPhotonGameObjectProxy component is required on the gameObject.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W912")]
	public class PhotonNetworkInstantiate : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerPhotonGameObjectProxy))]
		[Tooltip("GameObject to create. Usually a Prefab.\n A PlayMakerPhotonGameObjectProxy component is required on the gameObject.")]
		public FsmGameObject gameObject;

		[Tooltip("Optional Spawn Point.")]
		public FsmGameObject spawnPoint;

		[Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
		public FsmVector3 position;

		[Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
		public FsmVector3 rotation;
		
		[Tooltip("Usually 0. The group number allows you to group together Photon network messages which allows you to filter them if so desired.")]
		public FsmInt networkGroup;
		
		[ActionSection("")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally store the created object.")]
		public FsmGameObject storeObject;

		

		public override void Reset()
		{
			gameObject = null;
			spawnPoint = null;
			position = new FsmVector3 { UseVariable = true };
			rotation = new FsmVector3 { UseVariable = true };
			storeObject = null;
			networkGroup = 0;
		}

		public override void OnEnter()
		{
			doInstantiate();
			
			Finish();
		}
		
		
		void doInstantiate()
		{
			
			var go = gameObject.Value;

			if (go != null)
			{
			
				if (! runSanityCheckOnGameObject() )
				{
					return;
				}
				
				var spawnPosition = Vector3.zero;
				var spawnRotation = Vector3.up;

				if (spawnPoint.Value != null)
				{
					spawnPosition = spawnPoint.Value.transform.position;

					if (!position.IsNone)
					{
						spawnPosition += position.Value;
					}

					spawnRotation = !rotation.IsNone ? rotation.Value : spawnPoint.Value.transform.eulerAngles;
				}
				else
				{
					if (!position.IsNone)
					{
						spawnPosition = position.Value;
					}

					if (!rotation.IsNone)
					{
						spawnRotation = rotation.Value;
					}
				}
				
	
				GameObject newObject =	PhotonNetwork.Instantiate(go.name, spawnPosition, Quaternion.Euler(spawnRotation), networkGroup.Value);
				if(storeObject !=null)
				{
					storeObject.Value = newObject;
					
					
				}
			}
			
			
		}// doInstantiate
		
		
		
		/// <summary>
		/// Runs the sanity check on game object. It must have for example a PlayMakerPhotonGameObjectProxy component attached for example.
		/// </summary>
		bool runSanityCheckOnGameObject()
		{
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
		
			if (go == null )
			{
				return false;
			}
		
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				return false;
			}
			
			return _proxy.ValidatePreFlightCheckOnGameObject(gameObject.Value);
			
		}//doRunSanityCheckOnGameObject
		

	}
}