// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets Field of View used by the Camera.")]
	public class SetCameraFOV : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Camera))]
		public FsmOwnerDefault gameObject;
		[RequiredField]
		public FsmFloat fieldOfView;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			fieldOfView = 50f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetCameraFOV();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetCameraFOV();
		}
		
		void DoSetCameraFOV()
		{
			GameObject go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
			if (go == null) return;
			
			Camera camera = go.camera;
			
			if (camera == null)
			{
				LogError("Missing Camera Component!");
				return;
			}
			
			camera.fieldOfView = fieldOfView.Value;
		}
	}
}