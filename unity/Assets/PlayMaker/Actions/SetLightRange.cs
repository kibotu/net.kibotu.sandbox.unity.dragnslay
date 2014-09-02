// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Range of a Light.")]
	public class SetLightRange : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;
		public FsmFloat lightRange;
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightRange = 20f;
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
			
			light.range = lightRange.Value;
		}
	}
}