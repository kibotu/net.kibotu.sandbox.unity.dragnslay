// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Spot Angle of a Light.")]
	public class SetLightSpotAngle : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public FsmFloat lightSpotAngle;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightSpotAngle = 20f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightRange();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoSetLightRange();
		}
		
		void DoSetLightRange()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			Light light = go.light;
			if (light == null)
			{
				LogError("Missing Light Component!");
				return;
			}
			
			light.spotAngle = lightSpotAngle.Value;
		}
	}
}